using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Text.Encodings.Web;

namespace RazorPlus.TagHelpers;

/// <summary>
/// Renders a date/datetime picker input with calendar popup.
/// Supports single date, date range, and time selection.
/// </summary>
/// <example>
/// <code>
/// &lt;rp-date-picker asp-for="StartDate"
///                  label="Start Date"
///                  format="MM/dd/yyyy"
///                  range="false" /&gt;
/// </code>
/// </example>
[HtmlTargetElement("rp-date-picker", TagStructure = TagStructure.NormalOrSelfClosing)]
public class DatePickerTagHelper : TagHelper
{
    private readonly IHtmlGenerator _generator;

    [HtmlAttributeNotBound]
    [ViewContext]
    public ViewContext? ViewContext { get; set; }

    /// <summary>
    /// Model expression for binding
    /// </summary>
    public ModelExpression? For { get; set; }

    /// <summary>
    /// Label text
    /// </summary>
    public string? Label { get; set; }

    /// <summary>
    /// Date format (default: MM/dd/yyyy)
    /// </summary>
    public string Format { get; set; } = "MM/dd/yyyy";

    /// <summary>
    /// Enable date range selection
    /// </summary>
    public bool Range { get; set; }

    /// <summary>
    /// Enable time selection
    /// </summary>
    public bool Time { get; set; }

    /// <summary>
    /// Minimum allowed date (ISO format: yyyy-MM-dd)
    /// </summary>
    public string? MinDate { get; set; }

    /// <summary>
    /// Maximum allowed date (ISO format: yyyy-MM-dd)
    /// </summary>
    public string? MaxDate { get; set; }

    /// <summary>
    /// Placeholder text
    /// </summary>
    public string? Placeholder { get; set; }

    /// <summary>
    /// Required field
    /// </summary>
    public bool Required { get; set; }

    /// <summary>
    /// Hint text
    /// </summary>
    public string? Hint { get; set; }

    /// <summary>
    /// Disabled state
    /// </summary>
    public bool Disabled { get; set; }

    /// <summary>
    /// Read-only state
    /// </summary>
    public bool Readonly { get; set; }

    /// <summary>
    /// Show clear button
    /// </summary>
    public bool Clearable { get; set; } = true;

    public DatePickerTagHelper(IHtmlGenerator generator)
    {
        _generator = generator;
    }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        var encoder = HtmlEncoder.Default;
        output.TagName = "div";
        output.Attributes.SetAttribute("class", "rp-field rp-date-picker");

        var content = new StringWriter();

        // Label
        var labelText = !string.IsNullOrWhiteSpace(Label) ? Label : For?.Metadata.DisplayName ?? For?.Name ?? "Date";
        content.Write($"<label class=\"rp-label\">{encoder.Encode(labelText)}</label>");

        if (Required)
        {
            content.Write("<span class=\"rp-required\" style=\"color: var(--rp-danger, #dc2626); margin-left: 2px;\">*</span>");
        }

        // Input wrapper
        content.Write("<div class=\"rp-input\">");

        // Generate input
        var inputId = For?.Name ?? $"datepicker{Guid.NewGuid():N}";
        var inputAttrs = new Dictionary<string, object?>
        {
            ["class"] = "rp-input__control rp-date-picker__input",
            ["id"] = inputId,
            ["type"] = "text",
            ["data-rp-date-picker"] = "",
            ["data-format"] = Format,
            ["placeholder"] = Placeholder ?? Format
        };

        if (Range) inputAttrs["data-range"] = "true";
        if (Time) inputAttrs["data-time"] = "true";
        if (!string.IsNullOrWhiteSpace(MinDate)) inputAttrs["data-min-date"] = MinDate;
        if (!string.IsNullOrWhiteSpace(MaxDate)) inputAttrs["data-max-date"] = MaxDate;
        if (Clearable) inputAttrs["data-clearable"] = "true";
        if (Disabled) inputAttrs["disabled"] = "disabled";
        if (Readonly) inputAttrs["readonly"] = "readonly";
        if (Required) inputAttrs["required"] = "required";

        if (For != null && ViewContext != null)
        {
            var input = _generator.GenerateTextBox(
                ViewContext,
                For.ModelExplorer,
                For.Name,
                For.Model,
                null,
                inputAttrs);

            using var writer = new StringWriter();
            input.WriteTo(writer, encoder);
            content.Write(writer.ToString());
        }
        else
        {
            var attrs = string.Join(" ", inputAttrs.Where(kv => kv.Value != null).Select(kv => $"{kv.Key}=\"{encoder.Encode(kv.Value?.ToString() ?? "")}\""));
            content.Write($"<input {attrs} />");
        }

        // Calendar icon
        content.Write("<span class=\"rp-date-picker__icon\" aria-hidden=\"true\">📅</span>");

        content.Write("</div>"); // End input wrapper

        // Hint
        if (!string.IsNullOrWhiteSpace(Hint))
        {
            content.Write($"<div class=\"rp-hint\">{encoder.Encode(Hint)}</div>");
        }

        // Validation message
        if (For != null && ViewContext != null)
        {
            var validation = _generator.GenerateValidationMessage(
                ViewContext,
                For.ModelExplorer,
                For.Name,
                null,
                null,
                new { @class = "rp-error" });

            using var validWriter = new StringWriter();
            validation.WriteTo(validWriter, encoder);
            content.Write(validWriter.ToString());
        }

        output.Content.SetHtmlContent(content.ToString());
    }
}
