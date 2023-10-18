using BlazorExample.Client.Pages;
using BlazorExample.Client.Services;
using BlazorExample.Client.Services.Product;
using BlazorExample.Client.store.cart;
using BlazorExample.Shared;
using FluentAssertions;
using FluentAssertions.Execution;
using Fluxor;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace BlazorExample.Client.Tests.Pages;
public class ProductDetailRazorTests : TestContext
{
  private readonly Mock<IProductService> _productServiceMock;
  private readonly IStore _store;

  public ProductDetailRazorTests()
  {
    Services.AddMockHttpClient();
    _productServiceMock = new Mock<IProductService>();
    Services.AddSingleton(_productServiceMock.Object);
    Services.AddFluxor(o => o.ScanAssemblies(typeof(CartState).Assembly));
    _store = Services.GetRequiredService<IStore>();
    _store.InitializeAsync().Wait();
  }

  public class Initialized : ProductDetailRazorTests
  {
    [Fact]
    public void When_Initialized()
    {
      // Arrange.
      // Act.
      IRenderedComponent<ProductDetail> cut =
          RenderComponent<ProductDetail>(parameters => parameters.Add(p => p.Id, 1));

      // Assert.
      using (new AssertionScope())
      {
        cut.Instance.ProductService.Should().NotBeNull();
        cut.Instance.Dispatcher.Should().NotBeNull();
        cut.Instance.Id.Should().Be(1);
      }
    }
  }

  public class NotFound : ProductDetailRazorTests
  {
    [Fact]
    public void When_ProductDetail_NotFound()
    {
      // Arrange.
      ResponseResult<Product> response = new(HttpStatusCode.NotFound)
      {
        Message = "Product detail not found."
      };
      string expectedMessage = "Product detail not found.";

      _productServiceMock.Setup(x => x.GetProduct(It.IsAny<int>())).ReturnsAsync(response);

      // Act.
      IRenderedComponent<ProductDetail> cut =
          RenderComponent<ProductDetail>(parameters => parameters.Add(p => p.Id, 1));

      // Assert.
      using (new AssertionScope())
      {
        cut.Find("span").TextContent.Should().Be(expectedMessage);
      }
    }
  }

  public class Found : ProductDetailRazorTests
  {
    [Fact]
    public void When_ProductDetail_Found()
    {
      // Arrange.
      ResponseResult<Product> response = new(HttpStatusCode.OK)
      {
        Success = true,
        Data = new Product
        {
          Title = "Product Title",
          ImageUrl = "Image Url",
          Description = "Product Description",
          Variants = new List<ProductVariant>
                {
                    new ProductVariant
                    {
                        Price = 12.99m,
                        OriginalPrice = 12.99m,
                        ProductTypeId = 2,
                        ProductType = new ProductType { Id = 2, Name = "Product Type" }
                    }
                }
        }
      };

      _productServiceMock.Setup(x => x.GetProduct(It.IsAny<int>())).ReturnsAsync(response);

      // Act.
      IRenderedComponent<ProductDetail> cut =
          RenderComponent<ProductDetail>(parameters => parameters.Add(p => p.Id, 1));

      // Assert.
      using (new AssertionScope())
      {
        var image = cut.Find("[data-testid='product-image']");
        image.Should().NotBeNull();
        image.Attributes["src"]?.Value.Should().Be("Image Url");
        image.Attributes["alt"]?.Value.Should().Be("Product Title");

        cut.Find("[data-testid='product-title']").TextContent.Should().Be("Product Title");
        cut.Find("[data-testid='product-description']").TextContent.Should().Be("Product Description");
        cut.Find("[data-testid='product-price']").TextContent.Should().Contain("$12.99");

        var select = cut.Find("[data-testid='product-variants']");
        select.Should().NotBeNull();
        select.ChildNodes.Length.Should().Be(1);
        select.ChildNodes[0].TextContent.Should().Be("Product Type");
      }
    }
  }

  public class PriceNotEqualOriginalPrice : ProductDetailRazorTests
  {

    [Fact]
    public void When_ProductDetail_Price_NotEquals_OriginalPrice()
    {
      // Arrange.
      ResponseResult<Product> response = new(HttpStatusCode.OK)
      {
        Success = true,
        Data = new Product
        {
          Title = "Product Title",
          ImageUrl = "Image Url",
          Description = "Product Description",
          Variants = new List<ProductVariant>
                {
                    new ProductVariant
                    {
                        Price = 12.99m,
                        OriginalPrice = 13.99m,
                        ProductTypeId = 2,
                        ProductType = new ProductType { Id = 2, Name = "Product Type" }
                    }
                }
        }
      };
      _productServiceMock.Setup(x => x.GetProduct(It.IsAny<int>())).ReturnsAsync(response);

      // Act.
      IRenderedComponent<ProductDetail> cut =
          RenderComponent<ProductDetail>(parameters => parameters.Add(p => p.Id, 1));

      // Assert.
      using (new AssertionScope())
      {
        cut.Find("[data-testid='product-price']").TextContent.Should().Contain("$12.99");
        cut.Find("[data-testid='product-original-price']").TextContent.Should().Contain("$13.99");
        cut.Find("[data-testid='product-original-price']").ClassName.Should().Contain("original-price");
      }
    }
  }

  public class AddToCart : ProductDetailRazorTests
  {
    [Fact]
    public void When_AddToCart_Clicked_Should_Add_CartItem_To_State()
    {
      // Arrange.
      ResponseResult<Product> response = new(HttpStatusCode.OK)
      {
        Success = true,
        Data = new Product
        {
          Title = "Product Title",
          ImageUrl = "Image Url",
          Description = "Product Description",
          Variants = new List<ProductVariant>
          {
            new ProductVariant
            {
              ProductId = 1,
              Price = 12.99m,
              OriginalPrice = 12.99m,
              ProductTypeId = 2,
              ProductType = new ProductType { Id = 2, Name = "Product Type" }
            }
          }
        }
      };
      _productServiceMock.Setup(x => x.GetProduct(It.IsAny<int>())).ReturnsAsync(response);
      IRenderedComponent<ProductDetail> cut =
          RenderComponent<ProductDetail>(parameters => parameters.Add(p => p.Id, 1));
      IState<CartState> state = Services.GetRequiredService<IState<CartState>>();
      // Act.
      cut.Find("[data-testid='button-addtocart']").Click();

      // Assert.
      using (new AssertionScope())
      {
        state.Value.CurrentCartItemsCount.Should().Be(1);
        state.Value.CartItems.Should().NotBeNullOrEmpty();
        state.Value.CartItems?.First()
          .Should()
          .BeEquivalentTo(new CartItem
          {
            ProductId = 1,
            ProductTypeId = 2,
            ImageUrl = "Image Url",
            Title = "Product Title",
            ProductTypeName = "Product Type",
            Price = 12.99m,
            Qantity = 1,
          });
      }
    }

    [Fact]
    public void When_AddToCart_Clicked_For_ItemAlreadyAdded_ToCart()
    {
      // Arrange.
      ResponseResult<Product> response = new(HttpStatusCode.OK)
      {
        Success = true,
        Data = new Product
        {
          Title = "Product Title",
          ImageUrl = "Image Url",
          Description = "Product Description",
          Variants = new List<ProductVariant>
          {
            new ProductVariant
            {
              ProductId = 1,
              Price = 12.99m,
              OriginalPrice = 12.99m,
              ProductTypeId = 2,
              ProductType = new ProductType { Id = 2, Name = "Product Type" }
            }
          }
        }
      };
      _productServiceMock.Setup(x => x.GetProduct(It.IsAny<int>())).ReturnsAsync(response);
      IRenderedComponent<ProductDetail> cut =
          RenderComponent<ProductDetail>(parameters => parameters.Add(p => p.Id, 1));
      IState<CartState> state = Services.GetRequiredService<IState<CartState>>();
      cut.Find("[data-testid='button-addtocart']").Click();

      // Act.
      cut.Find("[data-testid='button-addtocart']").Click();

      // Assert.
      using (new AssertionScope())
      {
        state.Value.CurrentCartItemsCount.Should().Be(2);
        state.Value.CartItems.Should().NotBeNullOrEmpty();
        state.Value.CartItems?.First()
          .Should()
          .BeEquivalentTo(new CartItem
          {
            ProductId = 1,
            ProductTypeId = 2,
            ImageUrl = "Image Url",
            Title = "Product Title",
            ProductTypeName = "Product Type",
            Price = 12.99m,
            Qantity = 2,
          });
      }
    }
  }

  protected override void Dispose(bool disposing)
  {
    if (disposing)
    {
      DisposeComponents();
    }
  }
}
