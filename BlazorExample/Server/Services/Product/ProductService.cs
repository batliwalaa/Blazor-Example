namespace BlazorExample.Server.Services.Product;

public class ProductService : IProductService
{
  private readonly ApplicationDbContext _context;

  public ProductService(ApplicationDbContext context)
  {
    _context = context;
  }

  public async Task<Result<IEnumerable<Shared.Product>>> GetProductsAsync()
  {
    Result<IEnumerable<Shared.Product>> result = new()
    {
      Data = await _context.Products.Include(x => x.Variants).ToListAsync(),
    };

    if (!result.Data.Any())
    {
      result.Success = false;
      result.Message = "Sorry, no products found.";
    }

    return result;
  }

  public async Task<Result<Shared.Product>> GetProductByIdAsync(int id)
  {
    Result<Shared.Product> result = new()
    {
      Data = await _context.Products
        .Include(x => x.Variants)
        .ThenInclude(x => x.ProductType)
        .FirstOrDefaultAsync(x => x.Id == id)
    };

    if (result.Data == null)
    {
      result.Success = false;
      result.Message = "Sorry, but this product does not exist";
    }

    return result;
  }

  public async Task<Result<IEnumerable<Shared.Product>>> GetProductsByCategoryAsync(string categoryUrl)
  {
    Result<IEnumerable<Shared.Product>> result = new()
    {
      Data = await _context.Products
        .Where(p => p.Category != null && p.Category.Url.ToLower() == categoryUrl.ToLower())
        .Include(x => x.Variants)
        .ToListAsync(),
    };

    if (!result.Data.Any())
    {
      result.Success = false;
      result.Message = "Sorry, no products found for category.";
    }

    return result;
  }

  public async Task<Result<IEnumerable<Shared.Product>>> Search(string searchTerm, int page)
  {
    IQueryable<Shared.Product> query = _context.Products
      .Where(p =>
        p.Title.ToLower().Contains(searchTerm)
        ||
        p.Description.ToLower().Contains(searchTerm)
      );
    double pageSize = 2;
    int rowCount = await query.CountAsync();
    int pageCount = (int)Math.Ceiling(rowCount / pageSize);

    Result<IEnumerable<Shared.Product>> result = new()
    {
      Data = await query.Include(x => x.Variants)
        .Skip((page - 1) * (int)pageSize)
        .Take((int)pageSize)
        .ToListAsync(),
      CurrentPage = page,
      Pages = pageCount
    };

    return result;
  }

  public async Task<Result<IEnumerable<string>>> GetProductSearchSuggestions(string searchTerm)
  {
    IEnumerable<Shared.Product> products = await FindProducts(searchTerm);

    List<string> result = new();

    foreach (var product in products)
    {
      if (product.Title.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
      {
        result.Add(product.Title);
      }

      if (!string.IsNullOrWhiteSpace(product.Description))
      {
        var punctuation = product.Description.Where(char.IsPunctuation).Distinct().ToArray();
        var words = product.Description.Split().Select(s => s.Trim(punctuation));

        foreach (var word in words)
        {
          if (word.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) && !result.Contains(word))
          {
            result.Add(word);
          }
        }
      }
    }

    return new Result<IEnumerable<string>> { Data = result };
  }

  public async Task<Result<IEnumerable<Shared.Product>>> GetFeaturedProducts()
  {
    Result<IEnumerable<Shared.Product>> result = new()
    {
      Data = await _context.Products
        .Where(p => p.Featured)
        .Include(x => x.Variants)
        .ToListAsync(),
    };

    if (!result.Data.Any())
    {
      result.Success = false;
      result.Message = "Sorry, no featured products found.";
    }

    return result;
  }

  private async Task<IEnumerable<Shared.Product>> FindProducts(string searchTerm)
  {
    return await _context.Products
        .Where(p =>
          p.Title.ToLower().Contains(searchTerm)
          ||
          p.Description.ToLower().Contains(searchTerm)
        ).Include(x => x.Variants)
        .ToListAsync();
  }
}
