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
    return View(_db.Recipes.ToList());
  }

  public ActionResult Create()
  {
    ViewBag.CategoryId = new SelectList(_db.Categories, "CategoryId", "Name");
    return View();
  }

  [HttpPost]
  public async Task<ActionResult> Create(Recipe recipe)
  {
    if(!ModelState.IsValid)
    {
      ViewBag.CategoryId = new SelectList(_db.Categories, "CategoryId", "Name");
      return View(recipe);
    }
    else
    {
      string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      ApplicationUser currentUser = await _userManager.FindByIdAsync(userId);
      recipe.User = currentUser;
      _db.Recipes.Add(recipe);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }
  }

  public ActionResult Details(int id)
  {
    Recipe thisRecipe = _db.Recipes 
                              .Include(recipe => recipe.Category)
                              .Include(recipe => recipe.JoinEntities)
                              .ThenInclude(join => join.Tag)
                              .FirstOrDefault(recipe => recipe.RecipeId == id);
    return View(thisRecipe);
  }

  public ActionResult Edit(int id)
  {
    Recipe thisRecipe = _db.Recipes.FirstOrDefault(recipe => recipe.RecipeId == id);
    ViewBag.CategoryId = new SelectList(_db.Categories, "CategoryId", "Name");
    return View(thisRecipe);
  }

  [HttpPost]
  public ActionResult Edit(Recipe recipe)
  {
    _db.Recipes.Update(recipe);
    _db.SaveChanges();
    return RedirectToAction("Index");
  }

  public ActionResult Delete(int id)
  {
    Recipe thisRecipe = _db.Recipes.FirstOrDefault(r => r.RecipeId == id);
    return View(thisRecipe);
  }

  [HttpPost, ActionName("Delete")]
  public ActionResult DeleteConfirmed(int id)
  {
    Recipe thisRecipe = _db.Recipes.FirstOrDefault(r => r.RecipeId == id);
    _db.Recipes.Remove(thisRecipe);
    _db.SaveChanges();
    return RedirectToAction("Index");
  }

  public ActionResult AddTag(int id)
  {
    Recipe thisRecipe = _db.Recipes.FirstOrDefault(r => r.RecipeId == id);
    ViewBag.TagId = new SelectList(_db.Tags, "TagId", "Title");
    return View(thisRecipe);
  }

  [HttpPost]
  public ActionResult AddTag(Recipe recipe, int tagId)
  {
    #nullable enable
    RecipeTag? joinEntity = _db.RecipeTags.FirstOrDefault(join => (join.TagId == tagId && join.RecipeId == recipe.RecipeId));
    #nullable disable
    if (joinEntity == null && tagId != 0)
    {
      _db.RecipeTags.Add(new RecipeTag() { TagId = tagId, RecipeId = recipe.RecipeId});
      _db.SaveChanges();
    }
    return RedirectToAction("Details", new { id = recipe.RecipeId });
  }

  [HttpPost]
    public ActionResult DeleteJoin(int joinId)
    {
      RecipeTag joinEntry = _db.RecipeTags.FirstOrDefault(entry => entry.RecipeTagId == joinId);
      _db.RecipeTags.Remove(joinEntry);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

}