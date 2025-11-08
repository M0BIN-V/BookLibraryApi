using System.ComponentModel.DataAnnotations;

namespace Api.Common.Validators;

[AttributeUsage(AttributeTargets.Property)]
public class ValidPublishedYearAttribute : ValidationAttribute
{
    readonly int _minYear;

    public ValidPublishedYearAttribute(int minYear = 1500)
    {
        _minYear = minYear;
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is not int year) throw new InvalidCastException("value is not an integer");
        
        var maxYear = DateTime.UtcNow.Year;
        if (year < _minYear || year > maxYear)
            return new ValidationResult($"Published year must be between {_minYear} and {maxYear}");

        return ValidationResult.Success;
    }
}