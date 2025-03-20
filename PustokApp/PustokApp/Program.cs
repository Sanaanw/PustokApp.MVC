using Microsoft.EntityFrameworkCore;
using PustokApp.Data;
using PustokApp.Services;

namespace PustokApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            var config = builder.Configuration;
            builder.Services.AddDbContext<PustokAppContext>(options =>
            {
                options.UseSqlServer(config.GetConnectionString("DefaultConnection"));
            });
            builder.Services.AddScoped<LayoutService>();
            //IOptionPatternPart
            builder.Services.Configure<JwtServiceOption>(config.GetSection("Jwt"));

            
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

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
             name: "areas",
             pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}"
           );

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
