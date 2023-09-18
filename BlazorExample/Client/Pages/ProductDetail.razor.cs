using BlazorExample.Client.Services.Product;
using BlazorExample.Client.store.cart;
using Fluxor;
using Microsoft.AspNetCore.Components;

namespace BlazorExample.Client.Pages;

public partial class ProductDetail : ComponentBase
{
  private Product? Detail { get; set; }

  private string Message { get; set; } = "Loading product details...";

  private int? CurrentProductTypeId { get; set; }

  [Parameter] public int Id { get; set; }

  [Inject]
  public IProductService? ProductService { get; init; }

  [Inject]
  public IDispatcher? Dispatcher { get; init; }

  protected override async Task OnParametersSetAsync()
  {
    if (ProductService != null)
    {
      var response = await ProductService.GetProduct(Id);

      if (response != null)
      {
        Detail = response.Data;
        Message = response.Message;

        if (Detail?.Variants?.Any() ?? false)
        {
          CurrentProductTypeId = Detail.Variants.First().ProductTypeId;
        }
      }
    }

    await base.OnParametersSetAsync();
  }

  private ProductVariant? SelectedVariant
  {
    get =>
      Detail?.Variants?.FirstOrDefault(x => x.ProductTypeId == CurrentProductTypeId);
  }

  private async Task AddToCart()
  {
    if (Detail != null)
    {
      ProductVariant? productVariant = SelectedVariant;

      if (productVariant != null)
      {
        var cartItem = new CartItem
        {
          ProductId = productVariant.ProductId,
          ProductTypeId = productVariant.ProductTypeId,
          ImageUrl = Detail.ImageUrl,
          Price = productVariant.Price,
          ProductTypeName = productVariant.ProductType?.Name ?? string.Empty,
          Title = Detail.Title
        };

        Dispatcher?.Dispatch(new CartAddItemAction(cartItem));
      }
    }

    await Task.CompletedTask;
  }
}
