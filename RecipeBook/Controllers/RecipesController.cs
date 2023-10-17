using Microsoft.AspNetCore.Authorization; // lets us use the [Authorize] attribute
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using RecipeBook.Models;

namespace RecipeBook.Controllers;

public class RecipesController : Controller
{
  private readonly RecipeBookContext _db;
  private readonly UserManager<ApplicationUser> _userManager;

  public RecipesController(UserManager<ApplicationUser> userManager, RecipeBookContext database)
  {
    _userManager = userManager;
    _db = database;
  }

  public ActionResult Index()
  {
    return View();
  }

}