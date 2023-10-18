using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace RecipeBook.Models;

public class RoleEdit
{
  public IdentityRole Role { get; set; }
  public List<ApplicationUser> Members { get; set; }
  public List<ApplicationUser> NonMembers { get; set; }
}