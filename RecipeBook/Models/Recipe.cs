using System.Collections.Generic;

namespace RecipeBook.Models;

public class Recipe
{
  public int RecipeId { get; set; }

  // Collection Navigation Property
  public List<RecipeTag> JoinEntities { get; set; }
}