using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RecipeBook.Models;

public class Tag
{
  public int TagId { get; set; }
  [Required(ErrorMessage = "The tag's title can't be empty!")]
  public string Title { get; set; }

  // Collection Navigation Property
  public List<RecipeTag> JoinEntities { get; set; }
}