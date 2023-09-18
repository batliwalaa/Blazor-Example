using BlazorExample.Client.store.cart;
using Fluxor;
using Microsoft.AspNetCore.Components;

namespace BlazorExample.Client.Shared;

public partial class CartCounter
{
  [Inject]
  public IState<CartState>? CartState { get; init; }

  protected override void OnInitialized()
  {
    base.OnInitialized();
  }
}
