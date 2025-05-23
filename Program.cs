using GR1.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer("Data Source=localhost; User ID=SA; Password=123456;Encrypt=False;"));

// Configure authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Auth/Login";
        options.LogoutPath = "/Auth/Logout";
    });

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
// Trong Startup.cs hoặc Program.cs
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    
    // Bỏ qua ràng buộc dữ liệu tạm thời
    context.Database.ExecuteSqlRaw("ALTER TABLE Logs NOCHECK CONSTRAINT ALL");

    // Áp dụng các migration
    context.Database.Migrate();

    // Kích hoạt lại các ràng buộc dữ liệu
    context.Database.ExecuteSqlRaw("ALTER TABLE Logs WITH CHECK CHECK CONSTRAINT ALL");
}
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Student}/{action=Index}/{id?}")
    .WithStaticAssets();
app.MapControllerRoute(
    name: "class",
    pattern: "{controller=Class}/{action=Index}/{id?}");
app.Run();
