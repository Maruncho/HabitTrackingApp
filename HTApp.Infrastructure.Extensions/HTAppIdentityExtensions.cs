using HTApp.Infrastructure.EntityModels;
using Microsoft.Extensions.DependencyInjection;

namespace HTApp.Infrastructure.Extensions;

public static class HTAppIdentityExtensions
{
    public static IServiceCollection AddHTAppIdentity(this IServiceCollection services)
    {
        services.AddDefaultIdentity<AppUser>(options => options.SignIn.RequireConfirmedAccount = true)
            .AddEntityFrameworkStores<ApplicationDbContext>();

        return services;
    }
}
