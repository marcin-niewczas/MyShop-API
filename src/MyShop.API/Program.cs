using MyShop.API;
using MyShop.API.ApiEndpoints;
using MyShop.Application;
using MyShop.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddApi(builder.Host)
    .AddInfrastructure(builder.Environment, builder.Configuration)
    .AddApplication();

var app = builder.Build();

app.UseInfrastructure(builder.Configuration)
   .UseApplication(builder.Configuration)
   .MapApiEndpoints();


app.Run();
