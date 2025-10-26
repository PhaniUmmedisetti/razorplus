using System.Collections.Generic;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.AspNetCore.WebUtilities;

namespace RazorPlus.TagHelpers;

/// <summary>
/// Render pagination controls that preserve existing query string parameters.
/// </summary>
[HtmlTargetElement("rp-pagination")]
public class PaginationTagHelper : TagHelper
{
    [HtmlAttributeNotBound]
    [ViewContext]
    public ViewContext ViewContext { get; set; } = default!;

    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 25;
    public int TotalItems { get; set; }
    public string PageParam { get; set; } = "page";

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "nav";
        output.Attributes.SetAttribute("aria-label", "Pagination");
        var existingClass = output.Attributes.ContainsName("class") ? output.Attributes["class"].Value?.ToString() : string.Empty;
        var mergedClass = string.IsNullOrWhiteSpace(existingClass) ? "rp-pagination" : $"{existingClass} rp-pagination";
        output.Attributes.SetAttribute("class", mergedClass);

        if (PageSize <= 0) PageSize = 25;
        var totalPages = (int)System.Math.Ceiling(TotalItems / (double)PageSize);
        if (totalPages <= 1)
        {
            output.SuppressOutput();
            return;
        }

        var request = ViewContext?.HttpContext?.Request;
        var basePath = request?.Path.HasValue == true ? request.Path.Value! : string.Empty;
        var existing = new Dictionary<string, string?>(System.StringComparer.OrdinalIgnoreCase);
        if (request?.Query != null)
        {
            foreach (var kv in request.Query)
            {
                existing[kv.Key] = kv.Value.ToString();
            }
        }

        string BuildUrl(int page)
        {
            var snapshot = new Dictionary<string, string?>(existing, System.StringComparer.OrdinalIgnoreCase)
            {
                [PageParam] = page.ToString()
            };
            return QueryHelpers.AddQueryString(basePath, snapshot);
        }

        var list = new System.Text.StringBuilder();
        list.Append("<ul class=\"rp-pagination__list\">");

        void AppendItem(string label, int page, bool disabled, bool active, string rel)
        {
            list.Append("<li class=\"rp-pagination__item");
            if (disabled) list.Append(" is-disabled");
            if (active) list.Append(" is-active");
            list.Append("\">");
            if (disabled)
            {
                list.Append($"<span>{label}</span>");
            }
            else
            {
                var url = HtmlEncoder.Default.Encode(BuildUrl(page));
                list.Append($"<a href=\"{url}\" rel=\"{rel}\">{label}</a>");
            }
            list.Append("</li>");
        }

        var prevDisabled = Page <= 1;
        AppendItem("Prev", System.Math.Max(1, Page - 1), prevDisabled, false, "prev");

        const int window = 2;
        var start = System.Math.Max(1, Page - window);
        var end = System.Math.Min(totalPages, Page + window);

        if (start > 1)
        {
            AppendItem("1", 1, false, Page == 1, "nofollow");
            if (start > 2)
            {
                list.Append("<li class=\"rp-pagination__ellipsis\">&hellip;</li>");
            }
        }

        for (var i = start; i <= end; i++)
        {
            AppendItem(i.ToString(), i, false, i == Page, "nofollow");
        }

        if (end < totalPages)
        {
            if (end < totalPages - 1)
            {
                list.Append("<li class=\"rp-pagination__ellipsis\">&hellip;</li>");
            }
            AppendItem(totalPages.ToString(), totalPages, false, Page == totalPages, "nofollow");
        }

        var nextDisabled = Page >= totalPages;
        AppendItem("Next", System.Math.Min(totalPages, Page + 1), nextDisabled, false, "next");

        list.Append("</ul>");
        output.Content.SetHtmlContent(list.ToString());
    }
}
