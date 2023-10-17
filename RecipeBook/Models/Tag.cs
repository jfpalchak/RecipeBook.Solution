using System.Collections.Generic;

namespace RecipeBook.Models;

public class Tag
{
  public int TagId { get; set; }
  public string Title { get; set; }

  // Collection Navigation Property
  public List<RecipeTag> JoinEntities { get; set; }
}