namespace BlazorExample.Server.Services.Category;

public class CategoryService : ICategoryService
{
  private readonly ApplicationDbContext _context;

  public CategoryService(ApplicationDbContext context)
  {
    _context = context;
  }

  public async Task<Result<IEnumerable<Shared.Category>>> GetCategoriesAsync()
  {
    var categories = await _context.Categories.ToListAsync();

    return new Result<IEnumerable<Shared.Category>>() { Data = categories };
  }
}
