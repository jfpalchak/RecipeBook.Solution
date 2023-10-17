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
[Authorize]
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
    return View(_db.Tags.ToList());
  }

  public ActionResult Details(int id)
  {
    Tag thisTag = _db.Tags
                      .Include(tag => tag.JoinEntities) // load each join entity, ie, list of ItemTags (references to a relationship)
                      .ThenInclude(join => join.Recipe) // then, load the Item object associated with each ItemTag join entity
                      .FirstOrDefault(tag => tag.TagId == id);
    return View(thisTag);
  }

  public ActionResult Create()
  {
    return View();
  }

  [HttpPost]
  public ActionResult Create(Tag tag)
  {
    _db.Tags.Add(tag);
    _db.SaveChanges();
    return RedirectToAction("Index");
  }

  public ActionResult AddRecipe(int id)
  {
    Tag thisTag = _db.Tags.FirstOrDefault(tags => tags.TagId == id);
    ViewBag.RecipeId = new SelectList(_db.Recipes, "RecipeId", "Name");
    return View(thisTag);
  }

  [HttpPost]
  public ActionResult AddRecipe(Tag tag, int recipeId)
  {
    #nullable enable
    RecipeTag? joinEntity = _db.RecipeTags.FirstOrDefault(join => (join.RecipeId == recipeId && join.TagId == tag.TagId));
    #nullable disable
    if (joinEntity == null && recipeId != 0)
    {
      _db.RecipeTags.Add(new RecipeTag() { RecipeId = recipeId, TagId = tag.TagId });
      _db.SaveChanges();
    }
    return RedirectToAction("Details", new { id = tag.TagId });
  }

  public ActionResult Edit(int id)
  {
    Tag thisTag = _db.Tags.FirstOrDefault(tag => tag.TagId == id);
    return View(thisTag);
  }

  [HttpPost]
  public ActionResult Edit(Tag tag)
  {
    _db.Tags.Update(tag);
    _db.SaveChanges();
    return RedirectToAction("Index");
  }

  public ActionResult Delete(int id)
  {
    Tag thisTag = _db.Tags.FirstOrDefault(tags => tags.TagId == id);
    return View(thisTag);
  }

  [HttpPost, ActionName("Delete")]
  public ActionResult DeleteConfirmed(int id)
  {
    Tag thisTag = _db.Tags.FirstOrDefault(tags => tags.TagId == id);
    _db.Tags.Remove(thisTag);
    _db.SaveChanges();
    return RedirectToAction("Index");
  }
}