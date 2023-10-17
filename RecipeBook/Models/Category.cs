using System.Collections.Generic;

namespace RecipeBook.Models;

public class Category
{
  public int CategoryId { get; set; }
  public string Name { get; set; }

  // Collection Navigation Property
  public List<Recipe> Recipes { get; set; }
}