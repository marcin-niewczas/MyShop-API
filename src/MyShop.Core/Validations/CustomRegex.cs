using System.Text.RegularExpressions;
using SharedValueObjects = MyShop.Core.ValueObjects.Shared;

namespace MyShop.Core.Validations;
public sealed partial class CustomRegex
{
    public const string EmailPattern
        = @"(([^<>()\[\]\\.,;:\s@""]+(\.[^<>()\[\]\\.,;:\s@""]+)*)|("".+""))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))";

    [GeneratedRegex(EmailPattern)]
    public static partial Regex Email();

    public const string PasswordPattern
        = $"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[^A-Za-z0-9]).{{{SharedValueObjects.Password.MinLength},{SharedValueObjects.Password.MaxLength}}}$";

    [GeneratedRegex(PasswordPattern)]
    public static partial Regex Password();

    public const string PhoneNumberPattern
        = @"^[\+]?[(]?[0-9]{3}[)]?[-\s\.]?[0-9]{3}[-\s\.]?[0-9]{4,6}$";

    [GeneratedRegex(PhoneNumberPattern)]
    public static partial Regex PhoneNumber();

    public const string PricePattern
        = @"^[0-9]+(,|.[0-9]{1,2})?$";

    [GeneratedRegex(PricePattern)]
    public static partial Regex Price();

    public const string NaturalNumberPattern
        = @"^(0|[1-9][0-9]*)$";

    [GeneratedRegex(NaturalNumberPattern)]
    public static partial Regex NaturalNumber();
}
