using System.IO;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace RazorPlus.TagHelpers;

/// <summary>
/// <rp-radio-group asp-for label hint required items layout=\"horizontal|vertical\" />
/// </summary>
[HtmlTargetElement("rp-radio-group")]
public class RadioGroupTagHelper : TagHelper
{
    private readonly IHtmlGenerator _generator;
    public RadioGroupTagHelper(IHtmlGenerator generator) => _generator = generator;

    [HtmlAttributeNotBound]
    [ViewContext]
    public ViewContext ViewContext { get; set; } = default!;

    [HtmlAttributeName("asp-for")]
    public ModelExpression? For { get; set; }

    public IEnumerable<SelectListItem>? Items { get; set; }
    public string? Label { get; set; }
    public string? Hint { get; set; }
    public bool Required { get; set; }
    public string Layout { get; set; } = "vertical";

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "fieldset";
        output.Attributes.SetAttribute("class", $"rp-radio-group rp-radio-group--{Layout}");
        if (Required)
        {
            output.Attributes.SetAttribute("aria-required", "true");
        }

        var enc = HtmlEncoder.Default;
        var labelText = Label ?? For?.Metadata?.DisplayName ?? For?.Name ?? string.Empty;
        if (!string.IsNullOrWhiteSpace(labelText))
        {
            output.Content.AppendHtml($"<legend class=\"rp-label\">{enc.Encode(labelText)}</legend>");
        }

        var items = ResolveItems();
        var index = 0;
        foreach (var item in items)
        {
            var id = $"{For?.Name ?? "rp-radio"}_{index++}";
            var radio = _generator.GenerateRadioButton(ViewContext, For?.ModelExplorer, For?.Name, item.Value ?? item.Text, item.Selected, new
            {
                id,
                @class = "rp-radio__input"
            });
            using var writer = new StringWriter();
            radio.WriteTo(writer, enc);

            var inputHtml = writer.ToString();
            output.Content.AppendHtml($"<label class=\"rp-radio\" for=\"{id}\">{inputHtml}<span class=\"rp-radio__label\">{enc.Encode(item.Text)}</span></label>");
        }

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

    private IEnumerable<SelectListItem> ResolveItems()
    {
        if (Items != null) return Items;

        if (For?.Metadata?.EnumGroupedDisplayNamesAndValues is IEnumerable<SelectListItem> enumItems)
        {
            return enumItems;
        }

        return Array.Empty<SelectListItem>();
    }
}
