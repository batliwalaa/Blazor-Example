namespace BlazorExample.Client.store.auth;

public class AuthSaveTokenAction
{
  public string Token { get; }

  public AuthSaveTokenAction(string token)
  {
    Token = token;
  }
}
