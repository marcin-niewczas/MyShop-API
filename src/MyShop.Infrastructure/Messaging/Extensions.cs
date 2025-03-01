using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyShop.Application.Abstractions;
using MyShop.Application.Options;
using MyShop.Infrastructure.Commands.Handlers;
using MyShop.Infrastructure.Events.Handlers;
using MyShop.Infrastructure.Messaging.Brokers;
using MyShop.Infrastructure.Messaging.Dispatchers;
using MyShop.Infrastructure.Messaging.Dispatchers.Interfaces;
using MyShop.Infrastructure.Options;
using System.Reflection;

namespace MyShop.Infrastructure.Messaging;
internal static class Extensions
{
    public static IServiceCollection AddMessaging(this IServiceCollection services, IConfiguration configuration)
    {
        var messagingOptions = configuration.GetOptions<MessagingOptions>(MessagingOptions.Section);

        if (messagingOptions.UseRabbitMQ)
        {
            services.AddMassTransit(mt =>
            {
                mt.AddConsumer<AuthEmailHasBeenChangedEventHandler>();
                mt.AddConsumer<AuthPasswordHasBeenChangedEventHandler>();

                mt.AddConsumer<ChangeOrdersPaymentStatusCommandHandler>();
                mt.AddConsumer<OrderHasBeenCanceledEventHandler>();
                mt.AddConsumer<OrderHasBeenCreatedEventHandler>();
                mt.AddConsumer<OrderHasBeenUpdatedEventHandler>();

                mt.AddConsumer<PhotoHasBeenRemovedEventHandler>();

                mt.AddConsumer<PositionsOfProductVariantOptionValuesHasBeenChangedEventHandler>();
                mt.AddConsumer<PriceOfTheProductVariantHasBeenReducedEventHandler>();
                mt.AddConsumer<ProductVariantIsAvailableEventHandler>();
                mt.AddConsumer<ProductVariantOptionSortTypeHasBeenChangedToAlphabeticallyEventHandler>();
                mt.AddConsumer<ProductVariantOptionValueHasBeenAddednAlphabeticallyProductVariantOptionEventHandler>();
                mt.AddConsumer<ValueOfMainProductDetailOptionValueHasBeenUpdatedEventHandler>();
                mt.AddConsumer<ValueOfProductVariantOptionValueHasBeenUpdatedEventHandler>();

                mt.UsingRabbitMq((ctx, cfg) =>
                {
                    cfg.Host(messagingOptions.Hostname, "/", c =>
                    {
                        c.Username(messagingOptions.Username);
                        c.Password(messagingOptions.Password);
                    });

                    cfg.ReceiveEndpoint("auth-queue", re =>
                    {
                        re.ConfigureConsumer<AuthEmailHasBeenChangedEventHandler>(ctx);
                        re.ConfigureConsumer<AuthPasswordHasBeenChangedEventHandler>(ctx);
                    });

                    cfg.ReceiveEndpoint("orders-queue", re =>
                    {
                        re.ConfigureConsumer<ChangeOrdersPaymentStatusCommandHandler>(ctx);
                        re.ConfigureConsumer<OrderHasBeenCanceledEventHandler>(ctx);
                        re.ConfigureConsumer<OrderHasBeenCreatedEventHandler>(ctx);
                        re.ConfigureConsumer<OrderHasBeenUpdatedEventHandler>(ctx);
                    });

                    cfg.ReceiveEndpoint("photos-queue", re =>
                    {
                        re.ConfigureConsumer<PhotoHasBeenRemovedEventHandler>(ctx);
                    });

                    cfg.ReceiveEndpoint("products-queue", re =>
                    {
                        re.ConfigureConsumer<PositionsOfProductVariantOptionValuesHasBeenChangedEventHandler>(ctx);
                        re.ConfigureConsumer<PriceOfTheProductVariantHasBeenReducedEventHandler>(ctx);
                        re.ConfigureConsumer<ProductVariantIsAvailableEventHandler>(ctx);
                        re.ConfigureConsumer<ProductVariantOptionSortTypeHasBeenChangedToAlphabeticallyEventHandler>(ctx);
                        re.ConfigureConsumer<ProductVariantOptionValueHasBeenAddednAlphabeticallyProductVariantOptionEventHandler>(ctx);
                        re.ConfigureConsumer<ValueOfMainProductDetailOptionValueHasBeenUpdatedEventHandler>(ctx);
                        re.ConfigureConsumer<ValueOfProductVariantOptionValueHasBeenUpdatedEventHandler>(ctx);
                    });
                });
            });

            services.AddScoped<IMessageBroker, RabbitMQBroker>();
        }
        else
        {
            services.AddHostedService<MessageBackgroundDispatcher>();
            services.AddSingleton<IMessageBroker, MessageBroker>();
            services.AddSingleton<IMessageChannel, MessageChannel>();
            services.AddSingleton<IEventDispatcher, EventDispatcher>();
            services.AddSingleton<ICommandDispatcher, CommandDispatcher>();
            services.AddSingleton<IAsyncMessageDispatcher, AsyncMessageDispatcher>();
        }

        services.Scan(s => s.FromAssemblies(Assembly.GetExecutingAssembly(), Assembly.GetAssembly(typeof(Application.Extensions))!)
            .AddClasses(c => c.AssignableTo(typeof(IEventHandler<>))).AsImplementedInterfaces().WithScopedLifetime()
            );

        return services;
    }
}
