using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System.Collections.Generic;
using RecipeBook.Models;
using RecipeBook.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace RecipeBook.Controllers;

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

  // public async Task<ActionResult> Update(string id)
  // {
    
  // }
}