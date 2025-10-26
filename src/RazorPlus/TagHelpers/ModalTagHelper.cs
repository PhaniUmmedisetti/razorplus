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

    public override void Init(TagHelperContext context)
    {
        var modalContext = new ModalSections();
        context.Items[typeof(ModalSections)] = modalContext;
    }

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

        var sections = context.Items.TryGetValue(typeof(ModalSections), out var stored) && stored is ModalSections s ? s : new ModalSections();
        var titleText = string.IsNullOrWhiteSpace(Title) ? "Dialog" : Title!;

        var headerContent = sections.Header;
        string header;
        if (headerContent != null)
        {
            var accessible = $"<span id=\"{titleId}\" class=\"rp-sr-only\">{enc.Encode(titleText)}</span>";
            header = $"<header class=\"rp-modal__header\">{headerContent.GetContent()}{accessible}<button type=\"button\" class=\"rp-modal__close\" data-rp-modal-close aria-label=\"Close dialog\">&#215;</button></header>";
        }
        else
        {
            var defaultHeader = $"<h2 class=\"rp-modal__title\" id=\"{titleId}\">{enc.Encode(titleText)}</h2>";
            header = $"<header class=\"rp-modal__header\">{defaultHeader}<button type=\"button\" class=\"rp-modal__close\" data-rp-modal-close aria-label=\"Close dialog\">&#215;</button></header>";
        }

        var bodyContent = sections.Body ?? content;
        var body = $"<div class=\"rp-modal__body\">{bodyContent.GetContent()}</div>";

        var footer = sections.Footer != null
            ? $"<footer class=\"rp-modal__footer\">{sections.Footer.GetContent()}</footer>"
            : string.Empty;

        var dialog = $"<div class=\"rp-modal__dialog\" role=\"dialog\" aria-modal=\"true\" aria-labelledby=\"{titleId}\" tabindex=\"-1\">{header}{body}{footer}</div>";

        var overlayAttrs = StaticBackdrop ? " data-static=\"true\"" : string.Empty;
        var overlay = $"<div class=\"rp-modal__overlay\" data-rp-modal-overlay{overlayAttrs}></div>";

        output.Content.SetHtmlContent(overlay + dialog);
    }
}

public class ModalSections
{
    public TagHelperContent? Header { get; set; }
    public TagHelperContent? Body { get; set; }
    public TagHelperContent? Footer { get; set; }
}

[HtmlTargetElement("rp-modal-header", ParentTag = "rp-modal")]
public class ModalHeaderTagHelper : TagHelper
{
    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        if (context.Items.TryGetValue(typeof(ModalSections), out var stored) && stored is ModalSections sections)
        {
            sections.Header = await output.GetChildContentAsync();
        }
        output.SuppressOutput();
    }
}

[HtmlTargetElement("rp-modal-body", ParentTag = "rp-modal")]
public class ModalBodyTagHelper : TagHelper
{
    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        if (context.Items.TryGetValue(typeof(ModalSections), out var stored) && stored is ModalSections sections)
        {
            sections.Body = await output.GetChildContentAsync();
        }
        output.SuppressOutput();
    }
}

[HtmlTargetElement("rp-modal-footer", ParentTag = "rp-modal")]
public class ModalFooterTagHelper : TagHelper
{
    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        if (context.Items.TryGetValue(typeof(ModalSections), out var stored) && stored is ModalSections sections)
        {
            sections.Footer = await output.GetChildContentAsync();
        }
        output.SuppressOutput();
    }
}
