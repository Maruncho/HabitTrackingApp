using HTApp.Infrastructure.EntityModels;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
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
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>();

        return services;
    }
}
public static class EnsureRolesCreatedExtension
{
    public static async Task EnsureRolesCreated(IServiceProvider serviceProvider, IConfiguration configuration)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();

        string[] roleNames = { "Admin", "User" };

        List<IdentityError> errors = new();

        foreach (var roleName in roleNames)
        {
            var roleExist = await roleManager.RoleExistsAsync(roleName);
            if (!roleExist)
            {
                var result = await roleManager.CreateAsync(new IdentityRole(roleName));

                errors.AddRange(result.Errors);
            }
        }

        if(errors.Count > 0)
        {
            throw new Exception("Couldn't add roles. Errors: " + string.Join(Environment.NewLine, errors));
        }

        string? userAdminEmail = configuration["Admin:EmailAddress"];
        if(userAdminEmail is null)
        {
            throw new Exception("Admin:EmailAddress Env Var need cannot be found");
        }

        var adminUser = await userManager.FindByEmailAsync(userAdminEmail);

        if(adminUser is not null)
        {
            bool already = await userManager.IsInRoleAsync(adminUser, "Admin");
            if(!already)
            {
                IdentityResult result = await userManager.AddToRoleAsync(adminUser, "Admin");
                if (!result.Succeeded)
                {
                    throw new Exception("The Admin:EmailAddress user failed to be assigned the 'Admin' role");
                }
            }
        }
        else
        {
            //I'm lazy to inject the logger.
            Console.WriteLine("Admin Role was not assigned. Create an account with the admin email and restart the app.");
        }
    }
}
