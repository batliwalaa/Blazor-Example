using BlazorExample.Client.store.auth;
using Fluxor;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace BlazorExample.Client.Shared;

public partial class UserButton : ComponentBase
{
  [Inject]
  AuthenticationStateProvider? AuthenticationStateProvider { get; init; }

  [Inject]
  NavigationManager? NavigationManager { get; init; }

  [Inject]
  IDispatcher? Dispatcher { get; init; }

  private bool _showUserMenu = false;
  private string UserMenuCssClass => _showUserMenu ? "show-menu" : string.Empty;

  private void ToggleUserMenu() => _showUserMenu = !_showUserMenu;

  private async Task HideUserMenu()
  {
    await Task.Delay(200);
    _showUserMenu = false;
  }

  private async Task Logout()
  {
    Dispatcher?.Dispatch(new AuthSaveTokenAction(string.Empty));

    if (AuthenticationStateProvider != null)
    {
      await AuthenticationStateProvider.GetAuthenticationStateAsync();
      NavigationManager?.NavigateTo(string.Empty);
    }
  }
}
