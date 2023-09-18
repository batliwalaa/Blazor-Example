using BlazorExample.Client.Services.Product;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace BlazorExample.Client.Shared;

public partial class Search : ComponentBase
{
  [Inject]
  public IProductService? ProductService { get; init; }

  [Inject]
  public NavigationManager? NavigationManager { get; init; }

  private string _searchTerm = string.Empty;
  private IEnumerable<string> _suggestions = new List<string>();
  protected ElementReference _searchInput;

  protected async override Task OnAfterRenderAsync(bool firstRender)
  {
    if (firstRender)
    {
      await _searchInput.FocusAsync();
    }
  }

  public void SearchProducts()
  {
    NavigationManager?.NavigateTo($"search/{_searchTerm}/1");
  }

  public async Task HandleSearch(KeyboardEventArgs eventArgs)
  {
    if (eventArgs.Key == null || eventArgs.Key.Equals("Enter"))
    {
      SearchProducts();
    }
    else if (_searchTerm.Length > 1)
    {
      if (ProductService != null)
      {
        _suggestions = (await ProductService.SearchSuggestions(_searchTerm)).Data ?? new List<string>();
      }
    }
  }
}
