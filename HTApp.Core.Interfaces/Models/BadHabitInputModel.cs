using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

using static HTApp.Core.API.ApplicationInvariants;

namespace HTApp.Core.API;

public class BadHabitInputModel
{
    public string Name { get; set; } = null!;

    public int CreditsSuccess { get; set; }

    public int CreditsFail { get; set; }

    public string UserId { get; set; } = null!;
}
