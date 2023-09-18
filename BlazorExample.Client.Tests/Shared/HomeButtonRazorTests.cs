using BlazorExample.Client.Shared;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.AspNetCore.Components;
using System.Linq;

namespace BlazorExample.Client.Tests.Shared;

public class HomeButtonRazorTests : TestContext
{
  public HomeButtonRazorTests()
  {
    Services.AddSingleton<FakeNavigationManager>();
    Services.AddSingleton<NavigationManager>(s => s.GetRequiredService<FakeNavigationManager>());
  }

  [Fact]
  public void When_Initialized()
  {
    // Arrange.
    FakeNavigationManager navigationManager = Services.GetRequiredService<FakeNavigationManager>();
    int currentHistoryCount = navigationManager.History.Count;

    // Act.
    IRenderedComponent<HomeButton> cut = RenderComponent<HomeButton>();

    // Assert.
    using (new AssertionScope())
    {
      cut.Instance.NavigationManager.Should().NotBeNull();
      navigationManager.History.Count.Should().Be(currentHistoryCount);
      cut.Find("button").Should().NotBeNull();
    }
  }

  [Fact]
  public void When_HomeButton_Clicked()
  {
    // Arrange.
    FakeNavigationManager navigationManager = Services.GetRequiredService<FakeNavigationManager>();
    int currentHistoryCount = navigationManager.History.Count;
    IRenderedComponent<HomeButton> cut = RenderComponent<HomeButton>();

    // Act.
    cut.Find("button").Click();

    // Assert.
    using (new AssertionScope())
    {
      navigationManager.History.Count.Should().Be(currentHistoryCount + 1);
      navigationManager.History.Last().State.Should().Be(NavigationState.Succeeded);
    }
  }
}
