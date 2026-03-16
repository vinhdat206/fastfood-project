namespace FastFood.Models;

public class Category
{
    public int CategoryId { get; set; }

    public string CategoryName { get; set; }

    public string Description { get; set; }

    public ICollection<Food>? Foods { get; set; }
}