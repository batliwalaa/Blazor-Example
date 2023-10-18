namespace BlazorExample.Client.store.auth;

public record AuthState
{
  public string Token { get; set; } = string.Empty;
}
