using BlazorExample.Client.Shared;
using BlazorExample.Client.store.cart;
using BlazorExample.Shared;
using FluentAssertions;
using FluentAssertions.Execution;
using Fluxor;
using System.Linq;

namespace BlazorExample.Client.Tests.Shared;

public class CartCounterRazorTests : TestContext
{
  private readonly IStore _store;
  private readonly IState<CartState> _state;
  private readonly IDispatcher _dispatcher;

  public CartCounterRazorTests()
  {
    Services.AddFluxor(o => o.ScanAssemblies(typeof(CartState).Assembly));
    _store = Services.GetRequiredService<IStore>();
    _state = Services.GetRequiredService<IState<CartState>>();
    _dispatcher = Services.GetRequiredService<IDispatcher>();
    _store.InitializeAsync().Wait();
  }

  [Fact]
  public void When_Initialized()
  {
    // Arrange.
    // Act.
    IRenderedComponent<CartCounter> cut = RenderComponent<CartCounter>();

    // Assert.
    using (new AssertionScope())
    {
      _state.Value.CurrentCartItemsCount.Should().Be(0);
      _state.Value.CartItems.Should().NotBeNull();
      _state.Value.CartItems.Should().BeEmpty();
      cut.Find("[data-testid='cart-counter-badge']").TextContent.Should().Be("0");
    }
  }

  [Fact]
  public void When_ItemAdded_To_Cart()
  {
    // Arrange

    IRenderedComponent<CartCounter> cut = RenderComponent<CartCounter>();
    var cartItem = new CartItem
    {
      ProductId = 1,
      ProductTypeId = 1
    };

    // Act.
    _dispatcher.Dispatch(new CartAddItemAction(cartItem));

    // Assert.
    using (new AssertionScope())
    {
      _state.Value.CurrentCartItemsCount.Should().Be(1);
      _state.Value.CartItems.Should().NotBeNull();
      _state.Value.CartItems?.First().Should().BeSameAs(cartItem);
      cut.Find("[data-testid='cart-counter-badge']").TextContent.Should().Be("1");
    }
  }
}
