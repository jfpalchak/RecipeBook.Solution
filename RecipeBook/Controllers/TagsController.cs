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

public class TagsController : Controller
{
	private readonly RecipeBookContext _db;
	private UserManager<ApplicationUser> _userManager;
	
	public TagsController(UserManager<ApplicationUser> userManager, RecipeBookContext db)
	{
		_db = db;
		_userManager = userManager;
	}
	
  public ActionResult Index()
  {
    return View();
  }
}