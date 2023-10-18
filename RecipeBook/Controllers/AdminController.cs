using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System.Collections.Generic;
using RecipeBook.Models;
using RecipeBook.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace RecipeBook.Controllers;

public class AdminController : Controller
{
  private readonly RecipeBookContext _db;
  private readonly RoleManager<IdentityRole> _roleManager;
  private readonly UserManager<ApplicationUser> _userManager;
  private readonly SignInManager<ApplicationUser> _signInManager;

  public AdminController (RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RecipeBookContext db)
  {
    _userManager = userManager;
    _signInManager = signInManager;
    _roleManager = roleManager;
    _db = db;
  }

  public async Task<ActionResult> Index()
  {
    // Collect all of our current users.
    List<ApplicationUser> users = await _userManager.Users.ToListAsync();

    // To help provide the appropriate data to the View, we make a ViewModel
    // that will hold the necessary User data AND their assigned Role(s)
    List<UserRolesViewModel> userRolesViewModel = new List<UserRolesViewModel>();
    foreach (ApplicationUser user in users)
    {
      UserRolesViewModel thisUserRole = new UserRolesViewModel();
      thisUserRole.UserId = user.Id;
      thisUserRole.UserName = user.UserName;
      // Collect a User's assigned Role(s)
      thisUserRole.Roles = new List<string>(await _userManager.GetRolesAsync(user));
      userRolesViewModel.Add(thisUserRole);
    }

    return View(userRolesViewModel);
  }
}