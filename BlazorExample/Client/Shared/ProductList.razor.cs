using BlazorExample.Client.Services.Product;
using Microsoft.AspNetCore.Components;

namespace BlazorExample.Client.Shared;

public partial class ProductList : ComponentBase, IDisposable
{
  [Inject]
  public IProductService? ProductService { get; init; }

  public void Dispose()
  {
    if (ProductService != null)
    {
      ProductService.ProductsChanged -= StateHasChanged;
    }
    GC.SuppressFinalize(this);
  }

  protected override async Task OnInitializedAsync()
  {
    if (ProductService != null)
    {
      ProductService.ProductsChanged += StateHasChanged;
    }

    await Task.CompletedTask;
  }

  private static string GetPriceText(Product product)
  {
    var variants = product.Variants;

    if (variants?.Any() ?? false)
    {
      string priceText = variants.Count() == 1 ? "" : "Starting at ";

      return $"{priceText}${variants.Min(x => x.Price)}";
    }

    return string.Empty;
  }
}