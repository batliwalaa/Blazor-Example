namespace BlazorExample.Client.store.cart;

public class CartUpdateItemQuantityAction
{
  public CartItem Item { get; private set; }

  public CartUpdateItemQuantityAction(CartItem item)
  {
    this.Item = item;
  }
}
