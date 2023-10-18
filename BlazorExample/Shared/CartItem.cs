namespace BlazorExample.Shared;

public class CartItem
{
  public int ProductId { get; set; }
  public int ProductTypeId { get; set; }
  public string Title { get; set; } = string.Empty;
  public string ProductTypeName { get; set; } = string.Empty;
  public string ImageUrl { get; set; } = string.Empty;
  public decimal Price { get; set; }
  public int Qantity { get; set; } = 1;
}
