using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

using static HTApp.Core.API.ApplicationInvariants;

namespace HTApp.Core.API;

public class BadHabitFormModel
{
    [Length(BadHabitNameLengthMin, BadHabitNameLengthMax)]
    public string Name { get; set; } = null!;

    [Range(BadHabitCreditsSuccessMin, BadHabitCreditsSuccessMax)]
    public int CreditsSuccess { get; set; }

    [Range(BadHabitCreditsFailMin, BadHabitCreditsFailMax)]
    public int CreditsFail { get; set; }

    [ValidateNever]
    public string UserId { get; set; } = null!;


    public static implicit operator BadHabitInputModel(BadHabitFormModel model)
    {
        return new BadHabitInputModel
        {
            Name = model.Name,
            CreditsSuccess = model.CreditsSuccess,
            CreditsFail = model.CreditsFail,
            UserId = model.UserId
        };
    }

    public static implicit operator BadHabitFormModel(BadHabitInputModel model)
    {
        return new BadHabitFormModel
        {
            Name = model.Name,
            CreditsSuccess = model.CreditsSuccess,
            CreditsFail = model.CreditsFail,
            UserId = model.UserId
        };
    }
}
