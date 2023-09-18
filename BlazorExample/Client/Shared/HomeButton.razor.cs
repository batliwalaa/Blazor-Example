using Microsoft.AspNetCore.Components;

namespace BlazorExample.Client.Shared;

public partial class HomeButton : ComponentBase
{
  [Inject]
  public NavigationManager? NavigationManager { get; init; }

  public void GoToHome()
  {
    NavigationManager?.NavigateTo("");
  }
}
