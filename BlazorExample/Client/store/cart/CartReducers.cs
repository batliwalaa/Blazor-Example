using Fluxor;

namespace BlazorExample.Client.store.cart;

public static class CartReducers
{
  [ReducerMethod]
  public static CartState OnAddCartItem(CartState state, CartAddItemAction action)
  {
    var items = new List<CartItem>(state.CartItems ?? new List<CartItem>());

    CartItem? existingItem = items.Find(x => x.ProductId == action.Item.ProductId && x.ProductTypeId == action.Item.ProductTypeId);

    if (existingItem != null)
    {
      existingItem.Qantity += 1;
    }
    else 
    {
      items.Add(action.Item);
    }

    return state with { CartItems = items, CurrentCartItemsCount = items.Sum(x => x.Qantity) };
  }

  [ReducerMethod]
  public static CartState OnRemoveCartItem(CartState state, CartRemoveItemAction action)
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

      return state with { CartItems = items, CurrentCartItemsCount = items.Sum(x => x.Qantity) };
    }

    return state;
  }

  [ReducerMethod]
  public static CartState OnUpdateCartItemQuantity(CartState state, CartUpdateItemQuantityAction action)
  {
    if (state.CartItems != null)
    {
      var items = state.CartItems.ToList();
      var itemToUpdate = items.Find(i =>
        i.ProductId == action.Item.ProductId && i.ProductTypeId == action.Item.ProductTypeId);

      if (itemToUpdate != null)
      {
        itemToUpdate.Qantity = action.Item.Qantity;
      }

      return state with { CartItems = items, CurrentCartItemsCount = items.Sum(x => x.Qantity) };
    }

    return state;
  }
}
