using HomeServices.Data;
using HomeServices.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 1. Database Connection Configuration
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// 2. Identity Services Configuration
// ????? ???? ??????? ApplicationUser ??? ???? ??????? ?????
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options => {
    options.Password.RequiredLength = 4;
    options.Password.RequireDigit = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

// 3. MVC & Razor Pages Services
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

// 4. Middleware Pipeline Order
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

// 5. Automatic Database Creation & Seed Data
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<ApplicationDbContext>();

        // ?????? Migrate ??? EnsureCreated ???? ???? ??? Migrations ???? ??????? ???????
        context.Database.Migrate();

        // Seeding initial categories if the table is empty
        if (!context.Categories.Any())
        {
            context.Categories.AddRange(
                new Category { Name = "Electricity", Description = "Electrical maintenance, wiring, and repair services" },
                new Category { Name = "Plumbing", Description = "Water leaks repair, pipe installation, and sanitary works" },
                new Category { Name = "Cleaning", Description = "Full home cleaning, carpet washing, and sanitation" },
                new Category { Name = "Painting", Description = "Professional interior/exterior painting and wall decoration" }
            );
            context.SaveChanges();
        }
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while seeding the database.");
    }
}

app.Run();