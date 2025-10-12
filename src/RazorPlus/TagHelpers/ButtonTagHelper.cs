using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace RazorPlus.TagHelpers;

/// <summary>
/// <rp-button variant="primary|secondary|ghost|danger" size="sm|md|lg" icon block loading as="button|a" href>
/// Minimal accessible button/link with consistent styling.
/// </summary>
[HtmlTargetElement("rp-button", TagStructure = TagStructure.NormalOrSelfClosing)]
public class ButtonTagHelper : TagHelper
{
    public string? Variant { get; set; } = "primary";
    public string? Size { get; set; } = "md";
    public string? Icon { get; set; }
    public bool Block { get; set; }
    public bool Loading { get; set; }
    public string As { get; set; } = "button"; // button|a
    public string? Href { get; set; }
    public bool Disabled { get; set; }
    [HtmlAttributeName("type")]
    public string? ButtonType { get; set; } = "button";

    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        var tag = (As?.ToLowerInvariant() == "a") ? "a" : "button";
        output.TagName = tag;

        var content = await output.GetChildContentAsync();
        output.TagMode = TagMode.StartTagAndEndTag;

        // classes
        var cls = $"rp-btn rp-btn--{Variant} rp-btn--{Size}" + (Block ? " rp-btn--block" : "");
        var existing = output.Attributes["class"]?.Value?.ToString();
        var merged = string.IsNullOrWhiteSpace(existing) ? cls : ($"{existing} {cls}");
        output.Attributes.SetAttribute("class", merged);

        // attributes
        if (tag == "a")
        {
            if (!string.IsNullOrWhiteSpace(Href))
                output.Attributes.SetAttribute("href", Href);
            if (Disabled)
                output.Attributes.SetAttribute("aria-disabled", "true");
            output.Attributes.SetAttribute("role", "button");
            output.Attributes.SetAttribute("tabindex", Disabled ? "-1" : "0");
        }
        else
        {
            if (Disabled)
                output.Attributes.SetAttribute("disabled", "disabled");
            var typeValue = string.IsNullOrWhiteSpace(ButtonType) ? "button" : ButtonType;
            output.Attributes.SetAttribute("type", typeValue);
        }

        if (Loading)
        {
            output.Attributes.SetAttribute("data-rp-loading", "");
        }

        // icon + content
        if (!string.IsNullOrWhiteSpace(Icon))
        {
            var iconName = HtmlEncoder.Default.Encode(Icon);
            output.Content.AppendHtml($"<span class=\"rp-btn__icon\" aria-hidden=\"true\" data-icon=\"{iconName}\"></span>");
        }
        output.Content.AppendHtml($"<span class=\"rp-btn__label\">{content.GetContent()}</span>");
    }
}
