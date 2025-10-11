using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace RazorPlus.TagHelpers;

[HtmlTargetElement("rp-tabs")]
public class TabsTagHelper : TagHelper
{
    public string? Id { get; set; }
    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "div";
        output.Attributes.SetAttribute("class", "rp-tabs");
        output.Attributes.SetAttribute("data-rp-tabs", "");
        if (!string.IsNullOrWhiteSpace(Id))
            output.Attributes.SetAttribute("id", Id);
        output.TagMode = TagMode.StartTagAndEndTag;
    }
}

[HtmlTargetElement("rp-tab", ParentTag = "rp-tabs")]
public class TabTagHelper : TagHelper
{
    public string? Id { get; set; }
    public string? Header { get; set; }
    public bool Active { get; set; }

    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        var enc = HtmlEncoder.Default;
        var content = await output.GetChildContentAsync();
        var id = string.IsNullOrWhiteSpace(Id) ? Guid.NewGuid().ToString("N") : Id!;
        var header = Header ?? id;

        // Render as a section container; core.js will synthesize tablist/buttons at runtime for roving tabindex
        output.TagName = "section";
        output.Attributes.SetAttribute("class", Active ? "rp-tab rp-tab--active" : "rp-tab");
        output.Attributes.SetAttribute("role", "tabpanel");
        output.Attributes.SetAttribute("id", id);
        output.Attributes.SetAttribute("aria-labelledby", $"tab-{id}");
        output.PreElement.SetHtmlContent($"<template data-rp-tab-header id=\"tab-{id}\">{enc.Encode(header)}</template>");
        output.Content.SetHtmlContent(content);
    }
}

