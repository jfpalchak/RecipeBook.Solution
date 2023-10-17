using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using RecipeBook.Models;
using RecipeBook.ViewModels;

namespace RecipeBook.Controllers;

public class AccountController : Controller
{
  private readonly RecipeBookContext _db;
  private readonly UserManager<ApplicationUser> _userManager;
  private readonly SignInManager<ApplicationUser> _signInManager;
  private readonly RoleManager<IdentityRole> _roleManager;

  public AccountController (RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RecipeBookContext db)
  {
    _userManager = userManager;
    _signInManager = signInManager;
    _roleManager = roleManager;
    _db = db;
  }

  public ActionResult Index()
  {
    return View();
  }

  public IActionResult Register()
  {
    return View();
  }

  [HttpPost]
  public async Task<ActionResult> Register(RegisterViewModel model)
  {
    if(!ModelState.IsValid)
    {
      return View(model);
    }
    else
    {
      ApplicationUser user = new ApplicationUser { UserName = model.Email };
      IdentityResult result = await _userManager.CreateAsync(user, model.Password);
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
        return View(model);
      }
    }
  }

  public ActionResult Login()
  {
    return View();
  }

  [HttpPost]
  public async Task<ActionResult> Login(LoginViewModel model)
  {
    if (!ModelState.IsValid)
    {
      return View(model);
    }
    else
    {
      Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, isPersistent: true, lockoutOnFailure: false);
      if (result.Succeeded)
      {
        return RedirectToAction("Index");
      } 
      else
      {
        ModelState.AddModelError("", "There is something wrong with your email or username. Please try again");
        return View(model);
      }
    }
  }

  [HttpPost]
  public async Task<ActionResult> LogOff()
  {
    await _signInManager.SignOutAsync();
    return RedirectToAction("Index");
  }
}