﻿using MyShop.Core.Validations;
using MyShop.Core.ValueObjects.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Core.ValueObjects.Shared;
public sealed record StreetName : IValidatableValueObject
{
    public string Value { get; }
    public StreetName(string value)
    {
        if (!IsValid(value))
        {
            throw new ArgumentException(GetErrorMessage(value));
        }

        Value = value;
    }

    public static implicit operator string(StreetName value)
        => value.Value;

    public static implicit operator StreetName(string value)
        => new(value);

    public override string ToString()
        => Value;

    public const int MinLength = 1;
    public const int MaxLength = 140;

    private static bool IsValid(string value)
        => !string.IsNullOrWhiteSpace(value) && value.Length is >= MinLength and <= MaxLength;

    private static string GetErrorMessage()
        => $"The {nameof(StreetName)} must be between {MinLength} and {MaxLength} and not be whitespace.";

    private static string GetErrorMessage(string value)
        => $"The '{value}' is incorrect. {GetErrorMessage()}";

    public static void Validate(object? value, ICollection<ValidationMessage> validationMessages)
    {
        if (value is string stringValue)
        {
            if (!IsValid(stringValue))
            {
                validationMessages.Add(new(nameof(StreetName), [GetErrorMessage(stringValue)]));
            }
        }
        else
        {
            validationMessages.Add(
                new(
                    nameof(StreetName),
                    [$"The {nameof(StreetName)} must be a string.", GetErrorMessage()]
                    )
                );
        }
    }
}
