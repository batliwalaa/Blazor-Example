using BlazorExample.Server.Services.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace BlazorExample.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
  private readonly IAuthService _authService;

  public AuthController(IAuthService authService)
  {
    _authService = authService;
  }

  [HttpPost("register")]
  public async Task<ActionResult<Result<int>>> Register(UserRegister userRegister)
  {
    var result = await _authService.Register(new User { Email = userRegister.Email }, userRegister.Password);

    return !result.Success ? BadRequest(result) : Ok(result);
  }

  [HttpPost("login")]
  public async Task<ActionResult<Result<string>>> Login(UserLogin userLogin)
  {
    var result = await _authService.Login(userLogin.UserName, userLogin.Password);

    return !result.Success ? BadRequest(result) : Ok(result);
  }
}
