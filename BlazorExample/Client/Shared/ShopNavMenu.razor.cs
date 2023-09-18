using BlazorExample.Client.Services;
using BlazorExample.Client.Services.Category;
using Microsoft.AspNetCore.Components;
using System.Net;

namespace BlazorExample.Client.Shared;

public partial class ShopNavMenu : ComponentBase
{
  [Inject]
  public ICategoryService? CategoryService { get; init; }

  private bool collapseNavMenu = true;

  private string? NavMenuCssClass => collapseNavMenu ? "collapse" : null;
  private ResponseResult<IEnumerable<Category>> CategoryResponse { get; set; } = new(HttpStatusCode.OK);

  protected void ToggleNavMenu()
  {
    collapseNavMenu = !collapseNavMenu;
  }

  protected override async Task OnInitializedAsync()
  {
    if (CategoryService != null)
    {
      CategoryResponse = await CategoryService.GetCategories();
    }
  }
}
