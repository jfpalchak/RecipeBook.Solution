using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RecipeBook.Models;
using Microsoft.AspNetCore.Identity;

namespace RecipeBook
{
  class Program
  {
    static void Main(string[] args)
    {
      WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

      builder.Services.AddControllersWithViews();

      // Add EF Core as a service to our app.
      // We specify RecipeBookContext as the type of EF Core database context we want to set up.
      // (RecipeBookContext is a representation of our MySQL database.)
      builder.Services.AddDbContext<RecipeBookContext>(
                        dbContextOptions => dbContextOptions
                          .UseMySql(
                            builder.Configuration["ConnectionStrings:DefaultConnection"], ServerVersion.AutoDetect(builder.Configuration["ConnectionStrings:DefaultConnection"]
                            )
                          )
                        );

      builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                  .AddEntityFrameworkStores<RecipeBookContext>()
                  .AddDefaultTokenProviders();

      // OVERRIDE DEFAULT PASSWORD REQUIREMENTS FOR DEV PURPOSES 
      builder.Services.Configure<IdentityOptions>(options => 
      {
        options.Password.RequireDigit = false;
        options.Password.RequireLowercase = false;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireUppercase = false;
        options.Password.RequiredLength = 0;
        options.Password.RequiredUniqueChars = 0;
      });
  
      WebApplication app = builder.Build();

      app.UseHttpsRedirection();
      app.UseStaticFiles();

      app.UseRouting();

      app.UseAuthentication();
      app.UseAuthorization();

      app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}"
      );

      app.Run();
    }
  }
}