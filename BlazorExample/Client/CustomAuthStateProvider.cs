using BlazorExample.Client.store.auth;
using Fluxor;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;

namespace BlazorExample.Client;

public class CustomAuthStateProvider : AuthenticationStateProvider
{
  private readonly HttpClient _httpClient;
  private readonly IState<AuthState> _authenticationState;
  private readonly IDispatcher _dispatcher;

  public CustomAuthStateProvider(
    HttpClient httpClient,
    IState<AuthState> authenticationState,
    IDispatcher dispatcher)
  {
    _httpClient = httpClient;
    _authenticationState = authenticationState;
    _dispatcher = dispatcher;
  }

  public override async Task<AuthenticationState> GetAuthenticationStateAsync()
  {
    string authToken = _authenticationState.Value.Token;
    var identity = new ClaimsIdentity();

    _httpClient.DefaultRequestHeaders.Authorization = null;

    if (!string.IsNullOrWhiteSpace(authToken))
    {
      try
      {
        identity = new ClaimsIdentity(ParseClaimsFromJwt(authToken), "jwt");
        _httpClient.DefaultRequestHeaders.Authorization =
          new AuthenticationHeaderValue("Bearer", authToken.Replace("\"", string.Empty));
      }
      catch
      {
        _dispatcher?.Dispatch(new AuthSaveTokenAction(string.Empty));
      }
    }

    var user = new ClaimsPrincipal(identity);
    var state = new AuthenticationState(user);

    NotifyAuthenticationStateChanged(Task.FromResult(state));

    return await Task.FromResult(state);
  }

  private static byte[] ParseBase64WithoutPadding(string base64)
  {
    switch(base64.Length % 4)
    {
      case 2:
        base64 += "==";
        break;
      case 3:
        base64 += "=";
        break;
    }

    return Convert.FromBase64String(base64);
  }

  private static IEnumerable<Claim> ParseClaimsFromJwt(string authToken)
  {
    var payload = authToken.Split('.')[1];
    var jsonBytes = ParseBase64WithoutPadding(payload);
    var keyValuePairs = 
      JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);
    var claims = keyValuePairs?.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString() ?? string.Empty));

    return claims ?? Array.Empty<Claim>();
  }
}
