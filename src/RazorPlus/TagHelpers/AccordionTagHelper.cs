using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace RazorPlus.TagHelpers;

/// <summary>
/// <rp-accordion> with child <rp-accordion-item> elements.
/// </summary>
[HtmlTargetElement("rp-accordion")]
public class AccordionTagHelper : TagHelper
{
    public string? Id { get; set; }

    public override void Init(TagHelperContext context)
    {
        context.Items[typeof(AccordionTagHelper)] = this;
    }

    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        await output.GetChildContentAsync();
        output.TagName = "div";
        output.TagMode = TagMode.StartTagAndEndTag;
        output.Attributes.SetAttribute("class", "rp-accordion");
        output.Attributes.SetAttribute("data-rp-accordion", string.Empty);
        if (!string.IsNullOrWhiteSpace(Id))
        {
            output.Attributes.SetAttribute("id", Id);
        }
    }
}

[HtmlTargetElement("rp-accordion-item", ParentTag = "rp-accordion")]
public class AccordionItemTagHelper : TagHelper
{
    public string? Id { get; set; }
    public string? Header { get; set; }
    public bool Expanded { get; set; }

    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        var enc = HtmlEncoder.Default;
        var content = await output.GetChildContentAsync();

        var baseId = !string.IsNullOrWhiteSpace(Id) ? Id! : $"rp-acc-{Guid.NewGuid():N}";
        var buttonId = $"{baseId}-trigger";
        var panelId = $"{baseId}-panel";
        var headerText = string.IsNullOrWhiteSpace(Header) ? "Item" : Header!;

        output.TagName = "div";
        output.TagMode = TagMode.StartTagAndEndTag;
        output.Attributes.SetAttribute("class", "rp-accordion__item");

        var expandedAttr = Expanded ? "true" : "false";
        var hiddenAttr = Expanded ? string.Empty : " hidden";

        var trigger = $"<button type=\"button\" class=\"rp-accordion__trigger\" id=\"{buttonId}\" aria-expanded=\"{expandedAttr}\" aria-controls=\"{panelId}\" data-rp-accordion-trigger>{enc.Encode(headerText)}</button>";
        var panel = $"<div class=\"rp-accordion__panel\" id=\"{panelId}\" role=\"region\" aria-labelledby=\"{buttonId}\"{hiddenAttr} data-rp-accordion-panel>{content.GetContent()}</div>";

        output.Content.SetHtmlContent(trigger + panel);
    }
}

