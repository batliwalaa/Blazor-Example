using BlazorExample.Client.Services.Authentication;
using Microsoft.AspNetCore.Components;

namespace BlazorExample.Client.Pages;

public partial class Register : ComponentBase
{
  private UserRegister user = new();

  private string errorMessage = string.Empty;

  [Inject]
  public IAuthService? AuthService { get; init; }

  private async Task HandleRegistration()
  {
    if (AuthService != null)
    {
      var result = await AuthService.Register(user);

      errorMessage = result.Success ? string.Empty : result.Message;
    }
  }
}
