using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyShop.Core.Abstractions;

namespace MyShop.Infrastructure.DataAccessLayer.MainDatabase.EntityConfigurations;
internal static class UtilsConfigurationExtension
{
    public static PropertyBuilder<TProperty> HasPricePrecision<TProperty>(this PropertyBuilder<TProperty> propertyBuilder)
        => propertyBuilder.HasPrecision(14, 2);

    public static PropertyBuilder<TEnum> HasEnumConversion<TEnum>(this PropertyBuilder<TEnum> propertyBuilder) where TEnum : Enum
        => propertyBuilder
            .HasConversion(
                v => v.ToString(),
                v => (TEnum)Enum.Parse(typeof(TEnum), v)
                );

    public static PropertyBuilder<TAllowedValues> HasAllowedValuesStringMaxLength<TAllowedValues>(
        this PropertyBuilder<TAllowedValues> propertyBuilder
        ) where TAllowedValues : IAllowedValues
            => propertyBuilder.HasMaxLength(TAllowedValues.AllowedValues.Cast<string>().MaxBy(s => s.Length)!.Length);
}
