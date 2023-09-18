using BlazorExample.Client.Services;
using BlazorExample.Client.Services.Product;
using BlazorExample.Shared;
using FluentAssertions;
using FluentAssertions.Execution;
using RichardSzalay.MockHttp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace BlazorExample.Client.Tests.Services;

public class ProductServiceTests
{
  private readonly HttpClient MockkHttpClient;
  private readonly MockHttpMessageHandler MockHttpMessageHandler;

  public ProductServiceTests()
  {
    MockHttpMessageHandler = new MockHttpMessageHandler();
    MockkHttpClient = MockHttpMessageHandler.ToHttpClient();
    MockkHttpClient.BaseAddress = new Uri("http://localhost");
  }

  public class GetProducts : ProductServiceTests
  {
    [Fact]
    public async Task NotFound_Test()
    {
      // Arrange.
      int productsChangedEventTriggered = 0;
      Action productsChangedAction = () =>
      {
        productsChangedEventTriggered++;
      };

      IProductService sut = new ProductService(MockkHttpClient);
      MockHttpMessageHandler
        .When(HttpMethod.Get, "/api/product/featured")
        .RespondJson(new Result<Product>
        {
          Success = false,
          Message = "Not Found"
        }, HttpStatusCode.NotFound);
      sut.ProductsChanged += productsChangedAction;

      // Act.
      await sut.GetProducts();

      //Assert.
      using (new AssertionScope())
      {
        sut.Products.Should().BeNullOrEmpty();
        sut.Message.Should().Be("Not Found");
        productsChangedEventTriggered.Should().Be(1);
      }

      sut.ProductsChanged -= productsChangedAction;
    }

    [Fact]
    public async Task Ok_Test()
    {
      // Arrange.
      int productsChangedEventTriggered = 0;
      Action productsChangedAction = () =>
      {
        productsChangedEventTriggered++;
      };

      IProductService sut = new ProductService(MockkHttpClient);
      MockHttpMessageHandler
        .When(HttpMethod.Get, "/api/product/featured")
        .RespondJson(new Result<IEnumerable<Product>>
        {
          Data = new List<Product> { new Product { } },
        }, HttpStatusCode.OK);
      sut.ProductsChanged += productsChangedAction;

      // Act.
      await sut.GetProducts();

      //Assert.
      using (new AssertionScope())
      {
        sut.Products.Should().BeOfType<List<Product>>();
        sut.Products.Should().NotBeNullOrEmpty();
        sut.Products?.Count().Should().Be(1);
        productsChangedEventTriggered.Should().Be(1);
      }

      sut.ProductsChanged -= productsChangedAction;
    }
  }

  public class GetProductsByCategory : ProductServiceTests
  {
    [Fact]
    public async Task NotFound_Test()
    {
      // Arrange.
      int productsChangedEventTriggered = 0;
      Action productsChangedAction = () =>
      {
        productsChangedEventTriggered++;
      };

      IProductService sut = new ProductService(MockkHttpClient);
      MockHttpMessageHandler
        .When(HttpMethod.Get, "/api/product/category/video-games")
        .RespondJson(new Result<Product>
        {
          Success = false,
          Message = "Not Found"
        }, HttpStatusCode.NotFound);
      sut.ProductsChanged += productsChangedAction;

      // Act.
      await sut.GetProductsByCategory("video-games");

      //Assert.
      using (new AssertionScope())
      {
        sut.Products.Should().BeNullOrEmpty();
        sut.Message.Should().Be("Not Found");
        productsChangedEventTriggered.Should().Be(1);
      }

      sut.ProductsChanged -= productsChangedAction;
    }

    [Fact]
    public async Task Ok_Test()
    {
      // Arrange.
      int productsChangedEventTriggered = 0;
      Action productsChangedAction = () =>
      {
        productsChangedEventTriggered++;
      };

      IProductService sut = new ProductService(MockkHttpClient);
      MockHttpMessageHandler
        .When(HttpMethod.Get, "/api/product/category/video-games")
        .RespondJson(new Result<IEnumerable<Product>>
        {
          Data = new List<Product> { new Product { } },
        }, HttpStatusCode.OK);
      sut.ProductsChanged += productsChangedAction;

      // Act.
      await sut.GetProductsByCategory("video-games");

      //Assert.
      using (new AssertionScope())
      {
        sut.Products.Should().BeOfType<List<Product>>();
        sut.Products.Should().NotBeNullOrEmpty();
        sut.Products?.Count().Should().Be(1);
        productsChangedEventTriggered.Should().Be(1);
      }

      sut.ProductsChanged -= productsChangedAction;
    }
  }

  public class GetProduct : ProductServiceTests
  {
    [Fact]
    public async Task NotFound_Test()
    {
      // Arrange.
      IProductService sut = new ProductService(MockkHttpClient);
      MockHttpMessageHandler
        .When(HttpMethod.Get, "/api/product/1")
        .RespondJson(new Result<Product>
        {
          Success = false,
          Message = "Not Found"
        }, HttpStatusCode.NotFound);

      // Act.
      var response = await sut.GetProduct(1);

      //Assert.
      using (new AssertionScope())
      {
        response.Success.Should().BeFalse();
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        response.Should().BeOfType<ResponseResult<Product>>();
        response.Message.Should().Be("Not Found");
      }
    }

    [Fact]
    public async Task Ok_Test()
    {
      // Arrange.
      IProductService sut = new ProductService(MockkHttpClient);
      MockHttpMessageHandler
        .When(HttpMethod.Get, "/api/product/1")
        .RespondJson(new Result<Product>
        {
          Data = new Product { }
        }, HttpStatusCode.OK);

      // Act.
      var response = await sut.GetProduct(1);

      //Assert.
      using (new AssertionScope())
      {
        response.Success.Should().BeTrue();
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Should().BeOfType<ResponseResult<Product>>();
        response.Data.Should().BeOfType<Product>();
      }
    }
  }

  public class GetProductsBySearchText : ProductServiceTests
  {
    [Fact]
    public async Task NotFound_Test()
    {
      // Arrange.
      int productsChangedEventTriggered = 0;
      Action productsChangedAction = () =>
      {
        productsChangedEventTriggered++;
      };

      IProductService sut = new ProductService(MockkHttpClient);
      MockHttpMessageHandler
        .When(HttpMethod.Get, "/api/product/search/ready/1")
        .RespondJson(new Result<Product>
        {
          Success = false,
          Message = "Not Found"
        }, HttpStatusCode.NotFound);
      sut.ProductsChanged += productsChangedAction;

      // Act.
      await sut.SearchProducts("ready", 1);

      //Assert.
      using (new AssertionScope())
      {
        sut.Products.Should().BeNullOrEmpty();
        sut.Message.Should().Be("Not Found");
        productsChangedEventTriggered.Should().Be(1);
      }

      sut.ProductsChanged -= productsChangedAction;
    }

    [Fact]
    public async Task Ok_Test()
    {
      // Arrange.
      int productsChangedEventTriggered = 0;
      Action productsChangedAction = () =>
      {
        productsChangedEventTriggered++;
      };

      IProductService sut = new ProductService(MockkHttpClient);
      MockHttpMessageHandler
        .When(HttpMethod.Get, "/api/product/search/ready/1")
        .RespondJson(new Result<IEnumerable<Product>>
        {
          Data = new List<Product> { new Product { } },
        }, HttpStatusCode.OK);
      sut.ProductsChanged += productsChangedAction;

      // Act.
      await sut.SearchProducts("ready", 1);

      //Assert.
      using (new AssertionScope())
      {
        sut.Products.Should().BeOfType<List<Product>>();
        sut.Products.Should().NotBeNullOrEmpty();
        sut.Products?.Count().Should().Be(1);
        productsChangedEventTriggered.Should().Be(1);
      }

      sut.ProductsChanged -= productsChangedAction;
    }
  }

  public class GetSearchSuggestions : ProductServiceTests
  {
    [Fact]
    public async Task NotFound_Test()
    {
      // Arrange.
      IProductService sut = new ProductService(MockkHttpClient);
      MockHttpMessageHandler
        .When(HttpMethod.Get, "/api/product/search-suggestions/ready")
        .RespondJson(new Result<string>
        {
          Success = false,
          Message = "Not Found"
        }, HttpStatusCode.NotFound);

      // Act.
      var response = await sut.SearchSuggestions("ready");

      //Assert.
      using (new AssertionScope())
      {
        response.Data.Should().BeNullOrEmpty();
        response.Message.Should().Be("Not Found");
      }
    }

    [Fact]
    public async Task Ok_Test()
    {
      // Arrange.
      IProductService sut = new ProductService(MockkHttpClient);
      MockHttpMessageHandler
        .When(HttpMethod.Get, "/api/product/search-suggestions/ready")
        .RespondJson(new Result<IEnumerable<string>>
        {
          Data = new List<string> { "Ready", "Ready for test" },
        }, HttpStatusCode.OK);

      // Act.
      var response = await sut.SearchSuggestions("ready");

      //Assert.
      using (new AssertionScope())
      {
        response.Data.Should().BeOfType<List<string>>();
        response.Data.Should().NotBeNullOrEmpty();
        response.Data?.Count().Should().Be(2);
      }
    }
  }
}
