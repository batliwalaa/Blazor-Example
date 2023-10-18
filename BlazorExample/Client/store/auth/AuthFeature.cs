using BlazorExample.Client.store.cart;
using Fluxor;

namespace BlazorExample.Client.store.auth;

public class AuthFeature : Feature<AuthState>
{
  public override string GetName() => "Auth";

  protected override AuthState GetInitialState()
  {
    return new AuthState { Token = string.Empty };
  }
}
