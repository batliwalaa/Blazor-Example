using BlazorExample.Client.Services;
using BlazorExample.Client.Services.Authentication;
using BlazorExample.Client.store.auth;
using Fluxor;
using Fluxor.Blazor.Web.Middlewares.Routing;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace BlazorExample.Client.Pages;

public partial class Login : ComponentBase
{
  private readonly UserLogin user = new();
  private string errorMessage = string.Empty;

  [Inject]
  private IAuthService? AuthService { get; init; }

  [Inject]
  private IDispatcher? Dispatcher { get; init; }

  [Inject]
  private NavigationManager? NavigationManager { get; init; }

  [Inject]
  AuthenticationStateProvider? AuthenticationStateProvider { get; init; }

  private async Task HandleLogin()
  {
    if (AuthService != null) 
    {
      ResponseResult<string> response = await AuthService.Login(user);
      errorMessage = response.Message;

      if (response.Success)
      {
        Dispatcher?.Dispatch(new AuthSaveTokenAction(response.Data ?? string.Empty));
        await AuthenticationStateProvider.GetAuthenticationStateAsync();
        Dispatcher?.Dispatch(new GoAction(string.Empty));
      }
    }    
  }
}
