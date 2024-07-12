﻿using MyShop.Application.Validations.Interfaces;
using MyShop.Application.Validations.Validators;
using MyShop.Core.HelperModels;
using MyShop.Core.Validations;
using MyShop.Core.ValueObjects.MainPageSections;

namespace MyShop.Application.Commands.ManagementPanel.MainPageSections;
public sealed record UpdatePositionsOfMainPageSectionsMp(
    IReadOnlyCollection<ValuePosition<Guid>> IdPositions
    ) : ICommand, IValidatable
{
    public void Validate(ICollection<ValidationMessage> validationMessages)
    {
        CustomValidators.HelperModels.Validate(
            IdPositions,
            validationMessages,
            maxPosition: MainPageSectionPosition.Max,
            paramName: nameof(IdPositions)
            );
    }
}