using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace BlazorExample.Client.Services;

public abstract class ServiceBase
{
  private readonly HttpClient _httpClient;

  public ServiceBase(HttpClient httpClient)
  {
    _httpClient = httpClient;
  }

  public async Task<ResponseResult<T>> GetAsync<T>(string partialUrl) where T : class
  {
    Result<T>? result = null;
    HttpResponseMessage? response = null;

    if (_httpClient != null)
    {
      response = await _httpClient.GetAsync(partialUrl);
      result = await response.Content.ReadFromJsonAsync<Result<T>>();
    }

    return new ResponseResult<T>(response?.StatusCode ?? HttpStatusCode.ServiceUnavailable)
    {
      Data = result?.Data,
      Message = result?.Message ?? string.Empty,
      Success = result?.Success ?? false,
      Pages = result?.Pages,
      CurrentPage = result?.CurrentPage
    };
  }

  public async Task<ResponseResult<TOut>> PostAsync<TOut, TIn>(string partialUrl, TIn data)
    where TIn : class
  {
    Result<TOut>? result = null;
    HttpResponseMessage? response = null;

    if (_httpClient != null)
    {
      HttpContent content = JsonContent.Create(data, new MediaTypeHeaderValue("application/json"));
      response = await _httpClient.PostAsync(partialUrl, content);
      result = await response.Content.ReadFromJsonAsync<Result<TOut>>();
    }

    return new ResponseResult<TOut>(response?.StatusCode ?? HttpStatusCode.ServiceUnavailable)
    {
      Data = result == null ? default : result.Data,
      Message = result?.Message ?? string.Empty,
      Success = result?.Success ?? false,
      Pages = result?.Pages,
      CurrentPage = result?.CurrentPage
    };
  }
}
