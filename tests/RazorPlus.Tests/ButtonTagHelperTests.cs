using System.Text.Encodings.Web;
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
}

