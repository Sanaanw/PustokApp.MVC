using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PustokApp.Data;
using PustokApp.Models;
using PustokApp.Services;

namespace PustokApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            var config = builder.Configuration;
            builder.Services.AddServices(config);
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseSession();
            //wwwroot oxumaq uchun
            app.UseStaticFiles();

            app.UseRouting();



            app.MapControllerRoute(
             name: "areas",
             pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}"
           );

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            //User sistemi uchun
            app.UseAuthentication();
            app.UseAuthorization();

            app.Run();
        }
    }
}
