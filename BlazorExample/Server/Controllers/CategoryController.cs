using BlazorExample.Server.Services.Category;
using Microsoft.AspNetCore.Mvc;

namespace BlazorExample.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoryController : ControllerBase
{
  private readonly ICategoryService _categoryService;

  public CategoryController(ICategoryService categoryService)
  {
    _categoryService = categoryService;
  }

  [HttpGet]
  public async Task<ActionResult<IEnumerable<Category>>> Get()
  {
    return Ok(await _categoryService.GetCategoriesAsync());
  }
}
