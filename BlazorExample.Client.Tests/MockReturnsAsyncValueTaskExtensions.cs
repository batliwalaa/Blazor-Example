using Moq;
using Moq.Language;
using Moq.Language.Flow;
using System.Threading.Tasks;

namespace BlazorExample.Client.Tests;

public static class MockReturnsAsyncValueTaskExtensions
{
  public static IReturnsResult<TMock> ReturnsAsync<TMock, TResult>(
    this IReturns<TMock,
    ValueTask<TResult>> mock,
    TResult value) where TMock : class
  {
    return mock.ReturnsAsync(() => value);
  }
}
