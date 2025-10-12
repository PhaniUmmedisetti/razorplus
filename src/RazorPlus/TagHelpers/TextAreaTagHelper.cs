using System.Collections.Generic;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace RazorPlus.TagHelpers;

/// <summary>
/// <rp-textarea asp-for label hint required rows cols />
/// Renders a multi-line control with shared RazorPlus field layout.
/// </summary>
[HtmlTargetElement("rp-textarea")]
public class TextAreaTagHelper : TagHelper
{
    private readonly IHtmlGenerator _generator;
    public TextAreaTagHelper(IHtmlGenerator generator) => _generator = generator;

    [HtmlAttributeNotBound]
    [ViewContext]
    public ViewContext ViewContext { get; set; } = default!;

    [HtmlAttributeName("asp-for")]
    public ModelExpression? For { get; set; }

    public string? Label { get; set; }
    public string? Hint { get; set; }
    public bool Required { get; set; }
    public int Rows { get; set; } = 4;
    public int? Cols { get; set; }
    public string? Placeholder { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "div";
        output.Attributes.SetAttribute("class", "rp-field");
        var enc = HtmlEncoder.Default;
        var labelText = Label ?? For?.Metadata?.DisplayName ?? For?.Name ?? string.Empty;

        if (!string.IsNullOrWhiteSpace(labelText))
        {
            var labelTag = _generator.GenerateLabel(ViewContext, For?.ModelExplorer, For?.Name, labelText, new { @class = "rp-label" });
            output.Content.AppendHtml(labelTag);
        }

        var rows = Rows > 0 ? Rows : 4;
        var attrs = new Dictionary<string, object?>
        {
            ["class"] = "rp-textarea__control"
        };
        if (Required)
        {
            attrs["required"] = "required";
        }
        if (!string.IsNullOrWhiteSpace(Placeholder))
        {
            attrs["placeholder"] = Placeholder;
        }
        if (Cols.HasValue && Cols.Value > 0)
        {
            attrs["cols"] = Cols.Value;
        }

        var textarea = _generator.GenerateTextArea(ViewContext, For?.ModelExplorer, For?.Name, rows, Cols ?? 0, attrs);
        output.Content.AppendHtml(textarea);

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
