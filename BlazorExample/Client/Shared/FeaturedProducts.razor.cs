using BlazorExample.Client.Services.Product;
using Microsoft.AspNetCore.Components;

namespace BlazorExample.Client.Shared;

public partial class FeaturedProducts : ComponentBase, IDisposable
{
  [Inject]
  public IProductService? ProductService { get; set; }

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
}
