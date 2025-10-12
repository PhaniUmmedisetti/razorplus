using System.Collections.Generic;
using Microsoft.AspNetCore.Razor.TagHelpers;
using RazorPlus.TagHelpers;
using Xunit;

namespace RazorPlus.Tests;

public class ButtonTagHelperTests
{
    [Fact]
    public async Task Renders_Button_With_Classes()
    {
        var ctx = new TagHelperContext(
            tagName: "rp-button",
            allAttributes: new TagHelperAttributeList(),
            items: new Dictionary<object, object?>(),
            uniqueId: "test");

        var output = new TagHelperOutput("rp-button",
            attributes: new TagHelperAttributeList(),
            getChildContentAsync: (useCachedResult, encoder) => Task.FromResult(new DefaultTagHelperContent().SetContent("Save")));

        var tag = new ButtonTagHelper { Variant = "primary", Size = "md" };
        await tag.ProcessAsync(ctx, output);

        var html = output.PreContent.GetContent() + output.Content.GetContent() + output.PostContent.GetContent();
        Assert.Equal("button", output.TagName);
        Assert.Contains("rp-btn", output.Attributes["class"].Value.ToString());
        Assert.Contains("rp-btn--primary", output.Attributes["class"].Value.ToString());
    }

    [Fact]
    public async Task Honors_Button_Type()
    {
        var ctx = new TagHelperContext(
            tagName: "rp-button",
            allAttributes: new TagHelperAttributeList(new[] { new TagHelperAttribute("type", "submit") }),
            items: new Dictionary<object, object?>(),
            uniqueId: Guid.NewGuid().ToString());

        var output = new TagHelperOutput("rp-button",
            attributes: new TagHelperAttributeList(new[] { new TagHelperAttribute("type", "submit") }),
            getChildContentAsync: (useCachedResult, encoder) => Task.FromResult(new DefaultTagHelperContent().SetContent("Submit")));

        var tag = new ButtonTagHelper { Variant = "primary", ButtonType = "submit" };
        await tag.ProcessAsync(ctx, output);

        Assert.Equal("button", output.TagName);
        Assert.Equal("submit", output.Attributes["type"].Value.ToString());
    }
}

