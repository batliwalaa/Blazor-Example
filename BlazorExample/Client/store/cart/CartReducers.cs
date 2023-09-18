using Fluxor;

namespace BlazorExample.Client.store.cart;

public static class CartReducers
{
  [ReducerMethod]
  public static CartState OnAddCartItem(CartState state, CartAddItemAction action)
  {
    var items = new List<CartItem>(state.CartItems ?? new List<CartItem>())
    {
      action.Item
    };

    return state with { CartItems = items, CurrentCartItemsCount = items.Count };
  }

  [ReducerMethod]
  public static CartState OnARemoveCartItem(CartState state, CartRemoveItemAction action)
  {
    if (state.CartItems != null)
    {
      var items = state.CartItems.ToList();
      var itemToRemove = items.Find(i => 
        i.ProductId == action.Item.ProductId && i.ProductTypeId == action.Item.ProductTypeId);

      if (itemToRemove != null)
      {
        items.Remove(itemToRemove);
      }

      return state with { CartItems = items, CurrentCartItemsCount = items.Count() };
    }

    return state;    
  }
}
