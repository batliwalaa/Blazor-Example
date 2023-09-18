using BlazorExample.Client.store.cart;
using Fluxor;
using Microsoft.AspNetCore.Components;

namespace BlazorExample.Client.Pages;

public partial class Cart
{
  [Inject]
  public IState<CartState>? CartState { get; init; }

  [Inject]
  public IDispatcher? dispatcher { get; init; }

  public IEnumerable<CartItem> CartItems
  {
    get => CartState?.Value.CartItems ?? Enumerable.Empty<CartItem>();
  }

  public void RemoveCartItem(CartItem cartItem)
  {
    if (dispatcher != null && cartItem != null)
    {
      dispatcher.Dispatch(new CartRemoveItemAction(cartItem));
    }
  }

  protected override void OnInitialized()
  {
    base.OnInitialized();
  }
}
