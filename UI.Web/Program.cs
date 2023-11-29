using ApplicationCore.Interfaces;
using ApplicationCore.Services;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using ApplicationCore.Domain;
using UI.Web.Areas.Identity.Pages.Account;
using PdfSharp.Charting;
using System.Security.Claims;
using UI.Web.Controllers;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("ExamenContextConnection") ?? throw new InvalidOperationException("Connection string 'ExamenContextConnection' not found.");

// Add services to the container.

builder.Services.AddControllersWithViews();


builder.Services.AddDbContext<ExamenContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = false).AddDefaultTokenProviders()
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ExamenContext>();
//credit conges increment
builder.Services.AddHostedService<CreditCongesUpdateService>();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("EmployeeCreate", policy => policy.RequireClaim("Permission", "Create"));
    options.AddPolicy("EmployeeRead", policy => policy.RequireClaim("Permission", "Read"));
    options.AddPolicy("EmployeeUpdate", policy => policy.RequireClaim("Permission", "Update"));
    options.AddPolicy("EmployeeDelete", policy => policy.RequireClaim("Permission", "Delete"));
    // Add other policies for CRUD operations as needed
});




/*
builder.Services.AddIdentity<LoginModel, IdentityRole>(options =>
{
    options.User.RequireUniqueEmail = false;
});

*/
builder.Services.AddScoped<DbContext, ExamenContext>();
builder.Services.AddScoped<IUnitOfWork,UnitOfWork>();
builder.Services.AddScoped<IServiceEmployees, ServiceEmployees>();
builder.Services.AddScoped<IServiceSectioncs, ServiceSection>();
builder.Services.AddScoped<IServiceConges, ServiceConges>();
builder.Services.AddScoped<IServiceSituation, ServiceSituation>();
builder.Services.AddScoped<IServiceContreVisite, ServiceContreVisite>();
builder.Services.AddScoped<IServiceTypesConges, ServiceTypeConges>();
builder.Services.AddScoped<IServiceAttestation, ServiceAttestation>();
builder.Services.AddScoped<IServiceMission, ServiceMission>();
builder.Services.AddScoped<IServiceTypeConfirmation, ServiceTypeConfirmation>();
builder.Services.AddScoped<IServiceTypeJustificatif, ServiceTypeJustificatif>();
builder.Services.AddScoped<IServiecAbsence, ServiceAbsence>();
builder.Services.AddScoped<IServiceGenres, ServiceGenre>();
builder.Services.AddScoped<IServiceFonctionEmployee, ServiceFonctionEmployee>();
builder.Services.AddScoped<IServiceGestionDocument, ServiceGestionDocument>();
builder.Services.AddScoped<IServiceSoldeConges, ServiceSoldeConges>();
builder.Services.AddScoped<IServiceTypeAttestation, ServiceTypeAttestation>();

builder.Services.AddSingleton<Type>(t => typeof(GenericRepository<>));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();;




app.UseAuthorization();
app.MapRazorPages();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
/*
using (var scope = app.Services.CreateScope())
{
    var roleManager =
        scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var roles = new[] { "Administrateur", "Utilisateur" , "DeleteOnlyRole" };
    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
            await roleManager.CreateAsync(new IdentityRole(role));

    } 
}
using (var scope = app.Services.CreateScope())
{
    var userManager =
        scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
    string email = "admin@admin.com";
    string password = "Admin@2024";
   if(await userManager.FindByEmailAsync(email) == null)
    {
        var user = new ApplicationUser();
        user.UserName = email;
        user.Email = email;

      await  userManager.CreateAsync(user, password);
       await userManager.AddToRoleAsync(user, "Administrateur");
    }
  
}
using (var scope = app.Services.CreateScope())
{
    var userManager =
        scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
    string email = "user@user.com";
    string password = "User@2024";
    if (await userManager.FindByEmailAsync(email) == null)
    {
        var user = new ApplicationUser();
        user.UserName = email;
        user.Email = email;

        await userManager.CreateAsync(user, password);
        await userManager.AddToRoleAsync(user, "Utilisateur");
    }

}
using (var scope = app.Services.CreateScope())
{
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    string email = "deleteuser@user.com";
    string password = "Delete@2024";

    if (await userManager.FindByEmailAsync(email) == null)
    {
        var user = new ApplicationUser();
        user.UserName = email;
        user.Email = email;

        await userManager.CreateAsync(user, password);

        await userManager.AddToRolesAsync(user, new[] { "DeleteOnlyRole", "Role2", "Role3" });
    }
}

*/

app.Run();
