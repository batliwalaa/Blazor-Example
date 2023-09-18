namespace BlazorExample.Server.Services.Product;

public interface IProductService
{
  Task<Result<IEnumerable<Shared.Product>>> GetProductsAsync();

  Task<Result<Shared.Product>> GetProductByIdAsync(int id);

  Task<Result<IEnumerable<Shared.Product>>> GetProductsByCategoryAsync(string category);

  Task<Result<IEnumerable<Shared.Product>>> Search(string searchTerm, int page);

  Task<Result<IEnumerable<string>>> GetProductSearchSuggestions(string searchTerm);

  Task<Result<IEnumerable<Shared.Product>>> GetFeaturedProducts();
}
