using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Text.Encodings.Web;

namespace RazorPlus.TagHelpers;

/// <summary>
/// Renders a workflow stage container with colored background.
/// Useful for transaction workflows, manufacturing processes, and multi-step forms.
/// </summary>
/// <example>
/// <code>
/// &lt;rp-stage-divider stage="1" title="Preparation"&gt;
///   &lt;div class="row"&gt;
///     &lt;!-- Form fields for this stage --&gt;
///   &lt;/div&gt;
/// &lt;/rp-stage-divider&gt;
/// </code>
/// </example>
[HtmlTargetElement("rp-stage-divider", TagStructure = TagStructure.NormalOrSelfClosing)]
public class StageDividerTagHelper : TagHelper
{
    /// <summary>
    /// Stage number (1-8) which determines the background color.
    /// 1 = Preparation (lightcyan)
    /// 2 = Issuance (peachpuff)
    /// 3 = Execution (lightblue)
    /// 4 = Closure (lemonchiffon)
    /// 5 = Equipment (LightBlue)
    /// 6 = Location (PowderBlue)
    /// 7 = Workflow (yellow)
    /// 8 = Dispensing (Thistle)
    /// </summary>
    public int? Stage { get; set; }

    /// <summary>
    /// Title displayed as a data attribute (can be styled with CSS ::before)
    /// </summary>
    public string? Title { get; set; }

    /// <summary>
    /// Optional CSS class to add
    /// </summary>
    public string? CssClass { get; set; }

    /// <summary>
    /// Custom background color (overrides stage-based color)
    /// </summary>
    public string? BackgroundColor { get; set; }

    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "div";

        // Build CSS classes
        var cssClasses = new List<string> { "rp-stage-divider" };

        if (Stage.HasValue && Stage.Value >= 1 && Stage.Value <= 8)
        {
            cssClasses.Add($"rp-stage-divider--{Stage.Value}");
        }

        if (!string.IsNullOrWhiteSpace(CssClass))
        {
            cssClasses.Add(CssClass);
        }

        output.Attributes.SetAttribute("class", string.Join(" ", cssClasses));

        // Add title as data attribute
        if (!string.IsNullOrWhiteSpace(Title))
        {
            output.Attributes.SetAttribute("data-title", Title);
        }

        // Add custom background color if specified
        if (!string.IsNullOrWhiteSpace(BackgroundColor))
        {
            var style = $"background-color: {BackgroundColor};";
            output.Attributes.SetAttribute("style", style);
        }

        // Get child content
        var childContent = await output.GetChildContentAsync();
        output.Content.SetHtmlContent(childContent);
    }
}
