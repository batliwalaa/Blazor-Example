namespace BlazorExample.Client.Services.Authentication;

public class AuthService : ServiceBase, IAuthService
{
  public AuthService(HttpClient http) : base(http)
  {
  }

  public async Task<ResponseResult<string>> Login(UserLogin userLogin)
  {
    return await PostAsync<string, UserLogin>("api/auth/login", userLogin);
  }

  public async Task<ResponseResult<int>> Register(UserRegister userRegister)
  {
    return await PostAsync<int, UserRegister>("api/auth/register", userRegister);
  }
}
