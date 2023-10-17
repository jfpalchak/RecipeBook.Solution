using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace RecipeBook.Models
{
  public class RecipeBookContext : IdentityDbContext<ApplicationUser>
  {

    public RecipeBookContext(DbContextOptions options) : base(options) { }
  }
}