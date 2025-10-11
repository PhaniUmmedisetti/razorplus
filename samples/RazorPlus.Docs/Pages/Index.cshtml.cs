using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace RazorPlus.Docs.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    [BindProperty]
    public DemoInput InputModel { get; set; } = new();
    public IEnumerable<SelectListItem> Roles { get; private set; } = Array.Empty<SelectListItem>();
    public IEnumerable<Customer> Customers { get; private set; } = Array.Empty<Customer>();

    public IndexModel(ILogger<IndexModel> logger)
    {
        _logger = logger;
    }

    public void OnGet()
    {
        Roles = GetRoles();
        Customers = GetCustomers();
    }

    public IActionResult OnPost()
    {
        Roles = GetRoles();
        if (!ModelState.IsValid)
        {
            return Page();
        }
        TempData["ok"] = $"Saved {InputModel.Name} as {InputModel.Role}";
        return RedirectToPage();
    }

    private static IEnumerable<SelectListItem> GetRoles() => new[]
    {
        new SelectListItem("Admin","admin"),
        new SelectListItem("Editor","editor"),
        new SelectListItem("Viewer","viewer"),
    };

    private static IEnumerable<Customer> GetCustomers() => new[]
    {
        new Customer("Ada Lovelace","ada@example.com"),
        new Customer("Alan Turing","alan@example.com"),
        new Customer("Grace Hopper","grace@example.com"),
    };
}

public class DemoInput
{
    [Required]
    public string? Name { get; set; }
    [Required]
    public string? Role { get; set; }
}

public record Customer(string Name, string Email);
