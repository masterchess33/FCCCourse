using AbbyWeb.Data;
using AbbyWeb.Model;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AbbyWeb.Pages.Categories;

public class Index : PageModel
{
    private readonly ApplicationDbContext _db;
    public IEnumerable<Category> Categories { get; set; } = null!;

    public Index(ApplicationDbContext db)
    {
        _db = db;
    }
    
    public void OnGet()
    {
        Categories = _db.Category;
    }
}