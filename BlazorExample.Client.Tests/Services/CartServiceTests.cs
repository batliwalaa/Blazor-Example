using Blazored.LocalStorage;
using BlazorExample.Client.Services.Cart;
using BlazorExample.Shared;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BlazorExample.Client.Tests;
using FluentAssertions.Execution;
using FluentAssertions;

namespace BlazorExample.Client.Tests.Services;

public class CartServiceTests : TestContext
{
  private readonly Mock<ILocalStorageService> _mockStorageService;
  private readonly ICartService _cartService;

  public CartServiceTests()
  {
    _mockStorageService = new Mock<ILocalStorageService>();
    _cartService = new CartService(_mockStorageService.Object);
  }

  [Fact]
  public async Task When_AddToCart_Called_Should_Add_Item_To_Cart()
  {
    // Arrange.
    _mockStorageService
      .Setup(x => x.GetItemAsync<List<CartItem>>(It.IsAny<string>(), It.IsAny<CancellationToken>()))
      .ReturnsAsync(new List<CartItem>());
    _mockStorageService.Setup(x =>
      x.SetItemAsync(It.IsAny<string>(), It.IsAny<List<CartItem>>(), It.IsAny<CancellationToken>()));

    // Act.
    await _cartService.AddToCart(new CartItem { ProductId = 1, ProductTypeId = 1 });


    // Assert.
    using (new AssertionScope())
    {
      _mockStorageService.Verify(x =>
        x.GetItemAsync<List<CartItem>>(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
      _mockStorageService.Verify(x =>
        x.SetItemAsync(It.IsAny<string>(), It.IsAny<List<CartItem>>(), It.IsAny<CancellationToken>()), Times.Once);
    }
  }

  [Fact]
  public async Task When_GetCartItems_Called_Should_Return_List_Of_CartItems()
  {
    // Arrange.
    _mockStorageService
      .Setup(x => x.GetItemAsync<List<CartItem>>(It.IsAny<string>(), It.IsAny<CancellationToken>()))
      .ReturnsAsync(new List<CartItem>());

    // Act.
    List<CartItem> items = await _cartService.GetCartItems();

    // Assert.
    using (new AssertionScope())
    {
      _mockStorageService.Verify(x =>
        x.GetItemAsync<List<CartItem>>(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
      items.Should().NotBeNull();
      items.Should().BeOfType<List<CartItem>>();
    }
  }
}
