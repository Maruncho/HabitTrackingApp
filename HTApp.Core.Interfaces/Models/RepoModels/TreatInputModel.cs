using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using static HTApp.Core.API.ApplicationInvariants;

namespace HTApp.Core.API;

public class TreatInputModel
{
    public string Name { get; set; } = null!;

    public byte QuantityPerSession { get; set; }

    public int Price { get; set; }

    public string UserId { get; set; } = null!;
}
