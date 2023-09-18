using BlazorExample.Server.Services.Product;
using Microsoft.AspNetCore.Mvc;

namespace BlazorExample.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductController : ControllerBase
{
  private readonly IProductService _productService;

  public ProductController(IProductService productService)
  {
    _productService = productService;
  }

  [HttpGet]
  public async Task<ActionResult<Result<IEnumerable<Product>>>> Get()
  {
    Result<IEnumerable<Product>> result = await _productService.GetProductsAsync();

    if (result.Success)
    {
      return result;
    }

    return new NotFoundObjectResult(result);
  }

  [HttpGet]
  [Route("{id:int}")]
  public async Task<ActionResult<Result<Product>>> GetById(int id)
  {
    Result<Product> result = await _productService.GetProductByIdAsync(id);

    if (result.Success)
    {
      return result;
    }

    return new NotFoundObjectResult(result);
  }

  [HttpGet]
  [Route("category/{categoryUrl}")]
  public async Task<ActionResult<Result<IEnumerable<Product>>>> GetProductsByCategory(string categoryUrl)
  {
    Result<IEnumerable<Product>> result = await _productService.GetProductsByCategoryAsync(categoryUrl);

    if (result.Success)
    {
      return result;
    }

    return new NotFoundObjectResult(result);
  }

  [HttpGet]
  [Route("search/{searchTerm}/{page:int?}")]
  public async Task<ActionResult<Result<IEnumerable<Product>>>> Search(string searchTerm, int page = 1)
  {
    Result<IEnumerable<Product>> result = await _productService.Search(searchTerm, page);

    if (result.Success)
    {
      return result;
    }

    return new NotFoundObjectResult(result);
  }

  [HttpGet]
  [Route("search-suggestions/{searchTerm}")]
  public async Task<ActionResult<Result<IEnumerable<string>>>> SearchSuggestions(string searchTerm)
  {
    Result<IEnumerable<string>> result = await _productService.GetProductSearchSuggestions(searchTerm);

    if (result.Success)
    {
      return result;
    }

    return new NotFoundObjectResult(result);
  }

  [HttpGet]
  [Route("featured")]
  public async Task<ActionResult<Result<IEnumerable<Product>>>> GetFeaturedProducts()
  {
    Result<IEnumerable<Product>> result = await _productService.GetFeaturedProducts();

    if (result.Success)
    {
      return result;
    }

    return new NotFoundObjectResult(result);
  }
}
