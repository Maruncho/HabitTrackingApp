using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

using static HTApp.Core.API.ApplicationInvariants;

namespace HTApp.Core.API;

public class GoodHabitInputModel
{
    [Length(GoodHabitNameLengthMin, GoodHabitNameLengthMax)]
    public string Name { get; set; } = null!;

    [Range(GoodHabitCreditsSuccessMin, GoodHabitCreditsSuccessMax)]
    public int CreditsSuccess { get; set; }

    [Range(GoodHabitCreditsFailMin, GoodHabitCreditsFailMax)]
    public int CreditsFail { get; set; }

    public bool IsActive { get; set; } = true;

    [ValidateNever]
    public string UserId { get; set; } = null!;
}
