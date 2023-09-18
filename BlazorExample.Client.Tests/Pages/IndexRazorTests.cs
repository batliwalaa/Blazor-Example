using BlazorExample.Client.Services.Product;
using BlazorExample.Client.Shared;
using BlazorExample.Shared;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.AspNetCore.Components.Web;
using Moq;
using System.Collections.Generic;

namespace BlazorExample.Client.Tests.Pages;
public class IndexRazorTests : TestContext
{
  private readonly Mock<IProductService> _productServiceMock;

  public IndexRazorTests()
  {
    _productServiceMock = new Mock<IProductService>();
    Services.AddSingleton(_productServiceMock.Object);
  }

  public class Initialized : IndexRazorTests
  {
    [Fact]
    public void When_Initialized_Without_Category_And_SearchText()
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
          Variants = new List<ProductVariant>(),
          Featured = true
        }
      };

      Services.AddMockHttpClient();
      _productServiceMock.Setup(x => x.GetProducts());
      _productServiceMock.SetupGet(x => x.Products).Returns(productList);

      // Act.
      IRenderedComponent<Client.Pages.Index> cut =
          RenderComponent<Client.Pages.Index>();

      // Assert.
      using (new AssertionScope())
      {
        cut.FindComponent<PageTitle>().Should().NotBeNull();
        cut.FindComponent<FeaturedProducts>().Should().NotBeNull();
        cut.Instance.CategoryUrl.Should().BeNullOrEmpty();
      }
    }

    [Fact]
    public void When_Initialized_With_Category()
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
      IRenderedComponent<Client.Pages.Index> cut =
          RenderComponent<Client.Pages.Index>(parameters => parameters.Add(p => p.CategoryUrl, "video-games"));

      // Assert.
      using (new AssertionScope())
      {
        cut.FindComponent<PageTitle>().Should().NotBeNull();
        cut.FindComponent<ProductList>().Should().NotBeNull();
        cut.Instance.CategoryUrl.Should().Be("video-games");
      }
    }

    [Fact]
    public void When_Initialized_With_SearchText()
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
      _productServiceMock.Setup(x => x.SearchProducts(It.IsAny<string>(), It.IsAny<int>()));
      _productServiceMock.SetupGet(x => x.Products).Returns(productList);

      // Act.
      IRenderedComponent<Client.Pages.Index> cut =
          RenderComponent<Client.Pages.Index>(parameters => parameters.Add(p => p.SearchText, "ready"));

      // Assert.
      using (new AssertionScope())
      {
        cut.FindComponent<PageTitle>().Should().NotBeNull();
        cut.FindComponent<ProductList>().Should().NotBeNull();
        cut.Instance.SearchText.Should().Be("ready");

        _productServiceMock.Verify(x => x.SearchProducts(It.IsAny<string>(), It.IsAny<int>()), Times.Once);
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
