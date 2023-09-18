using BlazorExample.Client.Services.Category;
using BlazorExample.Client.Services.Product;
using BlazorExample.Client.Shared;
using BlazorExample.Client.store.cart;
using FluentAssertions;
using FluentAssertions.Execution;
using Fluxor;
using Microsoft.AspNetCore.Components;
using Moq;

namespace BlazorExample.Client.Tests.Shared;
public class ShopLayoutRazorTests : TestContext
{
  private readonly IStore _store;
  private readonly IState<CartState> _state;

  public ShopLayoutRazorTests()
  {
    Services.AddSingleton(new Mock<ICategoryService>().Object);
    Services.AddSingleton(new Mock<IProductService>().Object);
    Services.AddSingleton<FakeNavigationManager>();
    Services.AddSingleton<NavigationManager>(s => s.GetRequiredService<FakeNavigationManager>());
    Services.AddMockHttpClient();
    Services.AddFluxor(o => o.ScanAssemblies(typeof(CartState).Assembly));
    _store = Services.GetRequiredService<IStore>();
    _state = Services.GetRequiredService<IState<CartState>>();
    _store.InitializeAsync().Wait();
  }

  [Fact]
  public void When_Initialised()
  {
    // Arrange.
    // Act.
    IRenderedComponent<ShopLayout> cut = RenderComponent<ShopLayout>();

    // Assert.
    using (new AssertionScope())
    {
      cut.FindComponent<ShopNavMenu>().Should().NotBeNull();
      cut.FindComponent<Search>().Should().NotBeNull();
      cut.FindComponent<HomeButton>().Should().NotBeNull();
      cut.Find("article").Should().NotBeNull();
    }
  }
}
