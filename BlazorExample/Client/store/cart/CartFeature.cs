using Fluxor;

namespace BlazorExample.Client.store.cart;

public class CartFeature : Feature<CartState>
{
  public override string GetName() => "Cart";

  protected override CartState GetInitialState()
  {
    return new CartState
    {
      CurrentCartItemsCount = 0,
      CartItems = Enumerable.Empty<CartItem>()
    };
  }
}
