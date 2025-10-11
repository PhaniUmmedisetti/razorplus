using System.Text.Json;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace RazorPlus.TagHelpers;

/// <summary>
/// <rp-chart id type data options height theme defer export>
/// Wraps ECharts via data attributes; initialization in razorplus.chart.js (future stage)
/// </summary>
[HtmlTargetElement("rp-chart")]
public class ChartTagHelper : TagHelper
{
    public string? Id { get; set; }
    public string Type { get; set; } = "line";
    public object? Data { get; set; }
    public object? Options { get; set; }
    public int Height { get; set; } = 280;
    public string Theme { get; set; } = "auto"; // auto|light|dark
    public string? Export { get; set; } // png|svg
    public bool Defer { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "div";
        output.Attributes.SetAttribute("class", "rp-chart");
        var id = string.IsNullOrWhiteSpace(Id) ? $"rp-chart-{Guid.NewGuid():N}" : Id!;
        output.Attributes.SetAttribute("id", id);
        output.Attributes.SetAttribute("data-rp-chart", "");
        output.Attributes.SetAttribute("data-rp-chart-type", Type);
        output.Attributes.SetAttribute("data-rp-chart-theme", Theme);
        if (!string.IsNullOrWhiteSpace(Export))
            output.Attributes.SetAttribute("data-rp-chart-export", Export);
        if (Defer)
            output.Attributes.SetAttribute("loading", "lazy");
        output.Attributes.SetAttribute("style", $"height:{Height}px");

        var payload = new { data = Data, options = Options };
        var json = JsonSerializer.Serialize(payload, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
        output.Content.SetHtmlContent($"<script type=\"application/json\" class=\"rp-chart__data\">{json}</script>");
    }
}

