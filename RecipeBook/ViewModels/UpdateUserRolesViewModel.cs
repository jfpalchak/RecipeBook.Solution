using System.Collections.Generic;

namespace RecipeBook.ViewModels;

public class UpdateUserRolesViewModel
{
  public string RoleId { get; set; }
  public string RoleName { get; set; }

  public bool Selected { get; set; }
}