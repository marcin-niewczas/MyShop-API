using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyShop.Application.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Infrastructure.DocumentsGenerator;
internal static class Extensions
{
    public static IServiceCollection AddDocumentsGenerator(
        this IServiceCollection services      
        )
    {
        services.AddScoped<IInvoiceGenerator, InvoicePdfGenerator>();

        return services;
    }
}
