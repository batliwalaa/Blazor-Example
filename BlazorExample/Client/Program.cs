global using BlazorExample.Shared;
using Microsoft.AspNetCore.Components.Authorization;
using BlazorExample.Client;
using BlazorExample.Client.Services.Authentication;
using BlazorExample.Client.Services.Category;
using BlazorExample.Client.Services.Product;
using Fluxor;
using Fluxor.Blazor.Persistence;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");


builder.Services
  .AddFluxor(o =>
  {
    o.ScanAssemblies(typeof(Program).Assembly)
    .UseRouting()
    .UsePersistence(o => o.PersistenceType = PersistenceType.SessionStorage)
    .UseReduxDevTools();
  });
builder.Services.AddScoped(sp =>
  new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddOptions();
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();

await builder.Build().RunAsync();
