using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using DehaAccountingMvc.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = true;
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequiredLength = 8;
})
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

// Cấu hình Cookies
builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
    options.LoginPath = "/Identity/Account/Login";
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
    options.SlidingExpiration = true;
});

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
    app.UseDeveloperExceptionPage();

    // Seed database khi ở môi trường phát triển
    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        try
        {
            var context = services.GetRequiredService<ApplicationDbContext>();
            // Đảm bảo database được tạo
            context.Database.EnsureCreated();
            // Thêm seed data
            context.Seed();

            // Seed default user và roles
            var userManager = services.GetRequiredService<UserManager<IdentityUser>>();
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

            // Tạo các roles
            string[] roleNames = { "Admin", "Accountant", "Sales", "Manager" };
            foreach (var roleName in roleNames)
            {
                // Kiểm tra xem role đã tồn tại chưa
                if (!roleManager.RoleExistsAsync(roleName).Result)
                {
                    // Tạo role mới
                    var role = new IdentityRole(roleName);
                    roleManager.CreateAsync(role).Wait();
                }
            }

            // Tạo admin user
            var adminUser = new IdentityUser
            {
                UserName = "admin@deha.com.vn",
                Email = "admin@deha.com.vn",
                EmailConfirmed = true
            };

            // Kiểm tra xem admin user đã tồn tại chưa
            if (userManager.FindByEmailAsync(adminUser.Email).Result == null)
            {
                // Tạo user mới
                var result = userManager.CreateAsync(adminUser, "Admin@123456").Result;
                if (result.Succeeded)
                {
                    // Gán role admin cho user
                    userManager.AddToRoleAsync(adminUser, "Admin").Wait();
                }
            }
        }
        catch (Exception ex)
        {
            var logger = services.GetRequiredService<ILogger<Program>>();
            logger.LogError(ex, "Đã xảy ra lỗi khi seed database.");
        }
    }
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

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();
