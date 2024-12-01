using HTApp.Core.Contracts;
using HTApp.Infrastructure.EntityModels;
using HTApp.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace HTApp.Infrastructure.Extensions;

public static class HTAppDataExtensions
{
    public static IServiceCollection AddHTAppContext(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(connectionString));

        return services;
    }

    public static IServiceCollection AddHTAppRepositories(this IServiceCollection services)
    {
        services.AddScoped<IGoodHabitRepository, GoodHabitRepository>();
        services.AddScoped<IBadHabitRepository, BadHabitRepository>();
        services.AddScoped<ITreatRepository, TreatRepository>();
        services.AddScoped<ITransactionRepository, TransactionRepository>();
        services.AddScoped<ISessionRepository, SessionRepository>();
        services.AddScoped<IUserDataRepository, UserDataRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}
