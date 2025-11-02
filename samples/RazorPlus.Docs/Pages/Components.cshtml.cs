using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace RazorPlus.Docs.Pages;

public class ComponentsModel : PageModel
{
    [BindProperty]
    public FormDataModel FormData { get; set; } = new();

    public IEnumerable<SelectListItem> Countries { get; private set; } = new List<SelectListItem>();
    public IEnumerable<SelectListItem> Plans { get; private set; } = new List<SelectListItem>();
    public List<CustomerItem> Customers { get; private set; } = new();

    public void OnGet()
    {
        LoadData();
    }

    public IActionResult OnPost()
    {
        if (!ModelState.IsValid)
        {
            LoadData();
            return Page();
        }

        TempData["Success"] = "Form submitted successfully!";
        return RedirectToPage();
    }

    private void LoadData()
    {
        // Countries
        Countries = new List<SelectListItem>
        {
            new SelectListItem("United States", "us"),
            new SelectListItem("Canada", "ca"),
            new SelectListItem("United Kingdom", "uk"),
            new SelectListItem("Germany", "de"),
            new SelectListItem("France", "fr"),
            new SelectListItem("India", "in"),
            new SelectListItem("Japan", "jp"),
            new SelectListItem("Australia", "au")
        };

        // Plans
        Plans = new List<SelectListItem>
        {
            new SelectListItem("Free", "free"),
            new SelectListItem("Pro", "pro"),
            new SelectListItem("Enterprise", "enterprise")
        };

        // Sample customers
        Customers = new List<CustomerItem>
        {
            new CustomerItem { Id = 1, Name = "Alice Johnson", Email = "alice@example.com", Company = "Tech Corp", Status = "Active" },
            new CustomerItem { Id = 2, Name = "Bob Smith", Email = "bob@example.com", Company = "Design Studio", Status = "Active" },
            new CustomerItem { Id = 3, Name = "Carol White", Email = "carol@example.com", Company = "Marketing Inc", Status = "Pending" },
            new CustomerItem { Id = 4, Name = "David Brown", Email = "david@example.com", Company = "Consulting LLC", Status = "Active" },
            new CustomerItem { Id = 5, Name = "Eva Green", Email = "eva@example.com", Company = "Innovation Labs", Status = "Inactive" },
            new CustomerItem { Id = 6, Name = "Frank Miller", Email = "frank@example.com", Company = "Solutions Co", Status = "Active" },
            new CustomerItem { Id = 7, Name = "Grace Lee", Email = "grace@example.com", Company = "Digital Agency", Status = "Active" },
            new CustomerItem { Id = 8, Name = "Henry Wilson", Email = "henry@example.com", Company = "Creative Works", Status = "Pending" }
        };
    }
}

public class FormDataModel
{
    [Required(ErrorMessage = "Name is required")]
    [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
    public string? Name { get; set; }

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email address")]
    public string? Email { get; set; }

    [StringLength(500, ErrorMessage = "Message cannot exceed 500 characters")]
    public string? Message { get; set; }

    [Required(ErrorMessage = "Please select a country")]
    public string? Country { get; set; }

    [Range(typeof(bool), "true", "true", ErrorMessage = "You must accept the terms")]
    public bool AcceptTerms { get; set; }

    public bool EnableNotifications { get; set; }

    [Required(ErrorMessage = "Please select a plan")]
    public string? Plan { get; set; }

    public DateTime? StartDate { get; set; }
}

public class CustomerItem
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Company { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
}
