using Fluxor;

namespace BlazorExample.Client.store.auth;

public static class AuthReducers
{
  [ReducerMethod]
  public static AuthState OnSaveAuthToken(AuthState authState, AuthSaveTokenAction action)
  {
    return authState with { Token = action.Token };
  }
}
