using MyShop.Application.Dtos.ECommerce.ShoppingCarts;
using MyShop.Application.Responses;
using MyShop.Application.Validations.Interfaces;
using MyShop.Core.Utils;
using MyShop.Core.Validations;
using MyShop.Core.ValueObjects.ShoppingCarts;
using System.Collections.ObjectModel;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MyShop.Application.Commands.ECommerce.ShoppingCarts;
[JsonConverter(typeof(UpdateShoppingCartItemsEcJsonConverter))]
public sealed class UpdateShoppingCartItemsEc(
    IList<UpdateShoppingCartItemEc> list
    ) : ReadOnlyCollection<UpdateShoppingCartItemEc>(list),
        ICommand<ApiResponse<ShoppingCartIdValueDictionaryEcDto>>,
        IValidatable
{
    public void Validate(ICollection<ValidationMessage> validationMessages)
    {
        if (Items.HasDuplicateBy(e => e.ShoppingCartItemId))
        {
            validationMessages.Add(
                new(
                    "*",
                    [$"{nameof(UpdateShoppingCartItemEc.ShoppingCartItemId)}s must contains only unique value."]
                ));
        }

        if (Items.Any(e => !ShoppingCartItemQuantity.IsValid(e.Quantity)))
        {
            validationMessages.Add(
                new(
                    "*",
                    [$"The {nameof(UpdateShoppingCartItemEc.Quantity)}s must be inclusive between {ShoppingCartItemQuantity.Min} and {ShoppingCartItemQuantity.Max}."]
                ));
        }
    }
}

public sealed record UpdateShoppingCartItemEc(
    Guid ShoppingCartItemId,
    int Quantity
    );

internal sealed class UpdateShoppingCartItemsEcJsonConverter : JsonConverter<UpdateShoppingCartItemsEc>
{
    public override UpdateShoppingCartItemsEc? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType is not JsonTokenType.StartArray)
        {
            throw new JsonException();
        }

        string? currentPropertyName = null;
        Guid? currentGuid = null;
        int? currentQuantity = null;

        List<UpdateShoppingCartItemEc> list = [];

        while (reader.Read())
        {
            if (reader.TokenType is JsonTokenType.EndArray)
            {
                if (currentGuid is not null || currentQuantity is not null)
                {
                    throw new JsonException();
                }

                continue;
            }

            if (reader.TokenType is JsonTokenType.StartObject)
            {
                if (currentGuid is not null || currentQuantity is not null)
                {
                    throw new JsonException();
                }

                continue;
            }

            if (reader.TokenType is JsonTokenType.EndObject)
            {
                if (currentGuid is null || currentQuantity is null)
                {
                    throw new JsonException();
                }

                if (currentGuid is Guid guid && currentQuantity is int quantity)
                {
                    list.Add(new(guid, quantity));

                    currentGuid = null;
                    currentQuantity = null;

                    continue;
                }
                else
                {
                    throw new JsonException();
                }
            }


            if (reader.TokenType is JsonTokenType.StartObject or JsonTokenType.EndObject)
            {
                continue;
            }

            if (reader.TokenType == JsonTokenType.PropertyName)
            {
                currentPropertyName = reader.GetString();

                if (currentPropertyName is null ||
                    !(nameof(UpdateShoppingCartItemEc.ShoppingCartItemId).Equals(currentPropertyName, StringComparison.CurrentCultureIgnoreCase) ||
                      nameof(UpdateShoppingCartItemEc.Quantity).Equals(currentPropertyName, StringComparison.CurrentCultureIgnoreCase))
                   )
                {
                    throw new JsonException();
                }

                if (nameof(UpdateShoppingCartItemEc.ShoppingCartItemId).Equals(currentPropertyName, StringComparison.CurrentCultureIgnoreCase) &&
                    currentGuid is not null ||
                    nameof(UpdateShoppingCartItemEc.Quantity).Equals(currentPropertyName, StringComparison.CurrentCultureIgnoreCase) &&
                    currentQuantity is not null
                    )
                {
                    throw new JsonException();
                }

                continue;
            }

            if (nameof(UpdateShoppingCartItemEc.ShoppingCartItemId).Equals(currentPropertyName, StringComparison.CurrentCultureIgnoreCase))
            {
                if (reader.TokenType is not JsonTokenType.String)
                {
                    throw new JsonException();
                }

                currentGuid = reader.GetGuid();

                continue;
            }

            if (nameof(UpdateShoppingCartItemEc.Quantity).Equals(currentPropertyName, StringComparison.CurrentCultureIgnoreCase))
            {
                if (reader.TokenType is not JsonTokenType.Number)
                {
                    throw new JsonException();
                }

                currentQuantity = reader.GetInt32();

                continue;
            }
        }

        return new(list);
    }

    public override void Write(Utf8JsonWriter writer, UpdateShoppingCartItemsEc value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, value, options);
    }
}
