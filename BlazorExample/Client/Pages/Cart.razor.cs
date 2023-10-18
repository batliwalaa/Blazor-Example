using BlazorExample.Client.store.cart;
using Fluxor;
using Microsoft.AspNetCore.Components;

namespace BlazorExample.Client.Pages;

public partial class Cart
{
  [Inject]
  private IState<CartState>? CartState { get; init; }

  [Inject]
  private IDispatcher? Dispatcher { get; init; }

  private IEnumerable<CartItem> CartItems
  {
    get => CartState?.Value.CartItems ?? Enumerable.Empty<CartItem>();
  }

  private void RemoveCartItem(CartItem cartItem)
  {
    if (Dispatcher != null && cartItem != null)
    {
      Dispatcher.Dispatch(new CartRemoveItemAction(cartItem));
    }
  }

  private void UpdateCartItemQuantity(ChangeEventArgs changeEvent, CartItem cartItem)
  {
    cartItem.Qantity = int.Parse(changeEvent.Value?.ToString() ?? "0");

    if (cartItem.Qantity < 1)
    {
      cartItem.Qantity = 1;
    }

    Dispatcher?.Dispatch(new CartUpdateItemQuantityAction(cartItem));
  }

  protected override void OnInitialized()
  {
    base.OnInitialized();
  }
}
