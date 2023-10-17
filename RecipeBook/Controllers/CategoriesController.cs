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

public class CategoriesController : Controller
{
  private readonly RecipeBookContext _db;
  private readonly UserManager<ApplicationUser> _userManager;

  public CategoriesController(UserManager<ApplicationUser> userManager, RecipeBookContext database)
  {
    _userManager = userManager;
    _db = database;
  }

  public ActionResult Index()
  {
    return View(_db.Categories.ToList());
  }

  public ActionResult Create()
  {
    return View();
  }

  [HttpPost]
  public ActionResult Create(Category category)
  {
    if(!ModelState.IsValid)
    {
      return View(category);
    }
    else
    {
      _db.Categories.Add(category);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }
  }

  public ActionResult Details(int id)
  {
    Category foundCategory = _db.Categories
                                .Include(cat => cat.Recipes)
                                .ThenInclude(recipe => recipe.JoinEntities)
                                .ThenInclude(join => join.Tag)
                                .FirstOrDefault(cat => cat.CategoryId == id);
    return View(foundCategory);
  }

  public ActionResult Edit(int id)
  {
    Category foundCategory = _db.Categories.FirstOrDefault(cat => cat.CategoryId == id);
    return View(foundCategory);
  }

  [HttpPost]
  public ActionResult Edit(Category category)
  {
    _db.Categories.Update(category);
    _db.SaveChanges();
    return View("Details", new { id = category.CategoryId });
  }

  public ActionResult Delete(int id)
  {
    Category foundCategory = _db.Categories.FirstOrDefault(cat => cat.CategoryId == id);
    return View(foundCategory);
  }

  [HttpPost, ActionName("Delete")]
  public ActionResult DeleteConfirmed(int id)
  {
    Category foundCategory = _db.Categories.FirstOrDefault(cat => cat.CategoryId == id);
    _db.Categories.Remove(foundCategory);
    _db.SaveChanges();
    return RedirectToAction("Index");
  }

}