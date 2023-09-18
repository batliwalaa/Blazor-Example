using BlazorExample.Shared;
using FluentAssertions.Execution;
using FluentAssertions;
using RichardSzalay.MockHttp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using BlazorExample.Client.Services.Category;
using BlazorExample.Client.Services;

namespace BlazorExample.Client.Tests.Services;
public class CategoryServiceTests
{
  private readonly HttpClient MockkHttpClient;
  private readonly MockHttpMessageHandler MockHttpMessageHandler;

  public CategoryServiceTests()
  {
    MockHttpMessageHandler = new MockHttpMessageHandler();
    MockkHttpClient = MockHttpMessageHandler.ToHttpClient();
    MockkHttpClient.BaseAddress = new Uri("http://localhost");
  }

  public class GetCategories : CategoryServiceTests
  {
    [Fact]
    public async Task NotFound_Test()
    {
      // Arrange.
      ICategoryService sut = new CategoryService(MockkHttpClient);
      MockHttpMessageHandler
        .When(HttpMethod.Get, "/api/category")
        .RespondJson(new Result<Category>
        {
          Success = false,
          Message = "Not Found"
        }, HttpStatusCode.NotFound);

      // Act.
      ResponseResult<IEnumerable<Category>> result = await sut.GetCategories();

      //Assert.
      using (new AssertionScope())
      {
        result.Data.Should().BeNullOrEmpty();
        result.Message.Should().Be("Not Found");
      }
    }

    [Fact]
    public async Task Ok_Test()
    {
      // Arrange.
      ICategoryService sut = new CategoryService(MockkHttpClient);
      MockHttpMessageHandler
        .When(HttpMethod.Get, "/api/category")
        .RespondJson(new Result<IEnumerable<Category>>
        {
          Data = new List<Category> { new Category { } },
        }, HttpStatusCode.OK);

      // Act.
      ResponseResult<IEnumerable<Category>> result = await sut.GetCategories();

      //Assert.
      using (new AssertionScope())
      {
        result.Data.Should().NotBeNullOrEmpty();
        result.Data.Should().BeOfType<List<Category>>();
        result.Data?.Count().Should().Be(1);
      }
    }
  }
}
