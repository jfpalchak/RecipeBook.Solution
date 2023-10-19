using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using RecipeBook.Models;
using Microsoft.EntityFrameworkCore;

namespace RecipeBook.Controllers;

public class HomeController : Controller
{
	private readonly RecipeBookContext _db;
	private UserManager<ApplicationUser> _userManager;
	
	public HomeController(UserManager<ApplicationUser> userManager, RecipeBookContext db)
	{
		_db = db;
		_userManager = userManager;
	}
	
  public ActionResult Index()
  {
		Category[] cats = _db.Categories.ToArray();
		Recipe[] recs = _db.Recipes
															.Include(recipe => recipe.User)
															.ToArray();

		Dictionary<string, object[]> model = new Dictionary<string, object[]> ();

		model.Add("categories", cats);
		model.Add("recipes", recs);

    return View(model);
  }
}
