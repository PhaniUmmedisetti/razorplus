using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Text;
using System.Text.Encodings.Web;

namespace RazorPlus.TagHelpers;

/// <summary>
/// Renders a list of items with click handlers, navigation, and custom templates.
/// Perfect for navigation menus, item lists, and selectable lists.
/// </summary>
/// <example>
/// <code>
/// &lt;rp-list items="Model.Products"&gt;
///   &lt;rp-list-item href="/product/@item.Id"
///                 title="@item.Name"
///                 description="@item.Description"
///                 icon="box" /&gt;
/// &lt;/rp-list&gt;
/// </code>
/// </example>
[HtmlTargetElement("rp-list", TagStructure = TagStructure.NormalOrSelfClosing)]
public class ListTagHelper : TagHelper
{
    /// <summary>
    /// Items collection (optional - can use child rp-list-item tags instead)
    /// </summary>
    public IEnumerable<object>? Items { get; set; }

    /// <summary>
    /// Enable hover effect
    /// </summary>
    public bool Hoverable { get; set; } = true;

    /// <summary>
    /// Enable borders between items
    /// </summary>
    public bool Bordered { get; set; } = true;

    /// <summary>
    /// Enable striped background
    /// </summary>
    public bool Striped { get; set; }

    /// <summary>
    /// Compact spacing
    /// </summary>
    public bool Compact { get; set; }

    /// <summary>
    /// Additional CSS class
    /// </summary>
    public string? CssClass { get; set; }

    public List<ListItemDefinition> ListItems { get; } = new();

    public override void Init(TagHelperContext context)
    {
        context.Items[typeof(ListTagHelper)] = this;
    }

    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "ul";

        var cssClasses = new List<string> { "rp-list" };
        if (Hoverable) cssClasses.Add("rp-list--hoverable");
        if (Bordered) cssClasses.Add("rp-list--bordered");
        if (Striped) cssClasses.Add("rp-list--striped");
        if (Compact) cssClasses.Add("rp-list--compact");
        if (!string.IsNullOrWhiteSpace(CssClass)) cssClasses.Add(CssClass);

        output.Attributes.SetAttribute("class", string.Join(" ", cssClasses));
        output.Attributes.SetAttribute("data-rp-list", "");

        // Process child content to collect list items
        await output.GetChildContentAsync();

        var content = new StringBuilder();

        // If items were defined, render them
        foreach (var itemDef in ListItems)
        {
            content.Append(itemDef.Render());
        }

        output.Content.SetHtmlContent(content.ToString());
    }
}

/// <summary>
/// Individual list item within rp-list
/// </summary>
[HtmlTargetElement("rp-list-item", ParentTag = "rp-list", TagStructure = TagStructure.NormalOrSelfClosing)]
public class ListItemTagHelper : TagHelper
{
    /// <summary>
    /// Item title/main text
    /// </summary>
    public string? Title { get; set; }

    /// <summary>
    /// Item description/subtitle
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Icon name (displayed on left)
    /// </summary>
    public string? Icon { get; set; }

    /// <summary>
    /// Link URL (makes item clickable)
    /// </summary>
    public string? Href { get; set; }

    /// <summary>
    /// Click handler JavaScript
    /// </summary>
    public string? Onclick { get; set; }

    /// <summary>
    /// Badge text (displayed on right)
    /// </summary>
    public string? Badge { get; set; }

    /// <summary>
    /// Badge variant
    /// </summary>
    public string? BadgeVariant { get; set; }

    /// <summary>
    /// Active/selected state
    /// </summary>
    public bool Active { get; set; }

    /// <summary>
    /// Disabled state
    /// </summary>
    public bool Disabled { get; set; }

    /// <summary>
    /// Custom data value (rendered as data-value attribute)
    /// </summary>
    public string? CustomDataValue { get; set; }

    /// <summary>
    /// Additional CSS class
    /// </summary>
    public string? CssClass { get; set; }

    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        if (context.Items.TryGetValue(typeof(ListTagHelper), out var parent) && parent is ListTagHelper list)
        {
            var childContent = await output.GetChildContentAsync();
            var customContent = childContent.GetContent();

            var definition = new ListItemDefinition
            {
                Title = Title,
                Description = Description,
                Icon = Icon,
                Href = Href,
                Onclick = Onclick,
                Badge = Badge,
                BadgeVariant = BadgeVariant,
                Active = Active,
                Disabled = Disabled,
                CustomDataValue = CustomDataValue,
                CssClass = CssClass,
                CustomContent = !string.IsNullOrWhiteSpace(customContent) ? customContent : null
            };

            list.ListItems.Add(definition);
        }

        output.SuppressOutput();
    }
}

public class ListItemDefinition
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? Icon { get; set; }
    public string? Href { get; set; }
    public string? Onclick { get; set; }
    public string? Badge { get; set; }
    public string? BadgeVariant { get; set; }
    public bool Active { get; set; }
    public bool Disabled { get; set; }
    public string? CustomDataValue { get; set; }
    public string? CssClass { get; set; }
    public string? CustomContent { get; set; }

    public string Render()
    {
        var encoder = HtmlEncoder.Default;
        var cssClasses = new List<string> { "rp-list-item" };
        if (Active) cssClasses.Add("rp-list-item--active");
        if (Disabled) cssClasses.Add("rp-list-item--disabled");
        if (!string.IsNullOrWhiteSpace(CssClass)) cssClasses.Add(CssClass);

        var tagName = !string.IsNullOrWhiteSpace(Href) ? "a" : "li";
        var hrefAttr = !string.IsNullOrWhiteSpace(Href) ? $" href=\"{encoder.Encode(Href)}\"" : "";
        var onclickAttr = !string.IsNullOrWhiteSpace(Onclick) ? $" onclick=\"{encoder.Encode(Onclick)}\"" : "";
        var dataAttr = !string.IsNullOrWhiteSpace(CustomDataValue) ? $" data-value=\"{encoder.Encode(CustomDataValue)}\"" : "";
        var roleAttr = tagName == "a" ? "" : " role=\"button\" tabindex=\"0\"";

        var sb = new StringBuilder();
        sb.Append($"<{tagName} class=\"{string.Join(" ", cssClasses)}\"{hrefAttr}{onclickAttr}{dataAttr}{roleAttr}>");

        if (!string.IsNullOrWhiteSpace(CustomContent))
        {
            sb.Append(CustomContent);
        }
        else
        {
            // Icon
            if (!string.IsNullOrWhiteSpace(Icon))
            {
                sb.Append($"<span class=\"rp-list-item__icon\" aria-hidden=\"true\" data-icon=\"{encoder.Encode(Icon)}\"></span>");
            }

            // Content
            sb.Append("<div class=\"rp-list-item__content\">");

            if (!string.IsNullOrWhiteSpace(Title))
            {
                sb.Append($"<div class=\"rp-list-item__title\">{encoder.Encode(Title)}</div>");
            }

            if (!string.IsNullOrWhiteSpace(Description))
            {
                sb.Append($"<div class=\"rp-list-item__description\">{encoder.Encode(Description)}</div>");
            }

            sb.Append("</div>");

            // Badge
            if (!string.IsNullOrWhiteSpace(Badge))
            {
                var badgeClass = !string.IsNullOrWhiteSpace(BadgeVariant) ? $" rp-badge--{BadgeVariant}" : "";
                sb.Append($"<span class=\"rp-badge{badgeClass}\">{encoder.Encode(Badge)}</span>");
            }
        }

        sb.Append($"</{tagName}>");
        return sb.ToString();
    }
}
