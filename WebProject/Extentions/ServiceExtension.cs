using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using WebProject.DbContextLayer;
using WebProject.Entites;
using WebProject.FileManager;
using WebProject.Services.CategoryService;
using WebProject.Services.MailService;
using WebProject.Services.OrderService;
using WebProject.Services.ProductService;

namespace WebProject.Extentions
{
    public static class ServiceExtension
    {
        public static void ConfigureMySqlContext(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<AppDbContext>(
                    options => options.UseSqlServer(
                    config.GetConnectionString("LocalHost"),
                    providerOptions => providerOptions.EnableRetryOnFailure()));

            services.AddIdentity<AppUser, IdentityRole>()
                          .AddEntityFrameworkStores<AppDbContext>()
                          .AddDefaultTokenProviders();


        }

        public static void ConfigureServiceManager(this IServiceCollection services)
        {
            services.AddSingleton<IEmailSender, EmailSender>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddSingleton<IFileService, FileService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<ICategoryService, CategoryService>();

            services.Configure<IdentityOptions>(options =>
            {
                options.SignIn.RequireConfirmedEmail = false;
                options.SignIn.RequireConfirmedAccount = false; // Yêu cầu xác nhận tài khoản trước khi đăng nhập
                options.SignIn.RequireConfirmedPhoneNumber = false;

                options.Tokens.AuthenticatorTokenProvider = TokenOptions.DefaultAuthenticatorProvider;
                // Thiết lập về Password
                options.Password.RequireDigit = false; // Không bắt phải có số
                options.Password.RequireLowercase = false; // Không bắt phải có chữ thường
                options.Password.RequireNonAlphanumeric = false; // Không bắt ký tự đặc biệt
                options.Password.RequireUppercase = false; // Không bắt buộc chữ in
                options.Password.RequiredLength = 6; // Số ký tự tối thiểu của password
                options.Password.RequiredUniqueChars = 1; // Số ký tự riêng biệt
             
                // Cấu hình Lockout - khóa user
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5); // Khóa 5 phút
                options.Lockout.MaxFailedAccessAttempts = 5; // Thất bại 5 lầ thì khóa
                options.Lockout.AllowedForNewUsers = true;

                // Cấu hình về User.
                options.User.AllowedUserNameCharacters = // các ký tự đặt tên user
                    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
              

                // Cấu hình đăng nhập.

            });

        }


        public static void ConfigureAuthorizationHandlerService(this IServiceCollection services)
        {

            services.AddAuthorization(options =>
            {

                options.AddPolicy("Administrator", builder =>
                {
                    builder.RequireAuthenticatedUser();
                    builder.RequireRole("Administrator");

                });

                options.AddPolicy("Admin", builder =>
                {
                    builder.RequireAuthenticatedUser();
                    builder.RequireRole("Administrator", "Admin");

                });

                options.AddPolicy("Employee", builder =>
                {
                    builder.RequireAuthenticatedUser();
                    builder.RequireRole("Administrator", "Admin", "Employee");

                });

                options.AddPolicy("Customer", builder =>
                {
                    builder.RequireAuthenticatedUser();
                    builder.RequireRole("Administrator", "Admin", "Employee", "Customer");

                });

            });

        }

    }

}
