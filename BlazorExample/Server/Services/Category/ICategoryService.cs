namespace BlazorExample.Server.Services.Category;

public interface ICategoryService
{
  Task<Result<IEnumerable<Shared.Category>>> GetCategoriesAsync();
}
