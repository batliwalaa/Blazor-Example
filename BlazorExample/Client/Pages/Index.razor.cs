using BlazorExample.Client.Services.Product;
using Microsoft.AspNetCore.Components;

namespace BlazorExample.Client.Pages;

public partial class Index : ComponentBase
{
  [Parameter]
  public string? CategoryUrl { get; set; }

  [Parameter]
  public string? SearchText { get; set; }

  [Parameter]
  public int Page { get; set; } = 1;

  [Inject]
  public IProductService? ProductService { get; init; }

  protected override async Task OnParametersSetAsync()
  {
    if (ProductService != null)
    {
      if (!string.IsNullOrWhiteSpace(CategoryUrl))
      {
        await ProductService.GetProductsByCategory(CategoryUrl);
      }
      else if (!string.IsNullOrWhiteSpace(SearchText))
      {
        await ProductService.SearchProducts(SearchText, Page);
      }
      else
      {
        await ProductService.GetProducts();
      }
    }
  }
}
