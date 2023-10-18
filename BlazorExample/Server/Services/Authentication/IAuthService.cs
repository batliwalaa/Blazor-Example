namespace BlazorExample.Server.Services.Authentication;

public interface IAuthService
{
  Task<Result<int>> Register(User user, string password);
  Task<bool> UserExists(string email);
  Task<Result<string>> Login(string userName, string password);
}
