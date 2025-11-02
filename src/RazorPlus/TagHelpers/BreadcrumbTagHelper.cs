using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Text;
using System.Text.Encodings.Web;

namespace RazorPlus.TagHelpers;

/// <summary>
/// Breadcrumb navigation component for hierarchical navigation trails.
/// </summary>
/// <example>
/// <code>
/// &lt;rp-breadcrumb&gt;
///   &lt;rp-breadcrumb-item text="Home" href="/" /&gt;
///   &lt;rp-breadcrumb-item text="Products" href="/products" /&gt;
///   &lt;rp-breadcrumb-item text="Details" active="true" /&gt;
/// &lt;/rp-breadcrumb&gt;
/// </code>
/// </example>
[HtmlTargetElement("rp-breadcrumb", TagStructure = TagStructure.NormalOrSelfClosing)]
public class BreadcrumbTagHelper : TagHelper
{
    /// <summary>
    /// Separator character/text
    /// </summary>
    public string Separator { get; set; } = "/";

    /// <summary>
    /// Additional CSS class
    /// </summary>
    public string? CssClass { get; set; }

    public List<BreadcrumbItemDefinition> Items { get; } = new();

    public override void Init(TagHelperContext context)
    {
        context.Items[typeof(BreadcrumbTagHelper)] = this;
    }

    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "nav";

        var cssClasses = new List<string> { "rp-breadcrumb" };
        if (!string.IsNullOrWhiteSpace(CssClass)) cssClasses.Add(CssClass);
        output.Attributes.SetAttribute("class", string.Join(" ", cssClasses));
        output.Attributes.SetAttribute("aria-label", "Breadcrumb");

        // Process children to collect items
        await output.GetChildContentAsync();

        var content = new StringBuilder();
        content.Append("<ol class=\"rp-breadcrumb__list\">");

        for (int i = 0; i < Items.Count; i++)
        {
            var item = Items[i];
            var isLast = i == Items.Count - 1;

            content.Append(item.Render(isLast, Separator));
        }

        content.Append("</ol>");

        output.Content.SetHtmlContent(content.ToString());
    }
}

/// <summary>
/// Individual breadcrumb item
/// </summary>
[HtmlTargetElement("rp-breadcrumb-item", ParentTag = "rp-breadcrumb", TagStructure = TagStructure.NormalOrSelfClosing)]
public class BreadcrumbItemTagHelper : TagHelper
{
    /// <summary>
    /// Item text
    /// </summary>
    public string? Text { get; set; }

    /// <summary>
    /// Link URL
    /// </summary>
    public string? Href { get; set; }

    /// <summary>
    /// Icon identifier
    /// </summary>
    public string? Icon { get; set; }

    /// <summary>
    /// Active/current page state
    /// </summary>
    public bool Active { get; set; }

    /// <summary>
    /// Additional CSS class
    /// </summary>
    public string? CssClass { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        if (context.Items.TryGetValue(typeof(BreadcrumbTagHelper), out var parent) && parent is BreadcrumbTagHelper breadcrumb)
        {
            var definition = new BreadcrumbItemDefinition
            {
                Text = Text,
                Href = Href,
                Icon = Icon,
                Active = Active,
                CssClass = CssClass
            };

            breadcrumb.Items.Add(definition);
        }

        output.SuppressOutput();
    }
}

public class BreadcrumbItemDefinition
{
    public string? Text { get; set; }
    public string? Href { get; set; }
    public string? Icon { get; set; }
    public bool Active { get; set; }
    public string? CssClass { get; set; }

    public string Render(bool isLast, string separator)
    {
        var encoder = HtmlEncoder.Default;
        var cssClasses = new List<string> { "rp-breadcrumb__item" };
        if (Active || isLast) cssClasses.Add("rp-breadcrumb__item--active");
        if (!string.IsNullOrWhiteSpace(CssClass)) cssClasses.Add(CssClass);

        var sb = new StringBuilder();
        sb.Append($"<li class=\"{string.Join(" ", cssClasses)}\">");

        var iconHtml = !string.IsNullOrWhiteSpace(Icon)
            ? $"<span class=\"rp-breadcrumb__icon\" data-icon=\"{encoder.Encode(Icon)}\"></span>"
            : "";

        if (!Active && !isLast && !string.IsNullOrWhiteSpace(Href))
        {
            // Clickable link
            sb.Append($"<a href=\"{encoder.Encode(Href)}\" class=\"rp-breadcrumb__link\">");
            sb.Append(iconHtml);
            sb.Append(encoder.Encode(Text ?? ""));
            sb.Append("</a>");
        }
        else
        {
            // Plain text (active/last item)
            sb.Append("<span class=\"rp-breadcrumb__text\" aria-current=\"page\">");
            sb.Append(iconHtml);
            sb.Append(encoder.Encode(Text ?? ""));
            sb.Append("</span>");
        }

        // Separator (not for last item)
        if (!isLast)
        {
            sb.Append($"<span class=\"rp-breadcrumb__separator\" aria-hidden=\"true\">{encoder.Encode(separator)}</span>");
        }

        sb.Append("</li>");
        return sb.ToString();
    }
}
