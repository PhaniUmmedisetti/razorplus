using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Text.Encodings.Web;

namespace RazorPlus.TagHelpers;

/// <summary>
/// Renders an entity picker with popup modal selection.
/// Useful for selecting related entities like products, customers, locations, equipment, etc.
/// </summary>
/// <example>
/// <code>
/// &lt;rp-popup-selector id="ProductPopup"
///                     label="Product Name"
///                     required="true"
///                     modal-id="ProductModal"
///                     display-text="@Model.SelectedProductName"
///                     value="@Model.ProductId"
///                     name="ProductId"&gt;
/// &lt;/rp-popup-selector&gt;
/// </code>
/// </example>
[HtmlTargetElement("rp-popup-selector", TagStructure = TagStructure.NormalOrSelfClosing)]
public class PopupSelectorTagHelper : TagHelper
{
    private readonly IHtmlHelper _htmlHelper;

    [HtmlAttributeNotBound]
    [ViewContext]
    public ViewContext? ViewContext { get; set; }

    /// <summary>
    /// Unique identifier for the popup selector
    /// </summary>
    public string? Id { get; set; }

    /// <summary>
    /// Label text for the field
    /// </summary>
    public string? Label { get; set; }

    /// <summary>
    /// Whether the field is required
    /// </summary>
    public bool Required { get; set; }

    /// <summary>
    /// ID of the modal dialog that will be opened for selection
    /// </summary>
    public string? ModalId { get; set; }

    /// <summary>
    /// Currently selected value display text
    /// </summary>
    public string? DisplayText { get; set; }

    /// <summary>
    /// Currently selected value
    /// </summary>
    public string? Value { get; set; }

    /// <summary>
    /// Name attribute for the hidden input field
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Hint text below the field
    /// </summary>
    public string? Hint { get; set; }

    /// <summary>
    /// Button text (default: "Add Item")
    /// </summary>
    public string? ButtonText { get; set; }

    /// <summary>
    /// Icon class for the button (e.g., "ft-plus")
    /// </summary>
    public string? ButtonIcon { get; set; }

    /// <summary>
    /// JavaScript function to call when clearing selection
    /// </summary>
    public string? OnClear { get; set; }

    /// <summary>
    /// Additional CSS class
    /// </summary>
    public string? CssClass { get; set; }

    public PopupSelectorTagHelper(IHtmlHelper htmlHelper)
    {
        _htmlHelper = htmlHelper;
    }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        if (_htmlHelper is IViewContextAware contextAware && ViewContext != null)
        {
            contextAware.Contextualize(ViewContext);
        }

        var encoder = HtmlEncoder.Default;
        output.TagName = "div";

        var cssClasses = new List<string> { "rp-field", "rp-popup-selector" };
        if (!string.IsNullOrWhiteSpace(CssClass))
        {
            cssClasses.Add(CssClass);
        }
        output.Attributes.SetAttribute("class", string.Join(" ", cssClasses));

        var content = new StringWriter();

        // Label
        if (!string.IsNullOrWhiteSpace(Label))
        {
            content.Write($"<label class=\"rp-label title-heading\">{encoder.Encode(Label)}</label>");
            if (Required)
            {
                content.Write("<span class=\"rp-required required\" style=\"color: var(--rp-danger, #dc2626); margin-left: 2px;\">*</span>");
            }
        }

        // Selected value display area
        var divId = !string.IsNullOrWhiteSpace(Id) ? $"{Id}Div" : $"popup{Guid.NewGuid():N}Div";
        content.Write($"<div id=\"{divId}\" class=\"rp-popup-label\">");

        if (!string.IsNullOrWhiteSpace(DisplayText) && !string.IsNullOrWhiteSpace(Value))
        {
            var clearHandler = !string.IsNullOrWhiteSpace(OnClear)
                ? OnClear
                : $"document.getElementById('{divId}').innerHTML = ''; document.getElementById('{Name ?? Id}').value = '';";

            content.Write($@"
                <div class=""rp-popup-value-item"">
                    <span class=""rp-popup-value-item-text"" title=""{encoder.Encode(DisplayText)}"">{encoder.Encode(DisplayText)}</span>
                    <span class=""rp-popup-value-item-close"" onclick=""{encoder.Encode(clearHandler)}"">&times;</span>
                </div>
            ");
        }

        content.Write("</div>");

        // Button to open modal
        var modalTarget = !string.IsNullOrWhiteSpace(ModalId) ? ModalId : "modal";
        var btnIcon = !string.IsNullOrWhiteSpace(ButtonIcon) ? ButtonIcon : "plus";
        var btnText = !string.IsNullOrWhiteSpace(ButtonText) ? ButtonText : "Add Item";

        content.Write($@"
            <button type=""button""
                    id=""{Id}Btn""
                    class=""rp-btn-popup btn-popup""
                    data-bs-toggle=""modal""
                    data-bs-target=""#{modalTarget}""
                    onclick=""if(typeof RazorPlus !== 'undefined' && RazorPlus.openModal) {{ RazorPlus.openModal('{modalTarget}'); }}"">
                <span aria-hidden=""true"" data-icon=""{encoder.Encode(btnIcon)}""></span>
                <span class=""add-item"">{encoder.Encode(btnText)}</span>
            </button>
        ");

        // Hidden input to store selected value
        var inputName = !string.IsNullOrWhiteSpace(Name) ? Name : Id ?? "selectedValue";
        var inputId = Name ?? Id ?? $"hidden{Guid.NewGuid():N}";
        content.Write($"<input type=\"hidden\" id=\"{inputId}\" name=\"{inputName}\" value=\"{encoder.Encode(Value ?? string.Empty)}\" />");

        // Hint text
        if (!string.IsNullOrWhiteSpace(Hint))
        {
            content.Write($"<div class=\"rp-hint\">{encoder.Encode(Hint)}</div>");
        }

        output.Content.SetHtmlContent(content.ToString());
    }
}
