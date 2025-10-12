using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace RazorPlus.Docs.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    [BindProperty]
    public DemoInput InputModel { get; set; } = new();
    public IEnumerable<SelectListItem> Roles { get; private set; } = Array.Empty<SelectListItem>();
    public IEnumerable<SelectListItem> AccessLevels { get; private set; } = Array.Empty<SelectListItem>();
    public IEnumerable<Customer> Customers { get; private set; } = Array.Empty<Customer>();
    public object ChartData => new
    {
        xAxis = new { type = "category", boundaryGap = false, data = new[] { "Jan", "Feb", "Mar", "Apr", "May", "Jun" } },
        yAxis = new { type = "value" },
        series = new[]
        {
            new { name = "Revenue", type = "line", smooth = true, data = new[] { 120, 200, 180, 220, 260, 300 } }
        }
    };

    public object ChartOptions => new
    {
        tooltip = new { trigger = "axis" },
        legend = new { data = new[] { "Revenue" } },
        grid = new { left = "3%", right = "4%", bottom = "3%", containLabel = true }
    };

    public IndexModel(ILogger<IndexModel> logger)
    {
        _logger = logger;
    }

    public void OnGet()
    {
        Roles = GetRoles();
        AccessLevels = GetAccessLevels();
        Customers = GetCustomers();
        if (string.IsNullOrEmpty(InputModel.Role))
        {
            InputModel.Role = Roles.First().Value;
        }
        if (string.IsNullOrEmpty(InputModel.AccessLevel))
        {
            InputModel.AccessLevel = AccessLevels.First().Value;
        }
        if (string.IsNullOrWhiteSpace(InputModel.Bio))
        {
            InputModel.Bio = "Lead developer building accessible experiences.";
        }
    }

    public IActionResult OnPost()
    {
        Roles = GetRoles();
        AccessLevels = GetAccessLevels();
        Customers = GetCustomers();
        if (!ModelState.IsValid)
        {
            return Page();
        }
        TempData["ok"] = $"Saved {InputModel.Name} as {InputModel.Role} ({InputModel.AccessLevel})";
        return RedirectToPage();
    }

    private static IEnumerable<SelectListItem> GetRoles() => new[]
    {
        new SelectListItem("Admin","admin"),
        new SelectListItem("Editor","editor"),
        new SelectListItem("Viewer","viewer"),
    };

    private static IEnumerable<SelectListItem> GetAccessLevels() => new[]
    {
        new SelectListItem("Owner","owner"),
        new SelectListItem("Maintainer","maintainer"),
        new SelectListItem("Member","member"),
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
    [Required]
    [StringLength(280, MinimumLength = 10)]
    public string? Bio { get; set; }
    [Required]
    public string? AccessLevel { get; set; }
    [Display(Name = "Accept terms")]
    [Range(typeof(bool), "true", "true", ErrorMessage = "Please accept the terms.")]
    public bool AcceptTerms { get; set; }
}

public record Customer(string Name, string Email);
