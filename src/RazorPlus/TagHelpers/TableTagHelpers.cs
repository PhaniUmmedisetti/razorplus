using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.AspNetCore.WebUtilities;

namespace RazorPlus.TagHelpers;

[HtmlTargetElement("rp-table")]
public class TableTagHelper : TagHelper
{
    [HtmlAttributeNotBound]
    [ViewContext]
    public ViewContext ViewContext { get; set; } = default!;

    public IEnumerable<object>? Items { get; set; }
    public bool Sortable { get; set; }
    public bool Pageable { get; set; }
    public bool Client { get; set; }
    public int PageSize { get; set; } = 25;
    public int Page { get; set; } = 1;
    public int TotalItems { get; set; }
    public string SortParam { get; set; } = "sort";
    public string DirectionParam { get; set; } = "dir";
    public string PageParam { get; set; } = "page";
    public string EmptyText { get; set; } = "No records found.";
    public string? KeySelector { get; set; }

    /// <summary>
    /// Enable row selection with checkboxes
    /// </summary>
    public bool Selectable { get; set; }

    /// <summary>
    /// Enable row click handlers
    /// </summary>
    public bool RowClickable { get; set; }

    /// <summary>
    /// JavaScript function name for row click handler
    /// </summary>
    public string? OnRowClick { get; set; }

    /// <summary>
    /// Hover effect on rows
    /// </summary>
    public bool Hoverable { get; set; } = true;

    // server-only MVP: column definitions by child tag helpers
    public List<ColumnDefinition> Columns { get; } = new();

    public override void Init(TagHelperContext context)
    {
        // allow child rp-column to discover the parent instance
        context.Items[typeof(TableTagHelper)] = this;
    }

    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "table";
        var classes = "rp-table";
        if (Client) classes += " rp-table--client";
        if (Hoverable) classes += " rp-table--hoverable";
        if (RowClickable) classes += " rp-table--clickable";
        output.Attributes.SetAttribute("class", classes);
        output.Attributes.SetAttribute("data-rp-table", string.Empty);
        if (Client)
        {
            output.Attributes.SetAttribute("data-rp-table-client", "true");
        }
        if (Pageable)
        {
            output.Attributes.SetAttribute("data-rp-table-pageable", "true");
            output.Attributes.SetAttribute("data-rp-page-size", PageSize.ToString());
            output.Attributes.SetAttribute("data-rp-page", Page.ToString());
            output.Attributes.SetAttribute("data-rp-total", TotalItems.ToString());
        }
        if (Selectable)
        {
            output.Attributes.SetAttribute("data-rp-table-selectable", "true");
        }
        if (RowClickable && !string.IsNullOrWhiteSpace(OnRowClick))
        {
            output.Attributes.SetAttribute("data-rp-row-click", OnRowClick);
        }
        output.TagMode = TagMode.StartTagAndEndTag;
        output.PreElement.SetHtmlContent("<div class=\"rp-table-container\">");
        output.PostElement.SetHtmlContent("</div>");

        // allow child <rp-column> to register
        await output.GetChildContentAsync();

        // header
        var request = ViewContext?.HttpContext?.Request;
        var currentQuery = request?.Query ?? default;
        var currentSort = GetQueryValue(currentQuery, SortParam);
        var currentDir = GetQueryValue(currentQuery, DirectionParam) ?? "asc";
        var basePath = request?.Path.HasValue == true ? request.Path.Value! : string.Empty;

        output.Content.AppendHtml("<thead><tr>");

        // Add selection checkbox column header
        if (Selectable)
        {
            output.Content.AppendHtml("<th class=\"rp-table__select-cell\">");
            output.Content.AppendHtml("<input type=\"checkbox\" class=\"rp-table__select-all\" aria-label=\"Select all rows\" />");
            output.Content.AppendHtml("</th>");
        }

        var columnIndex = 0;
        foreach (var c in Columns)
        {
            var thBuilder = new TagBuilder("th");
            thBuilder.Attributes["data-rp-column-index"] = columnIndex.ToString();
            if (!string.IsNullOrWhiteSpace(c.Width))
            {
                thBuilder.Attributes["style"] = $"width:{c.Width}";
            }
            var cellClasses = new List<string>();
            if (!string.IsNullOrWhiteSpace(c.Align))
            {
                cellClasses.Add($"rp-table__cell--{c.Align}");
            }
            if (cellClasses.Count > 0)
            {
                thBuilder.AddCssClass(string.Join(" ", cellClasses));
            }

            var headerText = c.Header ?? c.For ?? string.Empty;
            if (Sortable && c.EnableSort && !string.IsNullOrWhiteSpace(c.SortKey))
            {
                if (Client)
                {
                    thBuilder.Attributes["data-rp-sort-key"] = c.SortKey;
                    thBuilder.Attributes["data-rp-sortable"] = "true";
                    var button = new TagBuilder("button");
                    button.Attributes["type"] = "button";
                    button.AddCssClass("rp-table__sort");
                    button.InnerHtml.Append(headerText ?? string.Empty);
                    var indicator = new TagBuilder("span");
                    indicator.AddCssClass("rp-table__sort-indicator");
                    indicator.Attributes["aria-hidden"] = "true";
                    button.InnerHtml.AppendHtml(indicator);
                    thBuilder.InnerHtml.AppendHtml(button);
                }
                else
                {
                    var isActive = string.Equals(currentSort, c.SortKey, StringComparison.OrdinalIgnoreCase);
                    var nextDir = isActive && string.Equals(currentDir, "asc", StringComparison.OrdinalIgnoreCase) ? "desc" : "asc";
                    var queryDict = new Dictionary<string, string?>(StringComparer.OrdinalIgnoreCase);
                    if (currentQuery != null)
                    {
                        foreach (var kv in currentQuery)
                        {
                            queryDict[kv.Key] = kv.Value.ToString();
                        }
                    }
                    queryDict[SortParam] = c.SortKey;
                    queryDict[DirectionParam] = nextDir;
                    queryDict[PageParam] = "1";

                    var sortUrl = QueryHelpers.AddQueryString(basePath, queryDict!);
                    var link = new TagBuilder("a");
                    link.Attributes["href"] = sortUrl;
                    link.AddCssClass("rp-table__sort");
                    if (isActive)
                    {
                        link.AddCssClass($"rp-table__sort--{currentDir.ToLowerInvariant()}");
                    }
                    link.InnerHtml.Append(headerText ?? string.Empty);
                    var indicator = new TagBuilder("span");
                    indicator.AddCssClass("rp-table__sort-indicator");
                    indicator.Attributes["aria-hidden"] = "true";
                    link.InnerHtml.AppendHtml(indicator);
                    thBuilder.InnerHtml.AppendHtml(link);
                }
            }
            else
            {
                thBuilder.InnerHtml.Append(headerText ?? string.Empty);
            }

            output.Content.AppendHtml(RenderTagBuilder(thBuilder));
            columnIndex++;
        }
        output.Content.AppendHtml("</tr></thead>");

        output.Content.AppendHtml("<tbody>");
        if (Items != null && Items.Any())
        {
            foreach (var item in Items)
            {
                var tr = new TagBuilder("tr");
                if (!string.IsNullOrWhiteSpace(KeySelector))
                {
                    var key = ResolvePropertyRaw(item, KeySelector);
                    if (key != null)
                    {
                        tr.Attributes["data-rp-row-key"] = key.ToString();
                    }
                }

                if (RowClickable)
                {
                    tr.AddCssClass("rp-table__row--clickable");
                }

                // Add selection checkbox cell
                if (Selectable)
                {
                    var checkboxTd = new TagBuilder("td");
                    checkboxTd.AddCssClass("rp-table__select-cell");
                    var checkbox = new TagBuilder("input");
                    checkbox.Attributes["type"] = "checkbox";
                    checkbox.AddCssClass("rp-table__select-row");
                    checkbox.Attributes["aria-label"] = "Select row";
                    checkboxTd.InnerHtml.AppendHtml(checkbox);
                    tr.InnerHtml.AppendHtml(checkboxTd);
                }

                foreach (var c in Columns)
                {
                    var td = new TagBuilder("td");
                    if (!string.IsNullOrWhiteSpace(c.Align))
                    {
                        td.AddCssClass($"rp-table__cell--{c.Align}");
                    }

                    var value = c.Template != null ? c.Template(item) : ResolvePropertyRaw(item, c.For);
                    if (value is IHtmlContent htmlContent)
                    {
                        td.InnerHtml.AppendHtml(htmlContent);
                    }
                    else if (value != null)
                    {
                        td.InnerHtml.Append(value.ToString() ?? string.Empty);
                    }
                    tr.InnerHtml.AppendHtml(td);
                }

                output.Content.AppendHtml(RenderTagBuilder(tr));
            }
        }
        else
        {
            var emptyRow = new TagBuilder("tr");
            var td = new TagBuilder("td");
            var colspan = Columns.Count;
            if (Selectable) colspan++; // Account for checkbox column
            td.Attributes["colspan"] = colspan > 0 ? colspan.ToString() : "1";
            td.AddCssClass("rp-table__empty");
            td.InnerHtml.Append(EmptyText);
            emptyRow.InnerHtml.AppendHtml(td);
            output.Content.AppendHtml(RenderTagBuilder(emptyRow));
        }
        output.Content.AppendHtml("</tbody>");
    }

    private static string? GetQueryValue(IQueryCollection? query, string key)
    {
        if (query == null) return null;
        return query.TryGetValue(key, out var value) ? value.ToString() : null;
    }

    private static object? ResolvePropertyRaw(object obj, string? name)
    {
        if (obj == null || string.IsNullOrWhiteSpace(name)) return null;
        var t = obj.GetType();
        var p = t.GetProperty(name, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
        var v = p?.GetValue(obj);
        return v;
    }

    private static string RenderTagBuilder(TagBuilder builder)
    {
        using var writer = new System.IO.StringWriter();
        builder.WriteTo(writer, HtmlEncoder.Default);
        return writer.ToString();
    }
}

[HtmlTargetElement("rp-column", ParentTag = "rp-table")]
public class ColumnTagHelper : TagHelper
{
    public string? For { get; set; }
    public string? Header { get; set; }
    public string? Width { get; set; }
    public string? Align { get; set; }
    public bool Sortable { get; set; }
    public string? SortKey { get; set; }
    public Func<object, object?>? Template { get; set; }

    public override void Init(TagHelperContext context)
    {
        if (context.Items.TryGetValue(typeof(TableTagHelper), out var parent) && parent is TableTagHelper table)
        {
            var definition = new ColumnDefinition
            {
                For = For,
                Header = Header,
                Width = Width,
                Align = Align,
                EnableSort = Sortable,
                SortKey = !string.IsNullOrWhiteSpace(SortKey) ? SortKey : For,
                Template = Template
            };
            table.Columns.Add(definition);
        }
    }

    public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        // columns are declarative only; no direct output
        output.SuppressOutput();
        return Task.CompletedTask;
    }
}

public class ColumnDefinition
{
    public string? For { get; set; }
    public string? Header { get; set; }
    public string? Width { get; set; }
    public string? Align { get; set; }
    public bool EnableSort { get; set; }
    public string? SortKey { get; set; }
    public Func<object, object?>? Template { get; set; }
}
