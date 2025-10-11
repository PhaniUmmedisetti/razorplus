using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace RazorPlus.TagHelpers;

/// <summary>
/// <rp-select asp-for label items multiple clearable filterable>
/// Server-rendered select using provided items. JS can enhance via data-rp-select.
/// </summary>
[HtmlTargetElement("rp-select", TagStructure = TagStructure.NormalOrSelfClosing)]
public class SelectTagHelper : TagHelper
{
    private readonly IHtmlGenerator _generator;
    public SelectTagHelper(IHtmlGenerator generator) => _generator = generator;

    [HtmlAttributeNotBound]
    [ViewContext]
    public ViewContext ViewContext { get; set; } = default!;

    [HtmlAttributeName("asp-for")]
    public ModelExpression? For { get; set; }

    public IEnumerable<SelectListItem>? Items { get; set; }
    public string? Label { get; set; }
    public bool Multiple { get; set; }
    public bool Clearable { get; set; }
    public bool Filterable { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "div";
        output.Attributes.SetAttribute("class", "rp-field");

        var labelText = Label ?? For?.Metadata?.DisplayName ?? For?.Name ?? string.Empty;
        if (!string.IsNullOrWhiteSpace(labelText))
        {
            var labelTag = _generator.GenerateLabel(ViewContext, For?.ModelExplorer, For?.Name, labelText, new { @class = "rp-label" });
            output.Content.AppendHtml(labelTag);
        }

        var attrs = new Dictionary<string, object?>
        {
            ["class"] = "rp-select__control",
            ["data-rp-select"] = "",
            ["aria-haspopup"] = "listbox"
        };
        if (Multiple) attrs["multiple"] = "multiple";

        var select = _generator.GenerateSelect(ViewContext, For?.ModelExplorer, optionLabel: null, expression: For?.Name, selectList: Items, currentValues: null, allowMultiple: Multiple, htmlAttributes: attrs);
        output.Content.AppendHtml($"<div class=\"rp-select\">");
        output.Content.AppendHtml(select);
        output.Content.AppendHtml("</div>");
    }
}

