namespace BlazorExample.Client.Services.Category;

public interface ICategoryService
{
  Task<ResponseResult<IEnumerable<BlazorExample.Shared.Category>>> GetCategories();
}
