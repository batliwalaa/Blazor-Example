using BlazorExample.Client.Services;
using BlazorExample.Client.Services.Product;
using BlazorExample.Client.Shared;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.AspNetCore.Components;
using Moq;
using System.Collections.Generic;
using System.Net;

namespace BlazorExample.Client.Tests.Shared;

public class SearchRazorTests : TestContext
{
  private readonly Mock<IProductService> _productServiceMock;

  public SearchRazorTests()
  {
    _productServiceMock = new Mock<IProductService>();
    Services.AddSingleton(_productServiceMock.Object);
    Services.AddSingleton<FakeNavigationManager>();
    Services.AddSingleton<NavigationManager>(s => s.GetRequiredService<FakeNavigationManager>());
  }

  [Fact]
  public void When_Initialised()
  {
    // Arrange.
    // Act.
    IRenderedComponent<Search> cut = RenderComponent<Search>();

    using (new AssertionScope())
    {
      cut.Find("[data-testid='search-input']").Should().NotBeNull();
      cut.Find("[data-testid='search-datalist']").Should().NotBeNull();
      cut.Find("[data-testid='search-button']").Should().NotBeNull();
    }
  }

  [Fact]
  public void When_SearchProducts_Button_Clicked()
  {
    // Arrange.
    IRenderedComponent<Search> cut = RenderComponent<Search>();
    var searchButton = cut.Find("[data-testid='search-button']");
    var navigationManager = Services.GetRequiredService<NavigationManager>();
    cut.Find("[data-testid='search-input']").Input("sci");

    // Act.
    searchButton.Click();

    // Assert.
    using (new AssertionScope())
    {
      navigationManager.Uri.Should().Contain("/search/sci");
    }
  }

  [Fact]
  public void When_SearchInput_Keyup_Event_Fired_With_Key_Enter()
  {
    IRenderedComponent<Search> cut = RenderComponent<Search>();
    var navigationManager = Services.GetRequiredService<NavigationManager>();
    cut.Find("[data-testid='search-input']").Input("sci");

    // Act.
    cut.Find("[data-testid='search-input']").KeyUp("Enter");

    // Assert.
    using (new AssertionScope())
    {
      navigationManager.Uri.Should().Contain("/search/sci");
    }
  }

  [Fact]
  public void When_SearchInput_Keyup_Event_Fired_And_Suggestions_Found_Should_Display_Suggestions()
  {
    // Arrange.
    ResponseResult<IEnumerable<string>> response = new(HttpStatusCode.OK)
    {
      Data = new List<string> { "Science", "Scientist" }
    };
    IRenderedComponent<Search> cut = RenderComponent<Search>();
    var navigationManager = Services.GetRequiredService<NavigationManager>();
    cut.Find("[data-testid='search-input']").Input("sc");
    _productServiceMock.Setup(x => x.SearchSuggestions(It.IsAny<string>())).ReturnsAsync(response);

    // Act.
    cut.Find("[data-testid='search-input']").KeyUp("i");

    // Assert.
    using (new AssertionScope())
    {
      _productServiceMock.Verify(x => x.SearchSuggestions(It.IsAny<string>()), Times.Once);
      cut.FindAll("[data-testid='search-datalist'] option").Count.Should().Be(2);
    }
  }

  [Fact]
  public void When_SearchInput_Keyup_Event_Fired_And_Suggestions_NotFound_Should_Not_Display_Suggestions()
  {
    // Arrange.
    ResponseResult<IEnumerable<string>> response = new(HttpStatusCode.NotFound)
    {
      Success = false
    };
    IRenderedComponent<Search> cut = RenderComponent<Search>();
    var navigationManager = Services.GetRequiredService<NavigationManager>();
    cut.Find("[data-testid='search-input']").Input("sc");
    _productServiceMock.Setup(x => x.SearchSuggestions(It.IsAny<string>())).ReturnsAsync(response);

    // Act.
    cut.Find("[data-testid='search-input']").KeyUp("i");

    // Assert.
    using (new AssertionScope())
    {
      _productServiceMock.Verify(x => x.SearchSuggestions(It.IsAny<string>()), Times.Once);
      cut.FindAll("[data-testid='search-datalist'] option").Count.Should().Be(0);
    }
  }
}
