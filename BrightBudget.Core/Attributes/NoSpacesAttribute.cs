using System.ComponentModel.DataAnnotations;

namespace BrightBudgetApp.Core.Attributes
{
    public class NoSpacesAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            if (value is string str)
                return !str.Contains(" ");
            return true;
        }
    }
}
