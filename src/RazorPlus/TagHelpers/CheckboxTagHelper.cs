using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Text;
using System.Text.Encodings.Web;

namespace RazorPlus.TagHelpers;

/// <summary>
/// Individual checkbox with label and validation support.
/// For boolean properties or acceptance checkboxes.
/// </summary>
/// <example>
/// <code>
/// &lt;rp-checkbox asp-for="AcceptTerms"
///              label="I accept the terms and conditions"
///              required="true" /&gt;
/// </code>
/// </example>
[HtmlTargetElement("rp-checkbox", TagStructure = TagStructure.NormalOrSelfClosing)]
public class CheckboxTagHelper : TagHelper
{
    private readonly IHtmlGenerator _generator;

    public CheckboxTagHelper(IHtmlGenerator generator)
    {
        _generator = generator;
    }

    [HtmlAttributeNotBound]
    [ViewContext]
    public ViewContext? ViewContext { get; set; }

    /// <summary>
    /// Model binding expression
    /// </summary>
    public ModelExpression? For { get; set; }

    /// <summary>
    /// Label text (displayed to the right of checkbox)
    /// </summary>
    public string? Label { get; set; }

    /// <summary>
    /// Help text displayed below checkbox
    /// </summary>
    public string? Hint { get; set; }

    /// <summary>
    /// Whether the field is required
    /// </summary>
    public bool Required { get; set; }

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
        var encoder = HtmlEncoder.Default;
        output.TagName = "div";

        var cssClasses = new List<string> { "rp-field", "rp-checkbox-field" };
        if (!string.IsNullOrWhiteSpace(CssClass)) cssClasses.Add(CssClass);
        output.Attributes.SetAttribute("class", string.Join(" ", cssClasses));

        var id = For?.Name != null ? $"rp-checkbox-{For.Name}" : $"rp-checkbox-{Guid.NewGuid():N}";
        var labelText = Label ?? For?.Metadata?.DisplayName ?? "";

        var content = new StringBuilder();

        // Checkbox wrapper
        content.Append("<div class=\"rp-checkbox\">");

        // Generate checkbox input
        var checkedValue = For?.Model as bool?;
        var inputAttrs = new Dictionary<string, object?>
        {
            ["id"] = id,
            ["class"] = "rp-checkbox__input"
        };

        if (Required) inputAttrs["required"] = "required";
        if (Disabled) inputAttrs["disabled"] = "disabled";

        var checkbox = _generator.GenerateCheckBox(
            ViewContext!,
            For?.ModelExplorer,
            For?.Name ?? "checkbox",
            isChecked: checkedValue == true,
            htmlAttributes: inputAttrs
        );

        using var checkboxWriter = new StringWriter();
        checkbox.WriteTo(checkboxWriter, encoder);
        content.Append(checkboxWriter.ToString());

        // Label
        content.Append($"<label class=\"rp-checkbox__label\" for=\"{id}\">");
        if (!string.IsNullOrWhiteSpace(labelText))
        {
            content.Append(encoder.Encode(labelText));
            if (Required)
            {
                content.Append("<span class=\"rp-label__required\" aria-label=\"required\">*</span>");
            }
        }
        content.Append("</label>");

        content.Append("</div>"); // Close checkbox wrapper

        // Hint text
        if (!string.IsNullOrWhiteSpace(Hint))
        {
            content.Append($"<div class=\"rp-hint\">{encoder.Encode(Hint)}</div>");
        }

        // Validation message
        if (For != null && ViewContext != null)
        {
            var validation = _generator.GenerateValidationMessage(
                ViewContext,
                For.ModelExplorer,
                For.Name,
                message: null,
                tag: null,
                htmlAttributes: new { @class = "rp-error" }
            );

            using var validationWriter = new StringWriter();
            validation.WriteTo(validationWriter, encoder);
            content.Append(validationWriter.ToString());
        }

        output.Content.SetHtmlContent(content.ToString());
    }
}
