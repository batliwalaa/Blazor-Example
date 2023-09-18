namespace BlazorExample.Client.store.cart;

public class CartAddItemAction
{
  public CartItem Item { get; private set; }

  public CartAddItemAction(CartItem item)
  {
    Item = item;
  }
}
