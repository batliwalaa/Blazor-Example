using BlazorExample.Client.Pages;
using BlazorExample.Client.store.cart;
using BlazorExample.Shared;
using FluentAssertions;
using FluentAssertions.Execution;
using Fluxor;
using Microsoft.AspNetCore.Components;

namespace BlazorExample.Client.Tests.Pages;

public class CartRazorTests : TestContext
{
  private readonly IStore _store;
  private readonly IState<CartState> _state;
  private readonly IDispatcher _dispatcher;

  public CartRazorTests()
  {
    Services.AddFluxor(o => o.ScanAssemblies(typeof(CartState).Assembly));
    _store = Services.GetRequiredService<IStore>();
    _state = Services.GetRequiredService<IState<CartState>>();
    _dispatcher = Services.GetRequiredService<IDispatcher>();
    _store.InitializeAsync().Wait();
  }

  [Fact]
  public void When_CartIsEmpty()
  {
    // Arrange.
    // Act.
    IRenderedComponent<Cart> cut = RenderComponent<Cart>();

    // Assert.
    using (new AssertionScope())
    {
      _state.Value.CurrentCartItemsCount.Should().Be(0);
      _state.Value.CartItems.Should().NotBeNull();
      _state.Value.CartItems.Should().BeEmpty();
      cut.Find("[data-testid='empty-cart-message']").TextContent.Should().Be("Your cart is empty.");
    }
  }

  [Fact]
  public void When_CartHasItems()
  {
    // Arrange.
    IRenderedComponent<Cart> cut = RenderComponent<Cart>();
    var cartItem = new CartItem
    {
      ProductId = 1,
      ProductTypeId = 1,
      ImageUrl = "Image Url",
      ProductTypeName = "Product type",
      Title = "Title",
      Price = 2.99m,
      Qantity = 2
    };

    // Act.
    _dispatcher.Dispatch(new CartAddItemAction(cartItem));

    // Assert.
    using (new AssertionScope())
    {
      cut.FindAll("[data-testid='cart-item']").Should().HaveCount(1);

      var image = cut.Find("[data-testid='cart-item']:first-child [data-testid='cart-item-image']");
      image.Should().NotBeNull();
      image.Attributes["src"]?.Value.Should().Be("Image Url");

      AngleSharp.Html.Dom.IHtmlInputElement? inputElement = WrappedElementHelper.GetWrappedElement<AngleSharp.Html.Dom.IHtmlInputElement>(
        cut.Find("[data-testid='cart-item']:first-child [data-testid='cart-item-quantity']"));
      
      inputElement.Should().NotBeNull();
      inputElement?.Value.Should().Be("2");

      cut.Find("[data-testid='cart-item']:first-child [data-testid='cart-item-detail']").Should().NotBeNull();
      cut.Find("[data-testid='cart-item']:first-child [data-testid='cart-item-detail']").GetAttribute("href").Should().Be("/product/1");
      cut.Find("[data-testid='cart-item']:first-child [data-testid='cart-item-remove']").TextContent.Should().Be("Remove");
      cut.Find("[data-testid='cart-item']:first-child [data-testid='cart-item-product-price']").TextContent.Should().Be("$5.98");
      cut.Find("[data-testid='cart-total-price']").TextContent.Should().Contain("$5.98");
    }
  }

  [Fact]
  public void When_CartItem_UpdateQuantity()
  {
    // Arrange.
    IRenderedComponent<Cart> cut = RenderComponent<Cart>();
    var cartItem = new CartItem
    {
      ProductId = 1,
      ProductTypeId = 1,
      ImageUrl = "Image Url",
      ProductTypeName = "Product type",
      Title = "Title",
      Price = 2.99m,
      Qantity = 2
    };
    _dispatcher.Dispatch(new CartAddItemAction(cartItem));

    // Act.
    cut.Find("[data-testid='cart-item']:first-child [data-testid='cart-item-quantity']").Change(3);

    // Assert.
    using (new AssertionScope())
    {
      AngleSharp.Html.Dom.IHtmlInputElement? inputElement = WrappedElementHelper.GetWrappedElement<AngleSharp.Html.Dom.IHtmlInputElement>(
        cut.Find("[data-testid='cart-item']:first-child [data-testid='cart-item-quantity']"));

      inputElement?.Value.Should().Be("3");
      cut.Find("[data-testid='cart-item']:first-child [data-testid='cart-item-product-price']").TextContent.Should().Be("$8.97");
      cut.Find("[data-testid='cart-total-price']").TextContent.Should().Contain("$8.97");
    }
  }

  [Fact]
  public void When_CartItem_UpdateQuantity_IsLessThan_One()
  {
    // Arrange.
    IRenderedComponent<Cart> cut = RenderComponent<Cart>();
    var cartItem = new CartItem
    {
      ProductId = 1,
      ProductTypeId = 1,
      ImageUrl = "Image Url",
      ProductTypeName = "Product type",
      Title = "Title",
      Price = 2.99m,
      Qantity = 1
    };
    _dispatcher.Dispatch(new CartAddItemAction(cartItem));

    // Act.
    cut.Find("[data-testid='cart-item']:first-child [data-testid='cart-item-quantity']").Change(0);

    // Assert.
    using (new AssertionScope())
    {
      AngleSharp.Html.Dom.IHtmlInputElement? inputElement = WrappedElementHelper.GetWrappedElement<AngleSharp.Html.Dom.IHtmlInputElement>(
        cut.Find("[data-testid='cart-item']:first-child [data-testid='cart-item-quantity']"));

      inputElement?.Value.Should().Be("1");
      cut.Find("[data-testid='cart-item']:first-child [data-testid='cart-item-product-price']").TextContent.Should().Be("$2.99");
      cut.Find("[data-testid='cart-total-price']").TextContent.Should().Contain("$2.99");
    }
  }

  [Fact]
  public void When_CartItem_UpdateQuantity_IsNull()
  {
    // Arrange.
    IRenderedComponent<Cart> cut = RenderComponent<Cart>();
    var cartItem = new CartItem
    {
      ProductId = 1,
      ProductTypeId = 1,
      ImageUrl = "Image Url",
      ProductTypeName = "Product type",
      Title = "Title",
      Price = 2.99m,
      Qantity = 1
    };
    _dispatcher.Dispatch(new CartAddItemAction(cartItem));

    // Act.
    cut.Find("[data-testid='cart-item']:first-child [data-testid='cart-item-quantity']").Change((new ChangeEventArgs()));

    // Assert.
    using (new AssertionScope())
    {
      AngleSharp.Html.Dom.IHtmlInputElement? inputElement = WrappedElementHelper.GetWrappedElement<AngleSharp.Html.Dom.IHtmlInputElement>(
        cut.Find("[data-testid='cart-item']:first-child [data-testid='cart-item-quantity']"));

      inputElement?.Value.Should().Be("1");
      cut.Find("[data-testid='cart-item']:first-child [data-testid='cart-item-product-price']").TextContent.Should().Be("$2.99");
      cut.Find("[data-testid='cart-total-price']").TextContent.Should().Contain("$2.99");
    }
  }

  [Fact]
  public void When_Remove_CartItem()
  {
    // Arrange.
    IRenderedComponent<Cart> cut = RenderComponent<Cart>();
    var cartItem = new CartItem
    {
      ProductId = 1,
      ProductTypeId = 1,
      ImageUrl = "Image Url",
      ProductTypeName = "Product type",
      Title = "Title",
      Price = 2.99m
    };
    _dispatcher.Dispatch(new CartAddItemAction(cartItem));

    // Act.
    cut.Find("[data-testid='cart-item']:first-child [data-testid='cart-item-remove']").Click();

    // Assert.
    using (new AssertionScope())
    {
      cut.FindAll("[data-testid='cart-item']").Should().HaveCount(0);
    }
  }
}
