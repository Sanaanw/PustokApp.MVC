using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PustokApp.Data;
using PustokApp.Models;
using PustokApp.Services;
using PustokApp.Settings;

namespace PustokApp
{
    public static class ServiceRegistration
    {
        public static void AddServices(this IServiceCollection services,IConfiguration config)
        {
            services.AddControllersWithViews();

            services.AddDbContext<PustokAppContext>(options =>
            {
                options.UseSqlServer(config.GetConnectionString("DefaultConnection"));
            });
            services.AddScoped<LayoutService>();
            services.AddSession(opt =>
            {
                opt.IdleTimeout = TimeSpan.FromSeconds(20);
            });
            services.AddScoped<EmailService>();
            //IOptionPatternPart
            services.Configure<JwtServiceOption>(config.GetSection("Jwt"));
            services.Configure<EmailSetting>(config.GetSection("Email"));

            services.AddIdentity<AppUser, IdentityRole>(opt =>
            {
                opt.Password.RequireDigit = true;
                opt.Password.RequireLowercase = true;
                opt.Password.RequireUppercase = true;
                opt.Password.RequireNonAlphanumeric = true;
                opt.Password.RequiredLength = 8;
                opt.User.RequireUniqueEmail = true;

                opt.Lockout.MaxFailedAccessAttempts = 3;
                opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                opt.Lockout.AllowedForNewUsers = true;
                opt.SignIn.RequireConfirmedEmail = true;
            }).AddEntityFrameworkStores<PustokAppContext>().AddDefaultTokenProviders();

            services.AddHttpContextAccessor();
            //Json cycle ucun
            services.AddControllers()
    .AddJsonOptions(opt =>
    {
        opt.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
        opt.JsonSerializerOptions.WriteIndented = true;
    });

            services.ConfigureApplicationCookie(opt =>
            {
                opt.Events.OnRedirectToLogin = opt.Events.OnRedirectToAccessDenied = context =>
                {
                    var uri = new Uri(context.RedirectUri);
                    if (context.Request.Path.Value.ToLower().StartsWith("/manage"))
                        context.Response.Redirect("/manage/account/login" + uri.Query);
                    else
                        context.Response.Redirect("/account/login" + uri.Query);
                    return Task.CompletedTask;
                };
            });
        }
    }
}
