namespace BlazorExample.Client.store.cart;

public class CartRemoveItemAction
{
  public CartItem Item { get; private set; }

  public CartRemoveItemAction(CartItem item)
  {
    Item = item;
  }
}
