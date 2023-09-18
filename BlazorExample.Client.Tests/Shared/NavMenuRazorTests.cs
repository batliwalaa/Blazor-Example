using BlazorExample.Client.Services;
using BlazorExample.Client.Services.Category;
using BlazorExample.Client.Shared;
using BlazorExample.Shared;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using Moq;
using System;
using System.Collections.Generic;
using System.Net;

namespace BlazorExample.Client.Tests.Shared;
public class NavMenuRazorTests : TestContext
{
  private readonly Mock<ICategoryService> _categoryServiceMock;

  public NavMenuRazorTests()
  {
    _categoryServiceMock = new Mock<ICategoryService>();
    Services.AddSingleton(_categoryServiceMock.Object);
    Services.AddSingleton<FakeNavigationManager>();
    Services.AddSingleton<NavigationManager>(s => s.GetRequiredService<FakeNavigationManager>());
  }

  [Fact]
  public void When_Initialized()
  {
    // Arrange.
    Services.AddMockHttpClient();

    // Act.
    IRenderedComponent<NavMenu> cut = RenderComponent<NavMenu>();

    // Assert.
    using (new AssertionScope())
    {
      cut.Instance.CategoryService?.Should().NotBeNull();
      cut.FindComponents<NavLink>().Should().HaveCount(1);
      cut.Find("[data-testid='nav-link-home']")
          .ClassList
          .Should()
          .Contain("active");
      cut.Find(".collapse").Should().NotBeNull();
    }
  }

  [Fact]
  public void When_Categories_NotFound()
  {
    // Arrange.
    Services.AddMockHttpClient();
    ResponseResult<IEnumerable<Category>> responseResult = new(HttpStatusCode.NotFound);

    _categoryServiceMock.Setup(x => x.GetCategories()).ReturnsAsync(responseResult);

    // Act.
    IRenderedComponent<NavMenu> cut = RenderComponent<NavMenu>();

    // Assert.
    using (new AssertionScope())
    {
      cut.Instance.CategoryService?.Should().NotBeNull();
      cut.FindComponents<NavLink>().Should().HaveCount(1);
      _categoryServiceMock.Verify(x => x.GetCategories(), Times.Once);
    }
  }

  [Fact]
  public void When_Categories_Found()
  {
    // Arrange.
    Services.AddMockHttpClient();
    ResponseResult<IEnumerable<Category>> responseResult = new(HttpStatusCode.OK)
    {
      Data = new List<Category>
      {
        new Category { Id = 1, Name = "Books", Url = "books" },
        new Category { Id = 1, Name = "Video Games", Url = "video-games" },
      }
    };

    _categoryServiceMock.Setup(x => x.GetCategories()).ReturnsAsync(responseResult);

    // Act.
    IRenderedComponent<NavMenu> cut = RenderComponent<NavMenu>();

    // Assert.
    using (new AssertionScope())
    {
      cut.Instance.CategoryService?.Should().NotBeNull();
      cut.FindComponents<NavLink>().Should().HaveCount(3);
      cut.Find("[data-testid='nav-link-books']").GetAttribute("href").Should().Be("books");
      cut.Find("[data-testid='nav-link-video-games']").GetAttribute("href").Should().Be("video-games");
      _categoryServiceMock.Verify(x => x.GetCategories(), Times.Once);
    }
  }

  [Theory]
  [InlineData("Books", "books")]
  [InlineData("Video Games", "video-games")]
  public void Category_NavLink_Is_Marked_Active_When_Viewing_Category(string name, string url)
  {
    // Arrange.
    Services.AddMockHttpClient();
    ResponseResult<IEnumerable<Category>> responseResult = new(HttpStatusCode.OK)
    {
      Data = new List<Category>
      {
        new Category { Id = 1, Name = name, Url = url }
      }
    };
    _categoryServiceMock.Setup(x => x.GetCategories()).ReturnsAsync(responseResult);
    var navigationManager = Services.GetRequiredService<NavigationManager>();

    // Act.
    IRenderedComponent<NavMenu> cut = RenderComponent<NavMenu>();
    navigationManager.NavigateTo(url);

    // Assert.
    using (new AssertionScope())
    {
      cut.Find($"[data-testid='nav-link-{url}']")
          .ClassList
          .Should()
          .Contain("active");
    }
  }

  [Fact]
  public void When_CollapseNavMenu_False_Element_Should_Not_Have_Collapse_Class()
  {
    // Arrange.
    Services.AddMockHttpClient();

    // Act.
    IRenderedComponent<NavMenu> cut = RenderComponent<NavMenu>();
    cut.Find(".navbar-toggler").Click();

    // Assert.
    using (new AssertionScope())
    {
      Action action = () => cut.Find(".collapse");
      action.Should().Throw<ElementNotFoundException>();
    }
  }
}
