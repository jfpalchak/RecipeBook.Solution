
namespace RecipeBook.Models;

public class RecipeTag
{
  public int RecipeTagId { get; set; }
  public int RecipeId { get; set; }
  public int TagId { get; set; }

  //Reference Navigation Properties
  public Recipe Recipe { get; set; }
  public Tag Tag { get; set; }
}