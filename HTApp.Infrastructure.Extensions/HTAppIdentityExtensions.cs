using HTApp.Infrastructure.EntityModels;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
