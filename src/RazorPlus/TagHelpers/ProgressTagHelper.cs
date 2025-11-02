using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Text;
using System.Text.Encodings.Web;

namespace RazorPlus.TagHelpers;

/// <summary>
/// Progress bar component for visualizing completion or loading states.
/// Supports determinate and indeterminate modes, variants, and labels.
/// </summary>
/// <example>
/// <code>
/// &lt;rp-progress value="65" max="100" label="65% Complete" variant="success" /&gt;
/// &lt;rp-progress indeterminate="true" label="Loading..." /&gt;
/// </code>
/// </example>
[HtmlTargetElement("rp-progress", TagStructure = TagStructure.NormalOrSelfClosing)]
public class ProgressTagHelper : TagHelper
{
    /// <summary>
    /// Current progress value
    /// </summary>
    public double Value { get; set; }

    /// <summary>
    /// Maximum value (default: 100)
    /// </summary>
    public double Max { get; set; } = 100;

    /// <summary>
    /// Minimum value (default: 0)
    /// </summary>
    public double Min { get; set; } = 0;

    /// <summary>
    /// Progress bar variant (primary, success, warning, danger, info)
    /// </summary>
    public string Variant { get; set; } = "primary";

    /// <summary>
    /// Show label text
    /// </summary>
    public string? Label { get; set; }

    /// <summary>
    /// Show percentage value
    /// </summary>
    public bool ShowPercentage { get; set; }

    /// <summary>
    /// Indeterminate/loading state (animated)
    /// </summary>
    public bool Indeterminate { get; set; }

    /// <summary>
    /// Progress bar height (default: md)
    /// </summary>
    public string Size { get; set; } = "md"; // sm, md, lg

    /// <summary>
    /// Striped style
    /// </summary>
    public bool Striped { get; set; }

    /// <summary>
    /// Animate stripes
    /// </summary>
    public bool Animated { get; set; }

    /// <summary>
    /// Additional CSS class
    /// </summary>
    public string? CssClass { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        var encoder = HtmlEncoder.Default;
        output.TagName = "div";

        var cssClasses = new List<string> { "rp-progress", $"rp-progress--{Size}" };
        if (!string.IsNullOrWhiteSpace(CssClass)) cssClasses.Add(CssClass);
        output.Attributes.SetAttribute("class", string.Join(" ", cssClasses));

        var content = new StringBuilder();

        // Label (above progress bar)
        if (!string.IsNullOrWhiteSpace(Label) || ShowPercentage)
        {
            content.Append("<div class=\"rp-progress__label\">");

            if (!string.IsNullOrWhiteSpace(Label))
            {
                content.Append($"<span>{encoder.Encode(Label)}</span>");
            }

            if (ShowPercentage && !Indeterminate)
            {
                var percentage = CalculatePercentage();
                content.Append($"<span class=\"rp-progress__percentage\">{percentage:F0}%</span>");
            }

            content.Append("</div>");
        }

        // Progress bar container
        content.Append("<div class=\"rp-progress__track\" role=\"progressbar\"");

        if (!Indeterminate)
        {
            var percentage = CalculatePercentage();
            content.Append($" aria-valuenow=\"{Value}\" aria-valuemin=\"{Min}\" aria-valuemax=\"{Max}\"");
            content.Append($" aria-valuet=\"{percentage:F0}%\"");
        }
        else
        {
            content.Append(" aria-busy=\"true\" aria-valuetext=\"Loading\"");
        }

        content.Append(">");

        // Progress bar fill
        var barClasses = new List<string> { "rp-progress__bar", $"rp-progress__bar--{Variant}" };
        if (Indeterminate) barClasses.Add("rp-progress__bar--indeterminate");
        if (Striped || Animated) barClasses.Add("rp-progress__bar--striped");
        if (Animated) barClasses.Add("rp-progress__bar--animated");

        var widthStyle = Indeterminate ? "" : $" style=\"width: {CalculatePercentage():F2}%\"";

        content.Append($"<div class=\"{string.Join(" ", barClasses)}\"{widthStyle}></div>");

        content.Append("</div>"); // Close track

        output.Content.SetHtmlContent(content.ToString());
    }

    private double CalculatePercentage()
    {
        if (Max == Min) return 0;
        var clamped = Math.Max(Min, Math.Min(Max, Value));
        return ((clamped - Min) / (Max - Min)) * 100;
    }
}
