using BlazorExample.Client.Services.Category;
using BlazorExample.Client.Shared;
using FluentAssertions.Execution;
using FluentAssertions;
using Microsoft.AspNetCore.Components;
using Moq;
using BlazorExample.Client.Services.Product;

namespace BlazorExample.Client.Tests.Shared;

public class MainLayoutRazorTests : TestContext
{
  public MainLayoutRazorTests()
  {
    Services.AddSingleton(new Mock<ICategoryService>().Object);
    Services.AddSingleton(new Mock<IProductService>().Object);
    Services.AddSingleton<FakeNavigationManager>();
    Services.AddSingleton<NavigationManager>(s => s.GetRequiredService<FakeNavigationManager>());
    Services.AddMockHttpClient();
  }

  [Fact]
  public void When_Initialised()
  {
    // Arrange.
    // Act.
    IRenderedComponent<MainLayout> cut = RenderComponent<MainLayout>();

    // Assert.
    using (new AssertionScope())
    {
      cut.FindComponent<NavMenu>().Should().NotBeNull();
      cut.FindComponent<Search>().Should().NotBeNull();
    }
  }
}
