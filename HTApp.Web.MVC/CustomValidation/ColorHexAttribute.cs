using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace HTApp.Web.MVC.CustomValidation;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
sealed public class ColorHexAttribute : ValidationAttribute
{

#pragma warning disable CS8765 // Nullability of type of parameter doesn't match overridden member (possibly because of nullability attributes).
    public override bool IsValid(object color)
#pragma warning restore CS8765 // Nullability of type of parameter doesn't match overridden member (possibly because of nullability attributes).
    {
        if(color as string == null)
            return false;
        return Regex.Match((color as string)!, "^#(?:[0-9a-fA-F]{3}){1,2}$").Success;
    }
}

