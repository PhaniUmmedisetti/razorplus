using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace RazorPlus.TagHelpers;

/// <summary>
/// <rp-modal id title open>
/// </summary>
[HtmlTargetElement("rp-modal")]
public class ModalTagHelper : TagHelper
{
    public string? Id { get; set; }
    public string? Title { get; set; }
    public bool Open { get; set; }
    public bool StaticBackdrop { get; set; }

    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        var enc = HtmlEncoder.Default;
        var content = await output.GetChildContentAsync();
        var modalId = string.IsNullOrWhiteSpace(Id) ? $"rp-modal-{Guid.NewGuid():N}" : Id!;
        var titleId = $"{modalId}-title";

        output.TagName = "div";
        output.TagMode = TagMode.StartTagAndEndTag;
        var classes = "rp-modal";
        if (Open)
        {
            classes += " rp-modal--open";
        }
        output.Attributes.SetAttribute("class", classes);
        output.Attributes.SetAttribute("id", modalId);
        output.Attributes.SetAttribute("data-rp-modal", string.Empty);
        output.Attributes.SetAttribute("data-rp-open", Open ? "true" : "false");
        if (StaticBackdrop)
        {
            output.Attributes.SetAttribute("data-rp-modal-static", "true");
        }
        if (!Open)
        {
            output.Attributes.SetAttribute("hidden", "hidden");
        }

        var titleText = string.IsNullOrWhiteSpace(Title) ? "Dialog" : Title!;

        var header = $"<header class=\"rp-modal__header\"><h2 class=\"rp-modal__title\" id=\"{titleId}\">{enc.Encode(titleText)}</h2><button type=\"button\" class=\"rp-modal__close\" data-rp-modal-close aria-label=\"Close dialog\">&#215;</button></header>";
        var body = $"<div class=\"rp-modal__body\">{content.GetContent()}</div>";

        var dialog = $"<div class=\"rp-modal__dialog\" role=\"dialog\" aria-modal=\"true\" aria-labelledby=\"{titleId}\" tabindex=\"-1\">{header}{body}</div>";

        var overlayAttrs = StaticBackdrop ? " data-static=\"true\"" : string.Empty;
        var overlay = $"<div class=\"rp-modal__overlay\" data-rp-modal-overlay{overlayAttrs}></div>";

        output.Content.SetHtmlContent(overlay + dialog);
    }
}
