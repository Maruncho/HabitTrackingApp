using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

using static HTApp.Core.API.ApplicationInvariants;

namespace HTApp.Web.MVC.Models; 
public class TransactionAddManualFormModel
{
    [Range(TransactionAmountMin, TransactionAmountMax)]
    public int Amount { get; set; }
}
