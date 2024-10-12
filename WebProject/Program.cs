using WebProject.Extentions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebProject.DbContextLayer;
using WebProject.Entites;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.

builder.Services.ConfigureServiceManager();

builder.Services.ConfigureMySqlContext(builder.Configuration);

builder.Services.ConfigureAuthorizationHandlerService();

builder.Services.AddControllers();
builder.Services.AddControllersWithViews();

builder.Services.AddRazorPages().

AddRazorPagesOptions(options => {

    // Thêm Page Route cho trang trong Areas
    // Truy cập /sanpham/ten-san-pham = /Product/Detail/ten-san-pưham
    options.Conventions.AddAreaPageRoute(
        areaName: "Identity",
        pageName: "/Account/Login",
        route: "dang-nhap"
        );
});



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.MapAreaControllerRoute(
            name: "Manager",
            pattern: "Manager/{controller}/{action}/{id?}",
            areaName: "Manager",
            defaults: new
            {
                controller = "Product",
                action = "index"
            }
        );

app.MapRazorPages();

app.UseRouting();

app.UseAuthentication();   // Phục hồi thông tin đăng nhập (xác thực)
app.UseAuthorization();   // Phục hồi thông tinn về quyền của User

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
