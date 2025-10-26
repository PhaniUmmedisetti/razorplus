using System.Collections.Generic;
using System.Linq;
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
    public string? Placeholder { get; set; }
    public string? FetchUrl { get; set; }
    public string SearchParam { get; set; } = "q";
    public int SearchMin { get; set; } = 2;
    public int DebounceMs { get; set; } = 250;

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
        if (Clearable) attrs["data-rp-select-clearable"] = "true";
        if (Filterable) attrs["data-rp-select-filterable"] = "true";
        if (!string.IsNullOrEmpty(Placeholder))
        {
            attrs["data-rp-select-placeholder"] = Placeholder;
        }
        if (!string.IsNullOrWhiteSpace(FetchUrl))
        {
            attrs["data-rp-select-fetch"] = FetchUrl;
            attrs["data-rp-select-search-param"] = string.IsNullOrWhiteSpace(SearchParam) ? "q" : SearchParam;
            attrs["data-rp-select-search-min"] = SearchMin.ToString();
            attrs["data-rp-select-debounce"] = DebounceMs.ToString();
        }
        if (Clearable || Filterable || !string.IsNullOrWhiteSpace(FetchUrl))
        {
            attrs["data-rp-select-enhance"] = "true";
        }

        var items = Items?.ToList() ?? new List<SelectListItem>();
        if (!Multiple && Clearable && !string.IsNullOrWhiteSpace(Placeholder))
        {
            var hasValue = !(For?.Model == null || string.IsNullOrEmpty(For.Model.ToString()));
            var placeholderValue = new SelectListItem(Placeholder!, string.Empty, !hasValue);
            items.Insert(0, placeholderValue);
        }

        var select = _generator.GenerateSelect(ViewContext, For?.ModelExplorer, optionLabel: null, expression: For?.Name, selectList: items, currentValues: null, allowMultiple: Multiple, htmlAttributes: attrs);
        output.Content.AppendHtml("<div class=\"rp-select\" data-rp-select-container>");
        output.Content.AppendHtml(select);
        output.Content.AppendHtml("</div>");
    }
}
