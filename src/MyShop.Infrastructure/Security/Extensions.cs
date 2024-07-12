using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using MyShop.Application.Abstractions;
using MyShop.Application.Options;
using MyShop.Application.Utils;
using MyShop.Core.Abstractions;
using MyShop.Core.Models.Users;
using MyShop.Core.ValueObjects.Users;
using MyShop.Infrastructure.Options;
using System.Text;

namespace MyShop.Infrastructure.Security;
internal static class Extensions
{
    public static IServiceCollection AddSecurity(this IServiceCollection services, IConfiguration configuration)
    {
        var authOptions = configuration.GetOptions<AuthOptions>(AuthOptions.Section);

        services.AddAuthentication(o =>
        {
            o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
         .AddJwtBearer(o =>
         {
             o.Audience = authOptions.Audience;
             o.IncludeErrorDetails = true;
             o.TokenValidationParameters = new TokenValidationParameters
             {
                 ValidIssuer = authOptions.Issuer,
                 ClockSkew = TimeSpan.Zero,
                 IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authOptions.SigningKey))
             };

             o.Events = new JwtBearerEvents
             {
                 OnMessageReceived = context =>
                 {
                     var accessToken = context.Request.Query["access_token"];

                     var path = context.HttpContext.Request.Path;
                     if (!string.IsNullOrEmpty(accessToken) &&
                         (path.StartsWithSegments("/api/v1/hubs")))
                     {
                         context.Token = accessToken;
                     }
                     return Task.CompletedTask;
                 }
             };
         });

        services.AddAuthorizationBuilder()
            .AddPolicy(PolicyNames.HasGuestPermission, policy =>
            {
                policy.RequireRole(UserRole.Guest, UserRole.Customer, UserRole.Employee);
            })
            .AddPolicy(PolicyNames.HasCustomerPermission, policy =>
            {
                policy.RequireRole(UserRole.Customer, UserRole.Employee);
            })
            .AddPolicy(PolicyNames.HasEmployeePermission, policy =>
            {
                policy.RequireRole(UserRole.Employee);
            })
            .AddPolicy(PolicyNames.HasManagerPermission, policy =>
            {
                policy.RequireRole(UserRole.Employee);
                policy.RequireClaim(CustomClaimTypes.EmployeeRole, [EmployeeRole.SuperAdmin, EmployeeRole.Admin, EmployeeRole.Manager]);
            })
            .AddPolicy(PolicyNames.HasAdminPermission, policy =>
            {
                policy.RequireRole(UserRole.Employee);
                policy.RequireClaim(CustomClaimTypes.EmployeeRole, [EmployeeRole.SuperAdmin, EmployeeRole.Admin]);
            }).AddPolicy(PolicyNames.HasSuperAdminPermission, policy =>
            {
                policy.RequireRole(UserRole.Employee);
                policy.RequireClaim(CustomClaimTypes.EmployeeRole, EmployeeRole.SuperAdmin);
            });

        services
            .AddSingleton<IPasswordHasher<User>, PasswordHasher<User>>()
            .AddSingleton<IPasswordManager, PasswordManager>()
            .AddScoped<IUserClaimsService, UserClaimsService>()
            .AddScoped<IAuthenticator, Authenticator>();

        return services;
    }

    public static WebApplication UseSecurity(this WebApplication app, IConfiguration configuration)
    {
        var clientOptions = configuration.GetOptions<WebSPAClientOptions>(WebSPAClientOptions.Section);

        app.UseCors(builder =>
        {
            builder
            .AllowCredentials()
            .WithOrigins(clientOptions.AllowedOriginUrls.Select(url => url.OriginalString).ToArray())
            .AllowAnyMethod()
            .AllowAnyHeader();
        });

        app.UseAuthentication();
        app.UseAuthorization();

        return app;
    }
}
