using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
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
        var input = inputWithSpan.Input;
        var span = inputWithSpan.Span;
        return $"<div class=\"editor\">{label}<br>{span}<div class=\"editor-field\">{input}</div></div>";
    }

    private static (string Input,string Span) CreateInput(PropertyInfo propertyInfo, object? model)
    {
        var inputWithSpan = (Input: "", Span: "");
        if (propertyInfo.PropertyType.IsEnum)
            inputWithSpan.Input = GetDropdown(propertyInfo.PropertyType);
        else
        {
            inputWithSpan.Span = ValidateData(propertyInfo, model);
            inputWithSpan.Input = GetInput(propertyInfo, model);
        }
        return inputWithSpan;
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
        var errorMessage = "";
        if (model != null)
        {
            var property = propertyInfo.GetValue(model)!;
            var attribute = propertyInfo
                .GetCustomAttributes()
                .OfType<ValidationAttribute>()
                .FirstOrDefault(validatorAttribute => !validatorAttribute.IsValid(property));
            errorMessage = attribute == null ? "" : attribute.ErrorMessage;
        }
        
        return $"<span class=\"field-validation-valid\" " +
               $"data-valmsg-for=\"{propertyInfo.Name}\" " +
               $"data-valmsg-replace=\"true\">{errorMessage}</span>";   
    }

    private static string GetDropdown(Type modelType)
    {
        string options = "";
        if (modelType.IsEnum)
            options = Enum
                .GetNames(modelType)
                .Aggregate((current, next) => $"<option value=\"{current}\">{current}</option>" 
                                              + $"<option value=\"{next}\">{next}</option>");
        return $"<select name=\"{modelType.Name}\" id=\"{modelType.Name}\">{options}</select>";
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