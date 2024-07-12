using MyShop.Core.Abstractions;

namespace MyShop.Core.Exceptions;
public static class AllowedValuesError
{
    public static string Message<TAllowedValue>() where TAllowedValue : IAllowedValues
        => $"The {typeof(TAllowedValue).Name} field must be in [ {string.Join(", ", TAllowedValue.AllowedValues)} ].";
}
