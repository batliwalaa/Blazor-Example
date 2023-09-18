namespace BlazorExample.Client.Services.Cart;

public interface ICartService
{
  event Action OnChange;
  Task AddToCart(CartItem cartItem);
  Task<List<CartItem>> GetCartItems();
}
