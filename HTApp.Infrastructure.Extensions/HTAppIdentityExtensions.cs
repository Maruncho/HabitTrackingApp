using HTApp.Infrastructure.EntityModels;
using Microsoft.Extensions.DependencyInjection;

namespace HTApp.Infrastructure.Extensions;

public static class HTAppIdentityExtensions
{
    public static IServiceCollection AddHTAppIdentity(this IServiceCollection services)
    {
        services.AddDefaultIdentity<AppUser>(options =>
        {
            options.SignIn.RequireConfirmedAccount = false; //Email provider is out of scope

            // No sensitive data, which google doesn't know about already.
            // So for now, security is a problem for another time.
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireDigit = false;
            options.Password.RequireUppercase = false;
            options.Password.RequireLowercase = false;
        })
            .AddEntityFrameworkStores<ApplicationDbContext>();

        return services;
    }
}
