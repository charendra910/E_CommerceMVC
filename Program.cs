using E_CommerceMVC.Configuration;
using E_CommerceMVC.Data;
using E_CommerceMVC.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// Add Razor Pages services (required for Identity UI)
builder.Services.AddRazorPages();

// Configure Identity with custom ApplicationUser and roles
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = true;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 8;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultUI() // Add this line to include the default Identity UI
.AddDefaultTokenProviders();

// Add authorization policy
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy =>
        policy.RequireRole("Admin")
              .RequireClaim("IsAdmin", "true"));
});

builder.Services.AddControllersWithViews()
    .AddRazorRuntimeCompilation();

builder.Services.Configure<RazorPayConfiguration>(
    builder.Configuration.GetSection("RazorPayConfiguration")
);

builder.Services.AddSingleton<IRazorPayConfiguration>(sp =>
    sp.GetRequiredService<IOptions<RazorPayConfiguration>>().Value
);

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
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// Seed admin user and role
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

        // Ensure Admin role exists
        if (!await roleManager.RoleExistsAsync("Admin"))
        {
            await roleManager.CreateAsync(new IdentityRole("Admin"));
        }

        // Create initial admin user
        var adminEmail = "admin1234@gmail.com";
        var adminUser = await userManager.FindByEmailAsync(adminEmail);
        if (adminUser == null)
        {
            adminUser = new ApplicationUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                EmailConfirmed = true, // Skip email confirmation for initial admin
                IsAdmin = true
            };
            var result = await userManager.CreateAsync(adminUser, "Admin1234@");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, "Admin");
                await userManager.AddClaimAsync(adminUser, new System.Security.Claims.Claim("IsAdmin", "true"));
            }
        }
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while seeding admin user.");
    }
}

app.MapControllerRoute(
    name: "Dashboard",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
);

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Map Razor Pages (for Identity UI)
app.MapRazorPages();

app.Run();