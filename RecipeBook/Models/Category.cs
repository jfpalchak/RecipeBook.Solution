using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RecipeBook.Models;

public class Category
{
  public int CategoryId { get; set; }
  [Required(ErrorMessage = "The category name can't be empty!")]
  public string Name { get; set; }

  // Collection Navigation Property
  public List<Recipe> Recipes { get; set; }
}