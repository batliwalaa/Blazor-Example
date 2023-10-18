namespace BlazorExample.Shared;

public class User
{
  public int Id { get; set; }
  public string Email { get; set; } = string.Empty;
  public byte[] PasswordHash { get; set; } = Array.Empty<byte>();
  public byte[] PasswordSalt { get; set; } = Array.Empty<byte>();
  public DateTime DateCreated { get; set; } = DateTime.Now;
}
