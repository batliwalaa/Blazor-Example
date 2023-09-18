using BlazorExample.Client.Services;
using BlazorExample.Client.Services.Product;
using BlazorExample.Client.Shared;
using BlazorExample.Shared;
using FluentAssertions;
using FluentAssertions.Execution;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BlazorExample.Client.Tests.Shared;

public class FeaturedProductsRazorTests : TestContext
{
  private readonly Mock<IProductService> _productServiceMock;

  public FeaturedProductsRazorTests()
  {
    _productServiceMock = new Mock<IProductService>();
    Services.AddSingleton(_productServiceMock.Object);
  }

  public class Initialized : FeaturedProductsRazorTests
  {

    [Fact]
    public void When_Initialized()
    {
      // Arrange.
      Services.AddMockHttpClient();

      // Act.
      IRenderedComponent<FeaturedProducts> cut =
          RenderComponent<FeaturedProducts>();

      // Assert.
      using (new AssertionScope())
      {
        cut.Instance.ProductService?.Should().NotBeNull();
        cut.Find("[data-testid='featured-products-title']").TextContent.Should().Be("Top Products of Today");
      }
    }
  }

  public class GetProducts_EmptyOrNull : FeaturedProductsRazorTests
  {
    [Fact]
    public void When_ProductList_NotFound()
    {
      // Arrange.
      ResponseResult<IEnumerable<Product>> response = new(HttpStatusCode.NotFound)
      {
        Message = "Products not found."
      };
      Services.AddMockHttpClient();
      string expectedMessage = "Products not found.";

      _productServiceMock.Setup(x => x.GetProductsByCategory(It.IsAny<string>()));
      _productServiceMock.SetupGet(x => x.Message).Returns(expectedMessage);
      _productServiceMock.SetupGet(x => x.Products).Returns(default(IEnumerable<Product>));

      // Act.
      IRenderedComponent<ProductList> cut =
          RenderComponent<ProductList>();

      if (cut.Instance.ProductService != null)
      {
        cut.InvokeAsync(async () =>
        {
          if (cut.Instance.ProductService != null)
          {
            await cut.Instance.ProductService.GetProductsByCategory("video-games");
          }
        });
      }

      // Assert.
      using (new AssertionScope())
      {
        cut.Find("[data-testid='list-message']").TextContent.Should().Be(expectedMessage);
      }
    }
  }

  public class GetProducts : FeaturedProductsRazorTests
  {
    [Fact]
    public void When_ProductList_Product_With_Multiple_Variants()
    {
      // Arrange.
      IEnumerable<Product> productList = new List<Product>
      {
        new Product
        {
          Id = 1,
          Title = "Product Title",
          ImageUrl = "Image Url",
          Description = "Product Description",
          Featured = true,
          Variants = new List<ProductVariant>
          {
            new ProductVariant
            {
              Price = 12.99m,
              OriginalPrice = 12.99m,
              ProductTypeId = 2,
              ProductType = new ProductType { Id = 1, Name = "Product Type 1" }
            },
            new ProductVariant
            {
              Price = 2.99m,
              OriginalPrice = 2.99m,
              ProductTypeId = 2,
              ProductType = new ProductType { Id = 2, Name = "Product Type 2" }
            }
          }
        }
      };

      Services.AddMockHttpClient();
      _productServiceMock.Setup(x => x.GetProducts());
      _productServiceMock.SetupGet(x => x.Products).Returns(productList);

      // Act.
      IRenderedComponent<FeaturedProducts> cut =
          RenderComponent<FeaturedProducts>();

      if (cut.Instance.ProductService != null)
      {
        cut.InvokeAsync(async () =>
        {
          if (cut.Instance.ProductService != null)
          {
            await cut.Instance.ProductService.GetProducts();
          }
        });
      }

      // Assert.
      using (new AssertionScope())
      {
        cut.FindAll("[data-testid='featured-product-item']").Should().HaveCount(1);

        cut.Find(".featured-product:first-child [data-testid='featured-product-img-link']").Should().NotBeNull();
        cut.Find(".featured-product:first-child [data-testid='featured-product-img-link']").GetAttribute("href").Should().Be("/product/1");
        var image = cut.Find(".featured-product:first-child [data-testid='featured-product-img-link'] img");
        image.Should().NotBeNull();
        image.Attributes["src"]?.Value.Should().Be("Image Url");
        image.Attributes["alt"]?.Value.Should().Be("Product Title");

        cut.Find(".featured-product:first-child [data-testid='featured-product-title']").Should().NotBeNull();
        cut.Find(".featured-product:first-child [data-testid='featured-product-title']").GetAttribute("href").Should().Be("/product/1");
        cut.Find(".featured-product:first-child [data-testid='featured-product-title']").TextContent.Should().Be("Product Title");
        cut.Find(".featured-product:first-child [data-testid='featured-product-price']").TextContent.Should().Contain("$12.99");
      }
    }

    [Fact]
    public void When_ProductList_Product_IsNot_Featured_Should_Not_Show_Product()
    {
      // Arrange.
      IEnumerable<Product> productList = new List<Product>
      {
        new Product
        {
          Id = 1,
          Title = "Product Title",
          ImageUrl = "Image Url",
          Description = "Product Description",
          Featured = false,
          Variants = new List<ProductVariant>
          {
            new ProductVariant
            {
              Price = 12.99m,
              OriginalPrice = 12.99m,
              ProductTypeId = 1,
              ProductType = new ProductType { Id = 1, Name = "Product Type 1" }
            }
          }
        }
      };

      Services.AddMockHttpClient();
      _productServiceMock.Setup(x => x.GetProducts());
      _productServiceMock.SetupGet(x => x.Products).Returns(productList);

      // Act.
      IRenderedComponent<FeaturedProducts> cut =
          RenderComponent<FeaturedProducts>();

      if (cut.Instance.ProductService != null)
      {
        cut.InvokeAsync(async () =>
        {
          if (cut.Instance.ProductService != null)
          {
            await cut.Instance.ProductService.GetProducts();
          }
        });
      }

      // Assert.
      using (new AssertionScope())
      {
        cut.FindAll("[data-testid='list-item']").Should().HaveCount(0);
      }
    }

    [Fact]
    public void When_ProductList_Product_With_No_Variant()
    {
      // Arrange.
      IEnumerable<Product> productList = new List<Product>
      {
        new Product
        {
          Id = 1,
          Title = "Product Title",
          ImageUrl = "Image Url",
          Description = "Product Description",
          Featured = true,
          Variants = new List<ProductVariant>()
        }
      };

      Services.AddMockHttpClient();
      _productServiceMock.Setup(x => x.GetProducts());
      _productServiceMock.SetupGet(x => x.Products).Returns(productList);

      // Act.
      IRenderedComponent<FeaturedProducts> cut =
          RenderComponent<FeaturedProducts>();

      if (cut.Instance.ProductService != null)
      {
        cut.InvokeAsync(async () =>
        {
          if (cut.Instance.ProductService != null)
          {
            await cut.Instance.ProductService.GetProducts();
          }
        });
      }

      // Assert.
      using (new AssertionScope())
      {
        cut.FindAll("[data-testid='featured-product-item']").Should().HaveCount(1);

        cut.Find(".featured-product:first-child [data-testid='featured-product-img-link']").Should().NotBeNull();
        cut.Find(".featured-product:first-child [data-testid='featured-product-img-link']").GetAttribute("href").Should().Be("/product/1");
        var image = cut.Find(".featured-product:first-child [data-testid='featured-product-img-link'] img");
        image.Should().NotBeNull();
        image.Attributes["src"]?.Value.Should().Be("Image Url");
        image.Attributes["alt"]?.Value.Should().Be("Product Title");

        cut.Find(".featured-product:first-child [data-testid='featured-product-title']").Should().NotBeNull();
        cut.Find(".featured-product:first-child [data-testid='featured-product-title']").GetAttribute("href").Should().Be("/product/1");
        cut.Find(".featured-product:first-child [data-testid='featured-product-title']").TextContent.Should().Be("Product Title");
        Action action = () => cut.Find(".featured-product:first-child [data-testid='featured-product-price']");
        action.Should().Throw<ElementNotFoundException>();
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
