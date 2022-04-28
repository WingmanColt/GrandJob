using HireMe.Entities.View;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Collections.Generic;
using System.Linq;

/// <summary>check-box-list Tag Helper</summary>
[HtmlTargetElement("Check-Box-List", Attributes = "asp-title, asp-items, asp-model-name, asp-check-all-label", TagStructure = TagStructure.NormalOrSelfClosing)]
public class CheckBoxListTagHelper : TagHelper
{
    /// <summary>HTML element ID of the tracking form element</summary>
    [HtmlAttributeName("asp-form-id")]
    public string FormId { get; set; }

    /// <summary>Optional bolder title set above the check box list</summary>
    [HtmlAttributeName("asp-title")]
    public string ListTitle { get; set; }

    /// <summary>List of individual child/item values to be rendered as check boxes</summary>
    [HtmlAttributeName("asp-items")]
    public List<CheckBoxListItem> Items { get; set; }

    /// <summary>The name of the view model which is used for rendering html "id" and "name" attributes of each check box input.
    /// Typically the name of a List[CheckBoxListItem] property on the actual passed in @Model</summary>
    [HtmlAttributeName("asp-model-name")]
    public string ModelName { get; set; }

    /// <summary>Optional label of a "Check All" type checkbox.  If left empty, a "Check All" check box will not be rendered.</summary>
    [HtmlAttributeName("asp-check-all-label")]
    public string CheckAllLabel { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        string id = context.UniqueId;
        output.TagName = "div";
        string html = "";

        output.PreElement.AppendHtml($"<!-- Check List Box for {(string.IsNullOrEmpty(ListTitle) ? ModelName : ListTitle)} -->\r\n");

        if (!string.IsNullOrEmpty(ListTitle))
        {
            // Prepend a Title to the control
            output.PreContent.AppendHtml($"\r\n\t<label id=\"check-box-list-label-{id}\" class=\"cblTitle\">\r\n"
                + $"\t\t<strong>{ListTitle}</strong>\r\n"
                + $"\t</label>\r\n");
        }

        if (!string.IsNullOrEmpty(CheckAllLabel))
        {
            // Prepend a "Check All" type checkbox to the control
            output.PreContent.AppendHtml("\t<div class=\"form-check\">\r\n"
                + $"\t\t<input id=\"check-box-list-all-{id}\"\r\n"
                + "\t\t\tclass=\"cblCheckAllInput form-check-input\"\r\n"
                + "\t\t\ttype=\"checkbox\"\r\n"
                + $"\t\t\tvalue=\"true\"\r\n"
                );

            if (Items.All(cbli => cbli.IsChecked))
            {
                output.PreContent.AppendHtml("\t\t\tchecked=\"checked\"\r\n");
            }

            output.PreContent.AppendHtml("\t\t\t/>\r\n"
                + $"\t\t<label id=\"check-box-list-all-label-{id}\" class=\"cblCheckAllLabel form-check-label\" for=\"check-box-list-all-{id}\">\r\n"
                + $"\t\t\t&nbsp; {CheckAllLabel}\r\n"
                + "\t\t</label>\r\n"
                + "\t</div>\r\n"
                );
        }

        // Begin the actual Check Box List control
        output.Content.AppendHtml($"\t<div id=\"cblContent-{id}\" class=\"cblContent\">\r\n");

        // Create an individual check box for each item
        for (int i = 0; i < Items.Count(); i++)
        {
            CheckBoxListItem item = Items[i];
            html = "\t\t<div class=\"form-check\">\r\n"
                + $"\t\t\t<input id=\"{ModelName}_{i}__IsChecked-{id}\"\r\n"
                + $"\t\t\t\tname=\"{ModelName}[{i}].IsChecked\"\r\n"
                + $"\t\t\t\tclass=\"cblCheckBox form-check-input\"\r\n"
                + $"\t\t\t\tform=\"{FormId}\"\r\n"
                + "\t\t\t\tdata-val=\"true\"\r\n"
                + "\t\t\t\ttype=\"checkbox\""
                + "\t\t\t\tvalue=\"true\""
                ;

            if (item.IsChecked)
            {
                html += "\t\t\t\tchecked=\"checked\"\r\n";
            }

            if (item.IsDisabled)
            {
                html += "\t\t\t\tdisabled=\"disabled\"\r\n";
            }

            html += "\t\t\t\t/>\r\n"
                + $"\t\t\t<label id=\"check-box-list-item-label-{id}-{i}\" class=\"cblItemLabel form-check-label\" for=\"{ModelName}_{i}__IsChecked-{id}\">\r\n"
                + $"\t\t\t\t&nbsp; {item.Value}\r\n"
                + "\t\t\t</label>\r\n"
                + $"\t\t\t<input type=\"hidden\" id=\"{ModelName}_{i}__IsChecked-{id}-tag\" name=\"{ModelName}[{i}].IsChecked\" form =\"{FormId}\" value=\"false\">\r\n"
                + $"\t\t\t<input type=\"hidden\" id=\"{ModelName}_{i}__Key-{id}\" name=\"{ModelName}[{i}].Key\" form =\"{FormId}\" value=\"{item.Key}\">\r\n"
                + $"\t\t\t<input type=\"hidden\" id=\"{ModelName}_{i}__Value-{id}\" name=\"{ModelName}[{i}].Value\" form =\"{FormId}\" value=\"{item.Value}\">\r\n"
                + "\t\t</div>\r\n"
                ;

            output.Content.AppendHtml(html);
        }

        output.Content.AppendHtml("\t</div>\r\n");
        output.Attributes.SetAttribute("id", $"check-box-list-{id}");
        output.Attributes.SetAttribute("class", "cblCheckBoxList");
    }
}