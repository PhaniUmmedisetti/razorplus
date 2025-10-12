using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace RazorPlus.TagHelpers;

/// <summary>
/// <rp-validation-message asp-for />
/// </summary>
[HtmlTargetElement("rp-validation-message")]
public class ValidationMessageTagHelper : TagHelper
{
    private readonly IHtmlGenerator _generator;
    public ValidationMessageTagHelper(IHtmlGenerator generator) => _generator = generator;

    [HtmlAttributeNotBound]
    [ViewContext]
    public ViewContext ViewContext { get; set; } = default!;

    [HtmlAttributeName("asp-for")]
    public ModelExpression? For { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = null; // render child only
        if (For == null)
        {
            return;
        }

        var validation = _generator.GenerateValidationMessage(ViewContext, For.ModelExplorer, For.Name, null, null, new { @class = "rp-error" });
        output.Content.SetHtmlContent(validation);
    }
}

