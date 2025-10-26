using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace RazorPlus.Docs.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    [BindProperty]
    public DemoInput InputModel { get; set; } = new();
    private const int DefaultPageSize = 8;

    public IEnumerable<SelectListItem> Roles { get; private set; } = Array.Empty<SelectListItem>();
    public IEnumerable<SelectListItem> AccessLevels { get; private set; } = Array.Empty<SelectListItem>();
    public IEnumerable<Customer> PagedCustomers { get; private set; } = Array.Empty<Customer>();
    public PagerModel Pager { get; private set; } = PagerModel.Empty;
    public string Sort { get; private set; } = "name";
    public string Direction { get; private set; } = "asc";

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

    public void OnGet(int? page, string? sort, string? dir)
    {
        Prepare(page, sort, dir);
    }

    public IActionResult OnPost(int? page, string? sort, string? dir)
    {
        Prepare(page, sort, dir);
        if (!ModelState.IsValid)
        {
            return Page();
        }
        TempData["ok"] = $"Saved {InputModel.Name} as {InputModel.Role} ({InputModel.AccessLevel})";
        return RedirectToPage(new { page = Pager.Page, sort = Sort, dir = Direction });
    }

    public JsonResult OnGetRoleSearch(string q)
    {
        var term = q?.Trim().ToLowerInvariant() ?? string.Empty;
        var roles = GetRoles()
            .Where(r => string.IsNullOrEmpty(term) || (r.Text?.ToLowerInvariant().Contains(term) ?? false))
            .Select(r => new { value = r.Value, text = r.Text })
            .ToList();
        return new JsonResult(roles);
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

    private void Prepare(int? page, string? sort, string? dir)
    {
        Roles = GetRoles();
        AccessLevels = GetAccessLevels();
        Sort = string.IsNullOrWhiteSpace(sort) ? "name" : sort;
        Direction = string.Equals(dir, "desc", StringComparison.OrdinalIgnoreCase) ? "desc" : "asc";

        var customers = SortCustomers(GetCustomers(), Sort, Direction).ToList();

        var totalItems = customers.Count;
        var currentPage = Math.Max(1, page ?? 1);
        var skip = (currentPage - 1) * DefaultPageSize;
        PagedCustomers = customers.Skip(skip).Take(DefaultPageSize).ToList();
        Pager = new PagerModel(totalItems, currentPage, DefaultPageSize, Sort, Direction);

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

    private static IEnumerable<Customer> GetCustomers() => new[]
    {
        new Customer("Ada Lovelace","ada@example.com","Analytical Engines", new DateTime(2019, 6, 1), 128_000m),
        new Customer("Alan Turing","alan@example.com","Enigma Labs", new DateTime(2020, 3, 14), 164_500m),
        new Customer("Grace Hopper","grace@example.com","Compiler Co", new DateTime(2018, 11, 9), 210_750m),
        new Customer("Margaret Hamilton","margaret@example.com","Apollo Systems", new DateTime(2017, 7, 20), 305_120m),
        new Customer("Donald Knuth","donald@example.com","TeXworks", new DateTime(2016, 4, 12), 98_330m),
        new Customer("Barbara Liskov","barbara@example.com","Distributed Inc", new DateTime(2021, 2, 25), 142_980m),
        new Customer("Edsger Dijkstra","edsger@example.com","ShortestPath Ltd", new DateTime(2015, 9, 5), 75_430m),
        new Customer("Radia Perlman","radia@example.com","Bridge Networks", new DateTime(2022, 1, 16), 63_200m),
        new Customer("Karen Sparck Jones","karen@example.com","IR Labs", new DateTime(2019, 9, 9), 54_600m),
        new Customer("Katherine Johnson","katherine@example.com","Orbital Dynamics", new DateTime(2018, 12, 1), 184_930m),
        new Customer("Dennis Ritchie","dennis@example.com","Unix Labs", new DateTime(2014, 5, 17), 125_400m),
        new Customer("Guido van Rossum","guido@example.com","Pythonic LLC", new DateTime(2021, 8, 21), 88_120m),
        new Customer("Ruth Teitelbaum","ruth@example.com","ENIAC Solutions", new DateTime(2013, 3, 8), 47_210m),
        new Customer("Jean Bartik","jean@example.com","Switchboard Tech", new DateTime(2014, 10, 15), 59_480m),
        new Customer("Mary Allen Wilkes","mary@example.com","LINC Systems", new DateTime(2017, 1, 31), 61_540m)
    };

    private static IEnumerable<Customer> SortCustomers(IEnumerable<Customer> customers, string sort, string direction)
    {
        var ascending = !string.Equals(direction, "desc", StringComparison.OrdinalIgnoreCase);
        return sort.ToLowerInvariant() switch
        {
            "email" => ascending ? customers.OrderBy(c => c.Email) : customers.OrderByDescending(c => c.Email),
            "company" => ascending ? customers.OrderBy(c => c.Company) : customers.OrderByDescending(c => c.Company),
            "joined" => ascending ? customers.OrderBy(c => c.Joined) : customers.OrderByDescending(c => c.Joined),
            "lifetimevalue" => ascending ? customers.OrderBy(c => c.LifetimeValue) : customers.OrderByDescending(c => c.LifetimeValue),
            _ => ascending ? customers.OrderBy(c => c.Name) : customers.OrderByDescending(c => c.Name)
        };
    }
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

public record Customer(string Name, string Email, string Company, DateTime Joined, decimal LifetimeValue);

public record PagerModel(int TotalItems, int Page, int PageSize, string Sort, string Direction)
{
    public static PagerModel Empty { get; } = new PagerModel(0, 1, 1, "name", "asc");
    public int TotalPages => (int)Math.Ceiling(TotalItems / (double)Math.Max(1, PageSize));
}

