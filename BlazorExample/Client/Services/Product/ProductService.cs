namespace BlazorExample.Client.Services.Product;

public class ProductService : ServiceBase, IProductService
{
  public IEnumerable<BlazorExample.Shared.Product>? Products { get; private set; }
  public string Message { get; private set; } = string.Empty;
  public string LastSearchText { get; private set; } = string.Empty;
  public int CurrentPage { get; private set; } = 1;
  public int PageCount { get; private set; }

  public ProductService(HttpClient http) : base(http)
  {
  }

  public event Action? ProductsChanged;

  public async Task GetProducts()
  {
    await GetProductList("/featured");
  }

  public async Task<ResponseResult<BlazorExample.Shared.Product>> GetProduct(int id)
  {
    return await GetAsync<BlazorExample.Shared.Product>($"api/product/{id}");
  }

  public async Task GetProductsByCategory(string categoryUrl)
  {
    await GetProductList($"/category/{categoryUrl}");
  }

  public async Task SearchProducts(string searchText, int page)
  {
    LastSearchText = searchText;
    await GetProductList($"/search/{searchText}/{page}");
  }

  public async Task<ResponseResult<IEnumerable<string>>> SearchSuggestions(string searchTerm)
  {
    return await GetAsync<IEnumerable<string>>($"api/product/search-suggestions/{searchTerm}");
  }

  private async Task GetProductList(string partialRoute = "")
  {
    var response = await GetAsync<IEnumerable<BlazorExample.Shared.Product>>($"api/product{partialRoute}");
    Products = response.Data;
    Message = response.Message;
    CurrentPage = response.CurrentPage.HasValue ? response.CurrentPage.Value : 1;
    PageCount = response.Pages.HasValue ? response.Pages.Value : 0;

    ProductsChanged?.Invoke();
  }
}
