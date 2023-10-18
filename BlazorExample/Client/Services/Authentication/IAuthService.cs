namespace BlazorExample.Client.Services.Authentication;

public interface IAuthService
{
  Task<ResponseResult<int>> Register(UserRegister userRegister);
  Task<ResponseResult<string>> Login(UserLogin userLogin);
}
