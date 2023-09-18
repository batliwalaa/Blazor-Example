namespace BlazorExample.Client.Services.Product;

public interface IProductService
{
  string Message { get; }
  string LastSearchText { get; }
  int CurrentPage { get; }
  int PageCount { get; }

  event Action ProductsChanged;

  IEnumerable<BlazorExample.Shared.Product>? Products { get; }
  Task GetProducts();
  Task<ResponseResult<BlazorExample.Shared.Product>> GetProduct(int id);
  Task GetProductsByCategory(string categoryUrl);
  Task SearchProducts(string searchText, int page);
  Task<ResponseResult<IEnumerable<string>>> SearchSuggestions(string searchTerm);
}
