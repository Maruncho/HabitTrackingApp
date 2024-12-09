using HTApp.Core.API;
using HTApp.Core.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace HTApp.Infrastructure.Extensions;

public static class HTAppServicesExtensions
{
    public static IServiceCollection AddHTAppServices(this IServiceCollection services)
    {
        services.AddHTAppRepositories();
        //Don't change to transient if you want the Subject-Observers to function.
        services.AddScoped<IGoodHabitService, GoodHabitService>();
        services.AddScoped<IBadHabitService, BadHabitService>();
        services.AddScoped<ITreatService, TreatService>();
        services.AddScoped<ITransactionService, TransactionService>();
        services.AddScoped<IUserDataService, UserDataService>();
        services.AddScoped<ISessionService, SessionService>();
        return services;
    }

    public static IApplicationBuilder EnableHTAppServicesObserverPattern(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<EnableHtAppServicesObserverPatternMiddleware>();
    }
}

public class EnableHtAppServicesObserverPatternMiddleware
{
    private readonly RequestDelegate _next;

    public EnableHtAppServicesObserverPatternMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    // IMessageWriter is injected into InvokeAsync
    public async Task Invoke(HttpContext httpContext)
    {
        //please don't break it, whoever is refactoring this codebase.
        var servicesAssembly = Assembly.GetAssembly(typeof(IUserDataService));
        if (servicesAssembly is null)
        {
            throw new Exception("Please fix the extension method.");
        }

        foreach (var iService in servicesAssembly.GetExportedTypes().Where(s => s.Name.Contains("Service")))
        {
            if(iService.GetInterfaces().Any(iface => iface.Name.Contains("Observer")))
            {
                httpContext.RequestServices.GetService(iService);
            }
        }

        await _next(httpContext);
    }
}
