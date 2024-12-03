using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

using static HTApp.Core.API.ApplicationInvariants;

namespace HTApp.Core.API;

public class GoodHabitInputModel
{
    public string Name { get; set; } = null!;

    public int CreditsSuccess { get; set; }

    public int CreditsFail { get; set; }

    public bool IsActive { get; set; } = true;

    public string UserId { get; set; } = null!;
}
