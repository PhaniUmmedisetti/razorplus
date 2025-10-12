using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Razor.TagHelpers;
using RazorPlus.TagHelpers;
using Xunit;

namespace RazorPlus.Tests;

public class StructureTagHelperTests
{
    [Fact]
    public async Task AccordionItem_Renders_Trigger_And_Panel()
    {
        var contextItems = new Dictionary<object, object?>
        {
            { typeof(AccordionTagHelper), new AccordionTagHelper() }
        };

        var ctx = new TagHelperContext(
            tagName: "rp-accordion-item",
            allAttributes: new TagHelperAttributeList(),
            items: contextItems,
            uniqueId: Guid.NewGuid().ToString());

        var output = new TagHelperOutput("rp-accordion-item",
            attributes: new TagHelperAttributeList(),
            getChildContentAsync: (useCachedResult, encoder) => Task.FromResult(new DefaultTagHelperContent().SetContent("Panel")));

        var item = new AccordionItemTagHelper { Header = "Section" };
        await item.ProcessAsync(ctx, output);

        Assert.Equal("div", output.TagName);
        var html = output.Content.GetContent();
        Assert.Contains("rp-accordion__trigger", html);
        Assert.Contains("rp-accordion__panel", html);
        Assert.Contains("aria-expanded=\"false\"", html);
        Assert.Contains("hidden", html);
    }

    [Fact]
    public async Task Modal_Renders_Dialog_With_Header()
    {
        var ctx = new TagHelperContext(
            tagName: "rp-modal",
            allAttributes: new TagHelperAttributeList(),
            items: new Dictionary<object, object?>(),
            uniqueId: Guid.NewGuid().ToString());

        var output = new TagHelperOutput("rp-modal",
            attributes: new TagHelperAttributeList(),
            getChildContentAsync: (useCachedResult, encoder) => Task.FromResult(new DefaultTagHelperContent().SetContent("Body")));

        var modal = new ModalTagHelper { Title = "Sample" };
        await modal.ProcessAsync(ctx, output);

        Assert.Equal("div", output.TagName);
        Assert.Equal("false", output.Attributes["data-rp-open"].Value.ToString());
        var html = output.Content.GetContent();
        Assert.Contains("rp-modal__dialog", html);
        Assert.Contains("rp-modal__header", html);
    }
}
