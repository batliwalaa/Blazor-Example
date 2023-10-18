using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace BlazorExample.Server.Services.Authentication;

public class AuthService : IAuthService
{
  private readonly ApplicationDbContext _context;
  private readonly IConfiguration _configuration;

  public AuthService(ApplicationDbContext context, IConfiguration configuration)
  {
    _context = context;
    _configuration = configuration;
  }

  public async Task<Result<string>> Login(string userName, string password)
  {
    var user = await _context.Users.FirstOrDefaultAsync(x => x.Email.ToLower().Equals(userName.ToLower()));
    var result = new Result<string> { Data = "token" };

    if (user != null)
    {
      if (VerifyPassword(password, user.PasswordHash, user.PasswordSalt))
      {
        return new Result<string>
        {
          Data = CreateToken(user)
        };
      }
    }

    return new Result<string>
    {
      Success = false,
      Message = "Invalid login."
    };
  }

  public async Task<Result<int>> Register(User user, string password)
  {
    if (await UserExists(user.Email))
    {
      return new Result<int>
      {
        Success = false,
        Message = "User already exists."
      };
    }

    CreatePasswordHash(password, out byte[] hash, out byte[] salt);

    user.PasswordHash = hash;
    user.PasswordSalt = salt;

    _context.Users.Add(user);

    await _context.SaveChangesAsync();

    return new Result<int> { Data = user.Id };
  }

  public async Task<bool> UserExists(string email) =>
    await _context.Users.AnyAsync(u =>
      u.Email.ToLower().Equals(email.ToLower()));

  private void CreatePasswordHash(string password, out byte[] hash, out byte[] salt)
  {
    using (var hmac = new HMACSHA512())
    {
      salt = hmac.Key;
      hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
    }
  }

  private bool VerifyPassword(string password, byte[] hash, byte[] salt)
  {
    using (var hmac = new HMACSHA512(salt))
    {
      var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

      return computedHash.SequenceEqual(hash);
    }
  }

  private string CreateToken(User user)
  {
    List<Claim> claims = new()
    {
      new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
      new Claim(ClaimTypes.Name, user.Email)
    };

    var key = new SymmetricSecurityKey(
      Encoding.UTF8.GetBytes(
        _configuration.GetSection("AppSettings:TokenKey")?.Value ?? string.Empty));
    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
    var token = new JwtSecurityToken(
      claims: claims,
      expires: DateTime.Now.AddDays(1),
      signingCredentials: creds);
    var jwt = new JwtSecurityTokenHandler().WriteToken(token);

    return jwt;
  }
}
