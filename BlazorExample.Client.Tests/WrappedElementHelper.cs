using AngleSharp.Dom;
using AngleSharpWrappers;

namespace BlazorExample.Client.Tests;

internal class WrappedElementHelper
{
  public static T? GetWrappedElement<T>(IElement element)
  {
    var wrappedElement = (element as ElementWrapper)?.WrappedElement;

    if (wrappedElement == null)
    {
      return default;
    }

    return (T)wrappedElement;
  }
}
