using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RecipeBook.Models;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace RecipeBook
{
  class Program
  {
    static void Main(string[] args)
    {
      WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

      builder.Services.AddControllersWithViews();

      builder.Services.AddDbContext<RecipeBookContext>(
                        dbContextOptions => dbContextOptions
                          .UseMySql(
                            builder.Configuration["ConnectionStrings:DefaultConnection"], ServerVersion.AutoDetect(builder.Configuration["ConnectionStrings:DefaultConnection"]
                            )
                          )
                        );

      builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                  .AddRoles<IdentityRole>()
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

      // SEED ROLES & ADMIN USER
      // using (var scope = app.Services.CreateScope())
      // {
      //   RoleManager<IdentityRole> roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
      //   UserManager<ApplicationUser> userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

      //   string[] roles = new [] { "Admin", "Manger", "Member" };

      //   string email = "admin@test.com";
      //   string password = "password";

      //   // SEED ROLES
      //   foreach (string role in roles)
      //   {
      //     if (!await roleManager.RoleExistsAsync(role))
      //     {
      //       await roleManager.CreateAsync(new IdentityRole(role));
      //     }
      //   }

      //   // SEED ADMIN
      //   // did we already seed it?
      //   if(await userManager.FindByEmailAsync(email) == null)
      //   {
      //     ApplicationUser user = new ApplicationUser();
      //     user.UserName = email;
      //     user.Email = email;

      //     // Create the account
      //     await userManager.CreateAsync(user, password);
      //     // Add the account to the Admin role
      //     await userManager.AddToRoleAsync(user, "Admin");
      //   }
      // }

      app.Run();
    }
  }
}