using System.Reflection;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace RazorPlus.TagHelpers;

[HtmlTargetElement("rp-table")]
public class TableTagHelper : TagHelper
{
    public IEnumerable<object>? Items { get; set; }
    public bool Sortable { get; set; }
    public bool Pageable { get; set; }
    public int PageSize { get; set; } = 25;

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
        output.Attributes.SetAttribute("class", "rp-table");
        output.TagMode = TagMode.StartTagAndEndTag;

        // allow child <rp-column> to register
        await output.GetChildContentAsync();

        // header
        output.Content.AppendHtml("<thead><tr>");
        foreach (var c in Columns)
        {
            var th = $"<th{(c.Width != null ? $" style=\\\"width:{c.Width}\\\"" : "")}>{(c.Header ?? c.For ?? "")}</th>";
            output.Content.AppendHtml(th);
        }
        output.Content.AppendHtml("</tr></thead>");

        output.Content.AppendHtml("<tbody>");
        if (Items != null)
        {
            foreach (var item in Items)
            {
                output.Content.AppendHtml("<tr>");
                foreach (var c in Columns)
                {
                    var val = ResolveProperty(item, c.For);
                    output.Content.AppendHtml($"<td>{val}</td>");
                }
                output.Content.AppendHtml("</tr>");
            }
        }
        output.Content.AppendHtml("</tbody>");
    }

    private static string ResolveProperty(object obj, string? name)
    {
        if (obj == null || string.IsNullOrWhiteSpace(name)) return string.Empty;
        var t = obj.GetType();
        var p = t.GetProperty(name, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
        var v = p?.GetValue(obj);
        return v?.ToString() ?? string.Empty;
    }
}

[HtmlTargetElement("rp-column", ParentTag = "rp-table")]
public class ColumnTagHelper : TagHelper
{
    public string? For { get; set; }
    public string? Header { get; set; }
    public string? Width { get; set; }

    public override void Init(TagHelperContext context)
    {
        if (context.Items.TryGetValue(typeof(TableTagHelper), out var parent) && parent is TableTagHelper table)
        {
            table.Columns.Add(new ColumnDefinition { For = For, Header = Header, Width = Width });
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
}
