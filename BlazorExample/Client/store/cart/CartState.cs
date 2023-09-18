namespace BlazorExample.Client.store.cart;

public record CartState
{
  public int CurrentCartItemsCount { get; init; }
  public IEnumerable<CartItem>? CartItems { get; init; }
}
