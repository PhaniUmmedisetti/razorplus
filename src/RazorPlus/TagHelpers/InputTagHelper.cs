using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace RazorPlus.TagHelpers;

/// <summary>
/// <rp-input asp-for label hint required prefix suffix />
/// Renders label + input + hint + validation message using the default MVC generator.
/// </summary>
[HtmlTargetElement("rp-input", TagStructure = TagStructure.NormalOrSelfClosing)]
public class InputTagHelper : TagHelper
{
    private readonly IHtmlGenerator _generator;
    public InputTagHelper(IHtmlGenerator generator) => _generator = generator;

    [HtmlAttributeNotBound]
    [ViewContext]
    public ViewContext ViewContext { get; set; } = default!;

    [HtmlAttributeName("asp-for")]
    public ModelExpression? For { get; set; }

    public string? Label { get; set; }
    public string? Hint { get; set; }
    public bool Required { get; set; }
    public string? Prefix { get; set; }
    public string? Suffix { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "div";
        output.Attributes.SetAttribute("class", "rp-field");

        var enc = HtmlEncoder.Default;
        var labelText = Label ?? For?.Metadata?.DisplayName ?? For?.Name ?? string.Empty;

        // label
        if (!string.IsNullOrWhiteSpace(labelText))
        {
            var labelTag = _generator.GenerateLabel(ViewContext, For?.ModelExplorer, For?.Name, labelText, new { @class = "rp-label" });
            output.Content.AppendHtml(labelTag);
        }

        // input wrapper
        output.Content.AppendHtml("<div class=\"rp-input\">");
        if (!string.IsNullOrEmpty(Prefix))
        {
            output.Content.AppendHtml($"<span class=\"rp-input__affix rp-input__prefix\">{enc.Encode(Prefix)}</span>");
        }

        var inputTag = _generator.GenerateTextBox(ViewContext, For?.ModelExplorer, For?.Name, For?.Model, null, new { @class = "rp-input__control", required = Required ? "required" : null });
        output.Content.AppendHtml(inputTag);

        if (!string.IsNullOrEmpty(Suffix))
        {
            output.Content.AppendHtml($"<span class=\"rp-input__affix rp-input__suffix\">{enc.Encode(Suffix)}</span>");
        }
        output.Content.AppendHtml("</div>");

        if (!string.IsNullOrWhiteSpace(Hint))
        {
            output.Content.AppendHtml($"<div class=\"rp-hint\">{enc.Encode(Hint)}</div>");
        }

        if (For != null)
        {
            var validation = _generator.GenerateValidationMessage(ViewContext, For.ModelExplorer, For.Name, message: null, tag: null, htmlAttributes: new { @class = "rp-error" });
            output.Content.AppendHtml(validation);
        }
    }
}

