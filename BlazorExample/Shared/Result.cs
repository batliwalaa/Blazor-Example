namespace BlazorExample.Shared;
public class Result<T>
{
  public T? Data { get; set; }

  public bool Success { get; set; } = true;

  public string Message { get; set; } = string.Empty;

  public int? Pages { get; set; } = 0;

  public int? CurrentPage { get; set; } = 1;
}
