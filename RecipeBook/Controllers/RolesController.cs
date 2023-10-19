using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System.Collections.Generic;
using RecipeBook.Models;
using RecipeBook.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;

namespace RecipeBook.Controllers;

[Authorize(Roles = "Admin")]
public class RolesController : Controller
{
  private readonly RecipeBookContext _db;
  private readonly RoleManager<IdentityRole> _roleManager;
  private readonly UserManager<ApplicationUser> _userManager;
  private readonly SignInManager<ApplicationUser> _signInManager;

  public RolesController (RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RecipeBookContext db)
  {
    _userManager = userManager;
    _signInManager = signInManager;
    _roleManager = roleManager;
    _db = db;
  }

  public async Task<ActionResult> Index()
  {
    List<IdentityRole> roles = await _roleManager.Roles.ToListAsync();
    return View(roles);
  }

  public ActionResult Create()
  {
    return View();
  }

  [HttpPost]
  public async Task<ActionResult> Create(string name)
  {
    try
    { // TODO : validation
      if (!string.IsNullOrEmpty(name) && !(await _roleManager.RoleExistsAsync(name)))
      {
        await _roleManager.CreateAsync(new IdentityRole(name));
        return RedirectToAction("Index");
      }
      return View();
    }
    catch
    {
      return View();
    }
  }

  [HttpPost]
  public async Task<ActionResult> Delete(string id)
  {
    // Find the specific role.
    IdentityRole role = await _roleManager.FindByIdAsync(id);
    if (role != null)
    {
      // If it exists, delete it.
      IdentityResult result = await _roleManager.DeleteAsync(role);
      if (result.Succeeded)
      {
        return RedirectToAction("Index");
      }
      else
      {
        foreach (IdentityError error in result.Errors)
        {
          ModelState.AddModelError("", error.Description);
        }
      }
    }
    else
    {
      ModelState.AddModelError("", "No role found.");
    }

    return View("Index", _roleManager.Roles);
  }

  public async Task<ActionResult> Update(string id)
  {
    IdentityRole role = await _roleManager.FindByIdAsync(id);
    List<ApplicationUser> members = new List<ApplicationUser>();
    List<ApplicationUser> nonMembers = new List<ApplicationUser>();

    List<ApplicationUser> users = await _userManager.Users.ToListAsync();
    foreach(ApplicationUser user in users)
    {
      // IF the user is in this role, add to the members list. 
      // Otherwise add to the Non Members list.
      if(await _userManager.IsInRoleAsync(user, role.Name))
        members.Add(user);
      else
        nonMembers.Add(user);
    }
    // Depending on the chosen Role, send an instance of the RoleEdit Model to list 
    // the particular Role's users that are both assigned and not assigned this Role.
    return View(new RoleEdit
    {
      Role = role,
      Members = members,
      NonMembers = nonMembers
    });
  }

  [HttpPost]
  public async Task<ActionResult> Update(RoleModification model)
  {
    IdentityResult result;
    if (!ModelState.IsValid)
    {
      return RedirectToAction("Update", new { id = model.RoleId });
    }
    else
    {
      foreach (string userId in model.AddIds ?? new string[] {})
      {
        ApplicationUser user = await _userManager.FindByIdAsync(userId);
        if (user != null)
        {
          result = await _userManager.AddToRoleAsync(user, model.RoleName);
          if (!result.Succeeded)
          {
            foreach (IdentityError error in result.Errors)
            {
              ModelState.AddModelError("", error.Description);
            }
          }
        }
      }
      foreach (string userId in model.DeleteIds ?? new string[] {})
      {
        ApplicationUser user = await _userManager.FindByIdAsync(userId);
        if (user != null)
        {
          result = await _userManager.RemoveFromRoleAsync(user, model.RoleName);
          if (!result.Succeeded)
          {
            foreach (IdentityError error in result.Errors)
            {
              ModelState.AddModelError("", error.Description);
            }
          }
        }
      }
    }

    return RedirectToAction("Index");
  }
}