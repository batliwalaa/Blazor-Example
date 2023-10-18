using System.ComponentModel.DataAnnotations;

namespace BlazorExample.Shared;

public class UserRegister
{
  [Required, EmailAddress]
  public string Email { get; set; } = string.Empty;

  [Required, StringLength(30, MinimumLength = 6)]
  public string Password { get; set; } = string.Empty;

  [Compare("Password", ErrorMessage = "The paswords do not match.")]
  public string ConfirmPassword { get; set; } = string.Empty;
}
