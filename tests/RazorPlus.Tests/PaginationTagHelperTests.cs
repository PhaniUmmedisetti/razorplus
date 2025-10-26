using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using RazorPlus.TagHelpers;
using Xunit;

namespace RazorPlus.Tests;

public class PaginationTagHelperTests
{
    [Fact]
    public void SuppressesOutput_WhenSinglePage()
    {
        var tag = new PaginationTagHelper
        {
            ViewContext = new ViewContext { HttpContext = new DefaultHttpContext() },
            TotalItems = 5,
            PageSize = 10
        };

        var ctx = new TagHelperContext("rp-pagination", new TagHelperAttributeList(), new Dictionary<object, object?>(), "pagination");
        var output = new TagHelperOutput("rp-pagination", new TagHelperAttributeList(), (useCached, encoder) => Task.FromResult<TagHelperContent>(new DefaultTagHelperContent()));

        tag.Process(ctx, output);

        Assert.Null(output.TagName);
    }

    [Fact]
    public void RendersLinks_ForMultiplePages()
    {
        var httpContext = new DefaultHttpContext();
        httpContext.Request.Path = "/docs";
        httpContext.Request.QueryString = new QueryString("?page=2&sort=name");

        var tag = new PaginationTagHelper
        {
            ViewContext = new ViewContext { HttpContext = httpContext },
            TotalItems = 50,
            PageSize = 10,
            Page = 2
        };

        var ctx = new TagHelperContext("rp-pagination", new TagHelperAttributeList(), new Dictionary<object, object?>(), "pagination");
        var output = new TagHelperOutput("rp-pagination", new TagHelperAttributeList(), (useCached, encoder) => Task.FromResult<TagHelperContent>(new DefaultTagHelperContent()));

        tag.Process(ctx, output);

        var html = output.Content.GetContent();
        Assert.Contains("Prev", html);
        Assert.Contains("Next", html);
        Assert.Contains("page=1", html);
        Assert.Contains("page=5", html);
    }
}
