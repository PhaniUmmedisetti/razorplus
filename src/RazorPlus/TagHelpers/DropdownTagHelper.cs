using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Text;
using System.Text.Encodings.Web;

namespace RazorPlus.TagHelpers;

/// <summary>
/// Dropdown menu with nested submenus, dividers, and action items.
/// Perfect for action menus, context menus, and navigation dropdowns.
/// </summary>
/// <example>
/// <code>
/// &lt;rp-dropdown trigger-text="Actions"&gt;
///   &lt;rp-dropdown-item text="Edit" icon="edit" href="/edit" /&gt;
///   &lt;rp-dropdown-item text="Delete" icon="trash" onclick="confirmDelete()" /&gt;
///   &lt;rp-dropdown-divider /&gt;
///   &lt;rp-dropdown text="More Actions"&gt;
///     &lt;rp-dropdown-item text="Export" /&gt;
///     &lt;rp-dropdown-item text="Archive" /&gt;
///   &lt;/rp-dropdown&gt;
/// &lt;/rp-dropdown&gt;
/// </code>
/// </example>
[HtmlTargetElement("rp-dropdown", TagStructure = TagStructure.NormalOrSelfClosing)]
public class DropdownTagHelper : TagHelper
{
    /// <summary>
    /// Unique ID for the dropdown
    /// </summary>
    public string? Id { get; set; }

    /// <summary>
    /// Trigger button text
    /// </summary>
    public string? TriggerText { get; set; }

    /// <summary>
    /// Trigger button icon
    /// </summary>
    public string? TriggerIcon { get; set; }

    /// <summary>
    /// Button variant (primary, secondary, ghost, danger)
    /// </summary>
    public string Variant { get; set; } = "ghost";

    /// <summary>
    /// Button size (sm, md, lg)
    /// </summary>
    public string Size { get; set; } = "md";

    /// <summary>
    /// Menu alignment (left or right)
    /// </summary>
    public string Align { get; set; } = "left";

    /// <summary>
    /// Additional CSS class for the container
    /// </summary>
    public string? CssClass { get; set; }

    /// <summary>
    /// Text for nested submenu (when used inside another dropdown)
    /// </summary>
    public string? Text { get; set; }

    /// <summary>
    /// Icon for nested submenu item
    /// </summary>
    public string? Icon { get; set; }

    public List<DropdownItemDefinition> Items { get; } = new();

    public override void Init(TagHelperContext context)
    {
        context.Items[typeof(DropdownTagHelper)] = this;
    }

    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        var encoder = HtmlEncoder.Default;
        var id = string.IsNullOrWhiteSpace(Id) ? $"rp-dropdown-{Guid.NewGuid():N}" : Id!;

        // Check if this is a nested submenu
        var isNested = context.Items.TryGetValue("IsNestedDropdown", out var nestedFlag) && nestedFlag is bool b && b;

        if (isNested)
        {
            // Render as submenu item
            output.TagName = "div";
            output.Attributes.SetAttribute("class", "rp-dropdown-submenu");

            // Process children
            context.Items["IsNestedDropdown"] = true;
            await output.GetChildContentAsync();

            var iconHtml = !string.IsNullOrWhiteSpace(Icon)
                ? $"<span class=\"rp-dropdown-item__icon\" data-icon=\"{encoder.Encode(Icon)}\"></span>"
                : "";

            var text = Text ?? TriggerText ?? "Menu";

            var content = $@"
                <button type=""button"" class=""rp-dropdown-item rp-dropdown-submenu__trigger"">
                    {iconHtml}
                    <span class=""rp-dropdown-item__text"">{encoder.Encode(text)}</span>
                    <span class=""rp-dropdown-submenu__arrow"">›</span>
                </button>
                <div class=""rp-dropdown-menu rp-dropdown-menu--submenu"" role=""menu"">
                    {string.Join("", Items.Select(i => i.Render()))}
                </div>";

            output.Content.SetHtmlContent(content);
        }
        else
        {
            // Render as top-level dropdown
            output.TagName = "div";

            var cssClasses = new List<string> { "rp-dropdown" };
            if (!string.IsNullOrWhiteSpace(CssClass)) cssClasses.Add(CssClass);
            output.Attributes.SetAttribute("class", string.Join(" ", cssClasses));
            output.Attributes.SetAttribute("data-rp-dropdown", "");
            output.Attributes.SetAttribute("id", id);

            // Process children to collect items
            context.Items["IsNestedDropdown"] = true;
            await output.GetChildContentAsync();

            var triggerText = TriggerText ?? "Menu";
            var triggerIconHtml = !string.IsNullOrWhiteSpace(TriggerIcon)
                ? $"<span class=\"rp-btn__icon\" data-icon=\"{encoder.Encode(TriggerIcon)}\"></span>"
                : "";

            var content = $@"
                <button type=""button""
                        class=""rp-btn rp-btn--{Variant} rp-btn--{Size} rp-dropdown__trigger""
                        aria-haspopup=""true""
                        aria-expanded=""false""
                        aria-controls=""{id}-menu"">
                    {triggerIconHtml}
                    <span class=""rp-btn__label"">{encoder.Encode(triggerText)}</span>
                    <span class=""rp-dropdown__caret"">▾</span>
                </button>
                <div class=""rp-dropdown-menu rp-dropdown-menu--{Align}"" id=""{id}-menu"" role=""menu"" hidden>
                    {string.Join("", Items.Select(i => i.Render()))}
                </div>";

            output.Content.SetHtmlContent(content);
        }
    }
}

/// <summary>
/// Individual dropdown menu item
/// </summary>
[HtmlTargetElement("rp-dropdown-item", ParentTag = "rp-dropdown", TagStructure = TagStructure.NormalOrSelfClosing)]
public class DropdownItemTagHelper : TagHelper
{
    /// <summary>
    /// Item text
    /// </summary>
    public string? Text { get; set; }

    /// <summary>
    /// Icon identifier
    /// </summary>
    public string? Icon { get; set; }

    /// <summary>
    /// Link URL
    /// </summary>
    public string? Href { get; set; }

    /// <summary>
    /// JavaScript click handler
    /// </summary>
    public string? Onclick { get; set; }

    /// <summary>
    /// Disabled state
    /// </summary>
    public bool Disabled { get; set; }

    /// <summary>
    /// Additional CSS class
    /// </summary>
    public string? CssClass { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        if (context.Items.TryGetValue(typeof(DropdownTagHelper), out var parent) && parent is DropdownTagHelper dropdown)
        {
            var definition = new DropdownItemDefinition
            {
                Text = Text,
                Icon = Icon,
                Href = Href,
                Onclick = Onclick,
                Disabled = Disabled,
                CssClass = CssClass
            };

            dropdown.Items.Add(definition);
        }

        output.SuppressOutput();
    }
}

/// <summary>
/// Dropdown menu divider
/// </summary>
[HtmlTargetElement("rp-dropdown-divider", ParentTag = "rp-dropdown", TagStructure = TagStructure.WithoutEndTag)]
public class DropdownDividerTagHelper : TagHelper
{
    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        if (context.Items.TryGetValue(typeof(DropdownTagHelper), out var parent) && parent is DropdownTagHelper dropdown)
        {
            dropdown.Items.Add(new DropdownItemDefinition { IsDivider = true });
        }

        output.SuppressOutput();
    }
}

public class DropdownItemDefinition
{
    public string? Text { get; set; }
    public string? Icon { get; set; }
    public string? Href { get; set; }
    public string? Onclick { get; set; }
    public bool Disabled { get; set; }
    public string? CssClass { get; set; }
    public bool IsDivider { get; set; }

    public string Render()
    {
        var encoder = HtmlEncoder.Default;

        if (IsDivider)
        {
            return "<div class=\"rp-dropdown-divider\" role=\"separator\"></div>";
        }

        var cssClasses = new List<string> { "rp-dropdown-item" };
        if (Disabled) cssClasses.Add("rp-dropdown-item--disabled");
        if (!string.IsNullOrWhiteSpace(CssClass)) cssClasses.Add(CssClass);

        var tagName = !string.IsNullOrWhiteSpace(Href) && !Disabled ? "a" : "button";
        var hrefAttr = !string.IsNullOrWhiteSpace(Href) && !Disabled ? $" href=\"{encoder.Encode(Href)}\"" : "";
        var onclickAttr = !string.IsNullOrWhiteSpace(Onclick) && !Disabled ? $" onclick=\"{encoder.Encode(Onclick)}\"" : "";
        var typeAttr = tagName == "button" ? " type=\"button\"" : "";
        var roleAttr = " role=\"menuitem\"";
        var disabledAttr = Disabled ? " disabled aria-disabled=\"true\"" : "";

        var iconHtml = !string.IsNullOrWhiteSpace(Icon)
            ? $"<span class=\"rp-dropdown-item__icon\" data-icon=\"{encoder.Encode(Icon)}\"></span>"
            : "";

        return $@"<{tagName} class=""{string.Join(" ", cssClasses)}""{hrefAttr}{onclickAttr}{typeAttr}{roleAttr}{disabledAttr}>
            {iconHtml}
            <span class=""rp-dropdown-item__text"">{encoder.Encode(Text ?? "")}</span>
        </{tagName}>";
    }
}
