using HTApp.Core.API;
using HTApp.Core.Services;
using Microsoft.Extensions.DependencyInjection;

namespace HTApp.Infrastructure.Extensions;

public static class HTAppServicesExtensions
{
    public static IServiceCollection AddHTAppServices(this IServiceCollection services)
    {
        services.AddHTAppRepositories();
        services.AddScoped<IGoodHabitService, GoodHabitService>();
        services.AddScoped<IBadHabitService, BadHabitService>();
        services.AddScoped<ITreatService, TreatService>();
        services.AddScoped<ITransactionService, TransactionService>();
        services.AddScoped<IUserDataService, UserDataService>();
        return services;
    }
}
