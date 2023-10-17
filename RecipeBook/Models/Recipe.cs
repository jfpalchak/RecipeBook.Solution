using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RecipeBook.Models;

public class Recipe
{
  public int RecipeId { get; set; }
  [Required(ErrorMessage = "The recipe's name can't be empty!")]
  public string Name { get; set; }
  [Required(ErrorMessage = "The recipe's instructions can't be empty!")]
  public string Instructions { get; set; }
  [Required(ErrorMessage = "The recipe's ingredients can't be empty!")]
  public string Ingredients { get; set; }

  [Range(1, int.MaxValue, ErrorMessage = "You must add your recipe to a category. Have you created a category yet?")]
  public int CategoryId { get; set; }
  // Reference Navigation Property
  public Category Category { get; set; }

  // Collection Navigation Property
  public List<RecipeTag> JoinEntities { get; set; }

  public ApplicationUser User { get; set; }
}