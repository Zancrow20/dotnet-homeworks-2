using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Hw7.MyHtmlServices;

public static class HtmlHelperExtensions
{
    public static IHtmlContent MyEditorForModel(this IHtmlHelper helper)
    {
        return CreateHtmlBody(helper);
    }
    
    private static IHtmlContent CreateHtmlBody(IHtmlHelper helper)
    {
        var model = helper.ViewData.Model;
        var htmlBody = new HtmlContentBuilder();
        var properties = helper.ViewData.ModelMetadata.ModelType.GetProperties();
        var htmlCode = properties
            .Select(prop => CreateHtmlCodeLine(prop,model));
        foreach (var htmlString in htmlCode)
            htmlBody.AppendHtmlLine(htmlString);
        return htmlBody;
    }
    
    private static string CreateHtmlCodeLine(PropertyInfo propertyInfo, object? model)
    {
        var label = CreateLabel(propertyInfo);
        var inputWithSpan = CreateInput(propertyInfo, model);
        var input = inputWithSpan.Item1;
        var span = inputWithSpan.Item2;
        return $"<div class=\"editor\">{label}<br>{span}<div class=\"editor-field\">{input}</div></div>";
    }

    private static (string,string) CreateInput(PropertyInfo propertyInfo, object? model)
    {
        var span = "";
        string input;
        if (propertyInfo.PropertyType.IsEnum)
            input = GetDropdown(propertyInfo.PropertyType);
        else
        {
            span = ValidateData(propertyInfo, model);
            input = GetInput(propertyInfo, model);
        }
        return (input,span);
    }

    private static string GetInput(PropertyInfo propertyInfo, object? model)
    {
        return "<input class=text-box single-line " +
               $"id=\"{propertyInfo.Name}\" name=\"{propertyInfo.Name}\" " +
               $"type=\"{(propertyInfo.PropertyType == typeof(int) ? "number" : "text")}\" " +
               $"value=\"{(model == null ? "" : propertyInfo.GetValue(model))}\">";
    }
    

    private static string ValidateData(PropertyInfo propertyInfo, object? model)
    {
        if(model != null)
            foreach (var validatorAttribute in propertyInfo.GetCustomAttributes().OfType<ValidationAttribute>())
            {
                if (!validatorAttribute
                        .IsValid(propertyInfo.GetValue(model)))
                    return $"<span class=\"field-validation-valid\" " +
                           $"data-valmsg-for=\"{propertyInfo.Name}\" " +
                           $"data-valmsg-replace=\"true\">{validatorAttribute.ErrorMessage}</span>";   
            }
        return $"<span class=\"field-validation-valid\" " +
               $"data-valmsg-for=\"{propertyInfo.Name}\" data-valmsg-replace=\"true\"></span>";
    }

    private static string GetDropdown(Type modelType)
    {
        var stringBuilder = new StringBuilder();
        if(modelType.IsEnum)
            foreach (var option in modelType.GetEnumValues())
                stringBuilder.Append($"<option value=\"{option}\">{option}</option>");
        
        return $"<select name=\"{modelType.Name}\" id=\"{modelType.Name}\">{stringBuilder.ToString()}</select>";
    }

    private static string CreateLabel(PropertyInfo propertyInfo)
    {
        var displayAttrName = propertyInfo
            .GetCustomAttributes()
            .OfType<DisplayAttribute>()
            .FirstOrDefault()
            ?.Name;
        var displayName = displayAttrName ?? GetCamelCase(propertyInfo.Name);
        return $"<label for=\"{propertyInfo.Name}\">{displayName}</label>";   
    }

    private static string GetCamelCase(string? name) => 
        Regex.Replace(name, "([A-Z])", " $1", RegexOptions.Compiled).Trim();
    
} 