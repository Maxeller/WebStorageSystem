using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace WebStorageSystem.TagHelpers
{
    // BASED ON: https://stackoverflow.com/questions/41071757/taghelpers-add-custom-class-for-labeltaghelper-based-on-validation-attribute-re
    [HtmlTargetElement("label", Attributes = "asp-for")]
    public class RequiredLabelTagHelper : LabelTagHelper
    {
        public RequiredLabelTagHelper(IHtmlGenerator generator) : base(generator)
        {
        }

        public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            if (For.Metadata.IsRequired)
            {
                CreateOrMergeAttribute("class", "required", output);
            }

            return base.ProcessAsync(context, output);
        }

        private void CreateOrMergeAttribute(string name, object content, TagHelperOutput output)
        {
            var currentAttribute = output.Attributes.FirstOrDefault(attribute => attribute.Name == name);
            if (currentAttribute == null)
            {
                var attribute = new TagHelperAttribute(name, content);
                output.Attributes.Add(attribute);
            }
            else
            {
                var newAttribute = new TagHelperAttribute(name, $"{currentAttribute.Value} {content}", currentAttribute.ValueStyle);
                output.Attributes.Remove(currentAttribute);
                output.Attributes.Add(newAttribute);
            }
        }
    }
}
