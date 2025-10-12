using System.Collections.Generic;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace RazorPlus.TagHelpers;

/// <summary>
/// <rp-switch asp-for label hint required on-text off-text />
/// Renders a toggle switch tied to a boolean model property.
/// </summary>
[HtmlTargetElement("rp-switch")]
public class SwitchTagHelper : TagHelper
{
    private readonly IHtmlGenerator _generator;
    public SwitchTagHelper(IHtmlGenerator generator) => _generator = generator;

    [HtmlAttributeNotBound]
    [ViewContext]
    public ViewContext ViewContext { get; set; } = default!;

    [HtmlAttributeName("asp-for")]
    public ModelExpression? For { get; set; }

    public string? Label { get; set; }
    public string? Hint { get; set; }
    public bool Required { get; set; }
    public string? OnText { get; set; }
    public string? OffText { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "div";
        output.Attributes.SetAttribute("class", "rp-field rp-switch");
        var enc = HtmlEncoder.Default;

        var labelText = Label ?? For?.Metadata?.DisplayName ?? For?.Name ?? string.Empty;
        if (!string.IsNullOrWhiteSpace(labelText))
        {
            output.Content.AppendHtml($"<div class=\"rp-label\">{enc.Encode(labelText)}</div>");
        }

        var checkedValue = For?.Model as bool?;
        var attributes = new Dictionary<string, object?>
        {
            ["class"] = "rp-switch__control",
            ["role"] = "switch",
            ["aria-checked"] = checkedValue == true ? "true" : "false"
        };
        if (Required)
        {
            attributes["required"] = "required";
        }

        var checkbox = _generator.GenerateCheckBox(ViewContext, For?.ModelExplorer, For?.Name, isChecked: checkedValue, htmlAttributes: attributes);

        output.Content.AppendHtml("<div class=\"rp-switch__track\" data-rp-switch>");
        output.Content.AppendHtml(checkbox);
        var stateAttributes = new List<string>();
        if (!string.IsNullOrWhiteSpace(OnText))
        {
            stateAttributes.Add($"data-on=\"{enc.Encode(OnText)}\"");
        }
        if (!string.IsNullOrWhiteSpace(OffText))
        {
            stateAttributes.Add($"data-off=\"{enc.Encode(OffText)}\"");
        }
        var stateAttrString = stateAttributes.Count > 0 ? " " + string.Join(" ", stateAttributes) : string.Empty;
        output.Content.AppendHtml($"<span class=\"rp-switch__state\"{stateAttrString}></span>");
        output.Content.AppendHtml("<span class=\"rp-switch__thumb\"></span>");
        output.Content.AppendHtml("</div>");

        if (!string.IsNullOrWhiteSpace(Hint))
        {
            output.Content.AppendHtml($"<div class=\"rp-hint\">{enc.Encode(Hint)}</div>");
        }

        if (For != null)
        {
            var validation = _generator.GenerateValidationMessage(ViewContext, For.ModelExplorer, For.Name, null, null, new { @class = "rp-error" });
            output.Content.AppendHtml(validation);
        }
    }
}
