using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

using static HTApp.Core.Contracts.ApplicationInvariants;

namespace HTApp.Core.Contracts;

public class BadHabitInputModel
{
    [Length(BadHabitNameLengthMin, BadHabitNameLengthMax)]
    public string Name { get; set; } = null!;

    [Range(BadHabitCreditsSuccessMin, BadHabitCreditsSuccessMax)]
    public int CreditsSuccess { get; set; }

    [Range(BadHabitCreditsFailMin, BadHabitCreditsFailMax)]
    public int CreditsFail { get; set; }

    [ValidateNever]
    public string UserId { get; set; } = null!;
}
