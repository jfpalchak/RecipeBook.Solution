using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System.Collections.Generic;
using RecipeBook.Models;
using RecipeBook.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Linq;

namespace RecipeBook.Controllers;

[Authorize(Roles = "Admin")]
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

  // USING A DICTIONARY INSTEAD OF A VIEWMODEL
  // public async Task<ActionResult> Index()
  // {
  //   List<IdentityRole> roles = await _roleManager.Roles.ToListAsync();
  //   Dictionary<IdentityRole, List<ApplicationUser>> usersWithRole = new Dictionary<IdentityRole, List<ApplicationUser>>();
  //   foreach(IdentityRole role in roles)
  //   {
  //     List<ApplicationUser> usersInRole = _userManager.GetUsersInRoleAsync(role.Name).Result.ToList();
  //     usersWithRole.Add(role, usersInRole);

  //   }
  //   return View(usersWithRole);
  // }

  public async Task<ActionResult> Update(string userId)
  {
    List<UpdateUserRolesViewModel> model = new List<UpdateUserRolesViewModel>();

    ViewBag.UserId = userId;
    List<IdentityRole> roles = await _roleManager.Roles.ToListAsync();
    ApplicationUser user = await _userManager.FindByIdAsync(userId);
    if (user == null)
    {
      ViewBag.ErrorMessage = $"User with Id = {userId} cannot be found.";
      return View();
    }

    ViewBag.UserName = user.UserName;
    foreach (IdentityRole role in roles)
    {
      UpdateUserRolesViewModel userRolesToManage = new UpdateUserRolesViewModel 
      {
        RoleId = role.Id, 
        RoleName = role.Name 
      };
      
      if( await _userManager.IsInRoleAsync(user, role.Name))
      {
        userRolesToManage.Selected = true;
      }
      else
      {
        userRolesToManage.Selected = false;
      }
      model.Add(userRolesToManage);
    }
    return View(model);
  }

  [HttpPost]
  public async Task<ActionResult> Update(List<UpdateUserRolesViewModel> model, string userId)
  {
    ApplicationUser user = await _userManager.FindByIdAsync(userId);
    if (user == null)
    {
      return View(model);
    }
    
    // Remove User's Roles
    List<string> roles = new List<string>(await _userManager.GetRolesAsync(user));
    IdentityResult result = await _userManager.RemoveFromRolesAsync(user, roles);
    if (!result.Succeeded)
    {
      ModelState.AddModelError("", "Cannot remove user's existing roles.");
      return View(model);
    }

    // Add Roles to User
    result = await _userManager.AddToRolesAsync(user, model
                                                        .Where(role => role.Selected)
                                                        .Select(role => role.RoleName));
    if (!result.Succeeded)
    {
      ModelState.AddModelError("", "Cannot add selected roles to User.");
      return View(model);
    }

    return RedirectToAction("Index");
  }
}