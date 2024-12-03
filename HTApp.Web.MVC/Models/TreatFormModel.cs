using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using static HTApp.Core.API.ApplicationInvariants;

namespace HTApp.Core.API;

public class TreatFormModel
{
    [Length(TreatNameLengthMin, TreatNameLengthMax)]
    public string Name { get; set; } = null!;

    [Range(TreatQuantityPerSessionMin, TreatQuantityPerSessionMax)]
    public byte QuantityPerSession { get; set; }

    [Range(TreatPriceMin, TreatPriceMax)]
    public int Price { get; set; }

    [ValidateNever]
    public string UserId { get; set; } = null!;

    public static implicit operator TreatInputModel(TreatFormModel model)
    {
        return new TreatInputModel
        {
            Name = model.Name,
            QuantityPerSession = model.QuantityPerSession,
            Price = model.Price,
            UserId = model.UserId
        };
    }

    public static implicit operator TreatFormModel(TreatInputModel model)
    {
        return new TreatFormModel
        {
            Name = model.Name,
            QuantityPerSession = model.QuantityPerSession,
            Price = model.Price,
            UserId = model.UserId
        };
    }
}
