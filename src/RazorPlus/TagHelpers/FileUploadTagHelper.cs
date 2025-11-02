using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Text;
using System.Text.Encodings.Web;

namespace RazorPlus.TagHelpers;

/// <summary>
/// File upload component with drag-and-drop, preview, and validation.
/// Supports single/multiple files, image previews, and progress tracking.
/// </summary>
/// <example>
/// <code>
/// &lt;rp-file-upload name="Documents"
///                 label="Upload Documents"
///                 accept=".pdf,.doc,.docx"
///                 max-size="10485760"
///                 max-files="5"
///                 multiple="true"
///                 preview="true"
///                 hint="Maximum 5 files, 10MB each" /&gt;
/// </code>
/// </example>
[HtmlTargetElement("rp-file-upload", TagStructure = TagStructure.NormalOrSelfClosing)]
public class FileUploadTagHelper : TagHelper
{
    private readonly IHtmlGenerator _generator;

    public FileUploadTagHelper(IHtmlGenerator generator)
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
    /// Input name attribute
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Label text
    /// </summary>
    public string? Label { get; set; }

    /// <summary>
    /// Help text displayed below the upload area
    /// </summary>
    public string? Hint { get; set; }

    /// <summary>
    /// Whether the field is required
    /// </summary>
    public bool Required { get; set; }

    /// <summary>
    /// Allow multiple file selection
    /// </summary>
    public bool Multiple { get; set; }

    /// <summary>
    /// File type restrictions (e.g., ".pdf,.doc,.docx" or "image/*")
    /// </summary>
    public string? Accept { get; set; }

    /// <summary>
    /// Maximum file size in bytes (default: 5MB)
    /// </summary>
    public long MaxSize { get; set; } = 5242880; // 5MB

    /// <summary>
    /// Maximum number of files (only applies when Multiple=true)
    /// </summary>
    public int MaxFiles { get; set; } = 10;

    /// <summary>
    /// Show image previews for image files
    /// </summary>
    public bool Preview { get; set; } = true;

    /// <summary>
    /// Drag-and-drop area text
    /// </summary>
    public string? DropText { get; set; }

    /// <summary>
    /// Browse button text
    /// </summary>
    public string? BrowseText { get; set; }

    /// <summary>
    /// Upload endpoint URL (for AJAX uploads)
    /// </summary>
    public string? UploadUrl { get; set; }

    /// <summary>
    /// Additional CSS class
    /// </summary>
    public string? CssClass { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        var encoder = HtmlEncoder.Default;
        output.TagName = "div";

        var cssClasses = new List<string> { "rp-field", "rp-file-upload" };
        if (!string.IsNullOrWhiteSpace(CssClass)) cssClasses.Add(CssClass);
        output.Attributes.SetAttribute("class", string.Join(" ", cssClasses));

        var name = Name ?? For?.Name ?? "file";
        var id = $"rp-file-upload-{name}";
        var labelText = Label ?? For?.Metadata?.DisplayName ?? "Upload File";
        var dropText = DropText ?? (Multiple ? "Drag files here or click to browse" : "Drag file here or click to browse");
        var browseText = BrowseText ?? "Browse";

        var content = new StringBuilder();

        // Label
        if (!string.IsNullOrWhiteSpace(labelText))
        {
            content.Append($"<label class=\"rp-label\" for=\"{id}\">");
            content.Append(encoder.Encode(labelText));
            if (Required)
            {
                content.Append("<span class=\"rp-label__required\" aria-label=\"required\">*</span>");
            }
            content.Append("</label>");
        }

        // File upload container with data attributes for JavaScript
        content.Append($"<div class=\"rp-file-upload__container\" data-rp-file-upload");
        content.Append($" data-max-size=\"{MaxSize}\"");
        content.Append($" data-max-files=\"{MaxFiles}\"");
        if (!string.IsNullOrWhiteSpace(Accept))
        {
            content.Append($" data-accept=\"{encoder.Encode(Accept)}\"");
        }
        if (Preview)
        {
            content.Append(" data-preview=\"true\"");
        }
        if (!string.IsNullOrWhiteSpace(UploadUrl))
        {
            content.Append($" data-upload-url=\"{encoder.Encode(UploadUrl)}\"");
        }
        content.Append(">");

        // Drop zone
        content.Append("<div class=\"rp-file-upload__dropzone\" tabindex=\"0\" role=\"button\" aria-label=\"File upload area\">");
        content.Append("<div class=\"rp-file-upload__dropzone-content\">");
        content.Append("<span class=\"rp-file-upload__icon\" aria-hidden=\"true\">📁</span>");
        content.Append($"<div class=\"rp-file-upload__text\">{encoder.Encode(dropText)}</div>");
        content.Append($"<button type=\"button\" class=\"rp-btn rp-btn--primary rp-btn--sm rp-file-upload__browse\">{encoder.Encode(browseText)}</button>");
        content.Append("</div>");
        content.Append("</div>");

        // Hidden file input
        var inputAttrs = new Dictionary<string, object?>
        {
            ["type"] = "file",
            ["id"] = id,
            ["name"] = name,
            ["class"] = "rp-file-upload__input",
            ["style"] = "display: none;",
            ["aria-hidden"] = "true"
        };

        if (Multiple) inputAttrs["multiple"] = "multiple";
        if (Required) inputAttrs["required"] = "required";
        if (!string.IsNullOrWhiteSpace(Accept)) inputAttrs["accept"] = Accept;

        var input = _generator.GenerateTextBox(
            ViewContext!,
            For?.ModelExplorer,
            For?.Name ?? name,
            null,
            null,
            inputAttrs
        );

        using var inputWriter = new StringWriter();
        input.WriteTo(inputWriter, encoder);
        var inputHtml = inputWriter.ToString().Replace("type=\"text\"", "type=\"file\"");
        content.Append(inputHtml);

        // File list preview area
        content.Append("<div class=\"rp-file-upload__files\" role=\"list\" aria-live=\"polite\"></div>");

        content.Append("</div>"); // Close container

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
