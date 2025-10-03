using System;
using System.ComponentModel.DataAnnotations;

public class RequiredIfAmountIsGreaterThanZero : ValidationAttribute
{
    private readonly string _amountPropertyName;

    public RequiredIfAmountIsGreaterThanZero(string amountPropertyName)
    {
        _amountPropertyName = amountPropertyName;
    }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var amountProperty = validationContext.ObjectType.GetProperty(_amountPropertyName);
        if (amountProperty == null)
        {
            return new ValidationResult($"Unknown property: {_amountPropertyName}");
        }

        var amount = (decimal?)amountProperty.GetValue(validationContext.ObjectInstance);

        if (amount > 0 && string.IsNullOrEmpty(value?.ToString()))
        {
            return new ValidationResult(ErrorMessage);
        }

        return ValidationResult.Success;
    }
}

public class RequiredIfResourceSelected : ValidationAttribute
{
    private readonly string _resourcePropertyName;

    public RequiredIfResourceSelected(string resourcePropertyName)
    {
        _resourcePropertyName = resourcePropertyName;
    }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var resourceProperty = validationContext.ObjectType.GetProperty(_resourcePropertyName);
        if (resourceProperty == null)
        {
            return new ValidationResult($"Unknown property: {_resourcePropertyName}");
        }

        var resourceType = (string)resourceProperty.GetValue(validationContext.ObjectInstance);

        if (!string.IsNullOrEmpty(resourceType) && value == null)
        {
            return new ValidationResult(ErrorMessage);
        }

        return ValidationResult.Success;
    }
}
