using Blazored.LocalStorage;

namespace BlazorExample.Client.Services.Cart;

public class CartService : ICartService
{
  private readonly ILocalStorageService _localStorageService;

  public event Action? OnChange;

  public CartService(ILocalStorageService localStorageService)
  {
    _localStorageService = localStorageService;
  }

  public async Task AddToCart(CartItem cartItem)
  {
    List<CartItem> cart = await _localStorageService.GetItemAsync<List<CartItem>>("cart") ?? new List<CartItem>();

    cart.Add(cartItem);

    await _localStorageService.SetItemAsync("cart", cart);
  }

  public async Task<IEnumerable<CartItem>> GetCartItems() =>
    await _localStorageService.GetItemAsync<List<CartItem>>("cart") ?? Enumerable.Empty<CartItem>();
}
