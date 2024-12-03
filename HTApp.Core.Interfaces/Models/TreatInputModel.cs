using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using static HTApp.Core.API.ApplicationInvariants;

namespace HTApp.Core.API;

public class TreatInputModel
{
    [Length(TreatNameLengthMin, TreatNameLengthMax)]
    public string Name { get; set; } = null!;

    [Range(TreatQuantityPerSessionMin, TreatQuantityPerSessionMax)]
    public byte QuantityPerSession { get; set; }

    [Range(TreatPriceMin, TreatPriceMax)]
    public int Price { get; set; }

    [ValidateNever]
    public string UserId { get; set; } = null!;
}
