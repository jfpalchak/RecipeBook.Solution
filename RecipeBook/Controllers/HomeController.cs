using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using RecipeBook.Models;

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
    return View();
  }
}
