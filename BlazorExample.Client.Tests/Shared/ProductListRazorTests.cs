using BlazorExample.Client.Services;
using BlazorExample.Client.Services.Product;
using BlazorExample.Client.Shared;
using BlazorExample.Shared;
using FluentAssertions;
using FluentAssertions.Execution;
using Moq;
using System.Collections.Generic;
using System.Net;

namespace BlazorExample.Client.Tests.Shared;
public class ProductListRazorTests : TestContext
{
  private readonly Mock<IProductService> _productServiceMock;

  public ProductListRazorTests()
  {
    _productServiceMock = new Mock<IProductService>();
    Services.AddSingleton(_productServiceMock.Object);
  }

  public class Initialized : ProductListRazorTests
  {

    [Fact]
    public void When_Initialized()
    {
      // Arrange.
      Services.AddMockHttpClient();

      // Act.
      IRenderedComponent<ProductList> cut =
          RenderComponent<ProductList>();

      // Assert.
      using (new AssertionScope())
      {
        cut.Instance.ProductService?.Should().NotBeNull();
      }
    }
  }

  public class GetProductsByCategory_EmptyOrNull : ProductListRazorTests
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

  public class GetProductsByCategory : ProductListRazorTests
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
          Featured = false,
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
      _productServiceMock.Setup(x => x.GetProductsByCategory(It.IsAny<string>()));
      _productServiceMock.SetupGet(x => x.Products).Returns(productList);

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
        cut.FindAll("[data-testid='list-item']").Should().HaveCount(1);

        cut.Find("li:first-child [data-testid='list-item-detail-img-link']").Should().NotBeNull();
        cut.Find("li:first-child [data-testid='list-item-detail-img-link']").GetAttribute("href").Should().Be("/product/1");
        var image = cut.Find("li:first-child [data-testid='list-item-image']");
        image.Should().NotBeNull();
        image.Attributes["src"]?.Value.Should().Be("Image Url");
        image.Attributes["alt"]?.Value.Should().Be("Product Title");

        cut.Find("li:first-child [data-testid='list-item-detail-title-link']").Should().NotBeNull();
        cut.Find("li:first-child [data-testid='list-item-detail-title-link']").GetAttribute("href").Should().Be("/product/1");
        cut.Find("li:first-child [data-testid='list-item-title']").TextContent.Should().Be("Product Title");
        cut.Find("li:first-child [data-testid='list-item-description']").TextContent.Should().Be("Product Description");
        cut.Find("li:first-child [data-testid='list-item-price']").TextContent.Should().Contain("Starting at $2.99");
      }
    }

    [Fact]
    public void When_ProductList_Product_With_Single_Variant()
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
              ProductTypeId = 2,
              ProductType = new ProductType { Id = 1, Name = "Product Type 1" }
            }
          }
        }
      };

      Services.AddMockHttpClient();
      _productServiceMock.Setup(x => x.GetProductsByCategory(It.IsAny<string>()));
      _productServiceMock.SetupGet(x => x.Products).Returns(productList);

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
        cut.FindAll("[data-testid='list-item']").Should().HaveCount(1);

        cut.Find("li:first-child [data-testid='list-item-detail-img-link']").Should().NotBeNull();
        cut.Find("li:first-child [data-testid='list-item-detail-img-link']").GetAttribute("href").Should().Be("/product/1");
        var image = cut.Find("li:first-child [data-testid='list-item-image']");
        image.Should().NotBeNull();
        image.Attributes["src"]?.Value.Should().Be("Image Url");
        image.Attributes["alt"]?.Value.Should().Be("Product Title");

        cut.Find("li:first-child [data-testid='list-item-detail-title-link']").Should().NotBeNull();
        cut.Find("li:first-child [data-testid='list-item-detail-title-link']").GetAttribute("href").Should().Be("/product/1");
        cut.Find("li:first-child [data-testid='list-item-title']").TextContent.Should().Be("Product Title");
        cut.Find("li:first-child [data-testid='list-item-description']").TextContent.Should().Be("Product Description");
        cut.Find("li:first-child [data-testid='list-item-price']").TextContent.Should().Contain("$12.99");
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
          Variants = new List<ProductVariant>()
        }
      };

      Services.AddMockHttpClient();
      _productServiceMock.Setup(x => x.GetProducts());
      _productServiceMock.SetupGet(x => x.Products).Returns(productList);

      // Act.
      IRenderedComponent<ProductList> cut =
          RenderComponent<ProductList>();

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
        cut.FindAll("[data-testid='list-item']").Should().HaveCount(1);

        cut.Find("li:first-child [data-testid='list-item-detail-img-link']").Should().NotBeNull();
        cut.Find("li:first-child [data-testid='list-item-detail-img-link']").GetAttribute("href").Should().Be("/product/1");
        var image = cut.Find("li:first-child [data-testid='list-item-image']");
        image.Should().NotBeNull();
        image.Attributes["src"]?.Value.Should().Be("Image Url");
        image.Attributes["alt"]?.Value.Should().Be("Product Title");

        cut.Find("li:first-child [data-testid='list-item-detail-title-link']").Should().NotBeNull();
        cut.Find("li:first-child [data-testid='list-item-detail-title-link']").GetAttribute("href").Should().Be("/product/1");
        cut.Find("li:first-child [data-testid='list-item-title']").TextContent.Should().Be("Product Title");
        cut.Find("li:first-child [data-testid='list-item-description']").TextContent.Should().Be("Product Description");
        cut.Find("li:first-child [data-testid='list-item-price']").TextContent.Should().Be("");
      }
    }
  }

  public class SearchProducts_EmptyOrNull : ProductListRazorTests
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

      _productServiceMock.Setup(x => x.SearchProducts(It.IsAny<string>(), It.IsAny<int>()));
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
            await cut.Instance.ProductService.SearchProducts("sci", 1);

          }
        });
      }

      // Assert.
      using (new AssertionScope())
      {
        cut.Find("[data-testid='list-message']").TextContent.Should().Be(expectedMessage);
        cut.FindAll("[data-testid='btn-pageno-*']").Count.Should().Be(0);
      }
    }
  }

  public class SearchProducts : ProductListRazorTests
  {
    [Fact]
    public void When_ProductList_Has_Pages()
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
              ProductTypeId = 2,
              ProductType = new ProductType { Id = 1, Name = "Product Type 1" }
            }
          }
        }
      };

      Services.AddMockHttpClient();
      _productServiceMock.Setup(x => x.SearchProducts(It.IsAny<string>(), It.IsAny<int>()));
      _productServiceMock.SetupGet(x => x.Products).Returns(productList);
      _productServiceMock.SetupGet(x => x.PageCount).Returns(5);
      _productServiceMock.SetupGet(x => x.CurrentPage).Returns(1);

      // Act.
      IRenderedComponent<ProductList> cut =
          RenderComponent<ProductList>();

      if (cut.Instance.ProductService != null)
      {
        cut.InvokeAsync(async () =>
        {
          if (cut.Instance.ProductService != null)
          {
            await cut.Instance.ProductService.SearchProducts("sci", 1);
          }
        });
      }

      // Assert.
      using (new AssertionScope())
      {
        cut.FindAll("[data-testid*='btn-pageno-']").Count.Should().Be(5);
        cut.Find("[data-testid='btn-pageno-1']").ClassName?.Contains("btn-info").Should().BeTrue();
        cut.Find("[data-testid='btn-pageno-1']").ClassName?.Contains("btn-outline-info").Should().BeFalse();
        cut.Find("[data-testid='btn-pageno-2']").ClassName?.Contains("btn-outline-info").Should().BeTrue();
        cut.Find("[data-testid='btn-pageno-3']").ClassName?.Contains("btn-outline-info").Should().BeTrue();
        cut.Find("[data-testid='btn-pageno-4']").ClassName?.Contains("btn-outline-info").Should().BeTrue();
        cut.Find("[data-testid='btn-pageno-5']").ClassName?.Contains("btn-outline-info").Should().BeTrue();
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
