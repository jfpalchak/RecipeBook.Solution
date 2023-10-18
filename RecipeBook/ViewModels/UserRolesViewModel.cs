using System.Collections.Generic;

namespace RecipeBook.ViewModels;

// UserRolesViewModel will hold all necessary user details, along with
// a user's roles (as a string list).
// We'll populate this ViewModel in the Admin controller, in order to help
// display a list of our Users AND their assigned Role(s).
public class UserRolesViewModel
{
  public string UserId { get; set; }
  public string UserName { get; set; }
  public List<string> Roles {get; set; }
}