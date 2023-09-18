global using BlazorExample.Shared;
using Blazored.LocalStorage;
using BlazorExample.Client;
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
    .UseFluxorPersistence(options => options.PersistenceType = PersistenceType.LocalStorage)
    .UseReduxDevTools();
  });
builder.Services.AddScoped(sp =>
  new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IProductService, ProductService>();

await builder.Build().RunAsync();
