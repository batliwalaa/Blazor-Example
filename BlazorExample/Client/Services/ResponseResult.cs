using System.Net;

namespace BlazorExample.Client.Services;

public class ResponseResult<T> : Result<T>
{
  public ResponseResult(HttpStatusCode statusCode)
  {
    StatusCode = statusCode;
  }

  public HttpStatusCode StatusCode { get; }
}
