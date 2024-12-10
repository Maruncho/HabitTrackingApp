using Microsoft.EntityFrameworkCore;

using HTApp.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddHTAppContext(connectionString);
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddHTAppIdentity();

//builder.Services.AddHTAppRepositories();
builder.Services.AddHTAppServices();

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

//It's necessary :)
app.EnableHTAppServicesObserverPattern();


app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.UseStatusCodePagesWithReExecute("/StatusCode", "?statusCode={0}");

app.MapRazorPages();

using (var scope = app.Services.CreateScope())
{
    await EnsureRolesCreatedExtension.EnsureRolesCreated(scope.ServiceProvider, builder.Configuration);
}

app.Run();
