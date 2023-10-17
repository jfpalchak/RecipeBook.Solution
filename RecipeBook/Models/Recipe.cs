using System.Collections.Generic;

namespace RecipeBook.Models;

public class Recipe
{
  public int RecipeId { get; set; }

  public string Name { get; set; }
  public string Instructions { get; set; }
  public string Ingredients { get; set; }

  public int CategoryId { get; set; }
  // Reference Navigation Property
  public Category Category { get; set; }

  // Collection Navigation Property
  public List<RecipeTag> JoinEntities { get; set; }

  public ApplicationUser User { get; set; }
}