namespace BlazorExample.Client.Services.Category;

public class CategoryService : ServiceBase, ICategoryService
{
  public CategoryService(HttpClient http) : base(http)
  {
  }

  public async Task<ResponseResult<IEnumerable<BlazorExample.Shared.Category>>> GetCategories()
  {
    return await GetAsync<IEnumerable<BlazorExample.Shared.Category>>("api/category");
  }
}
