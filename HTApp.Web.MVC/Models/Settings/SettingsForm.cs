
using System.ComponentModel.DataAnnotations;
using static HTApp.Core.API.ApplicationInvariants;

namespace HTApp.Web.MVC.Models;

public class SettingsForm
{
    [Range(UserDataRefundsMin, UserDataRefundsMax)]
    public byte RefundsPerSession { get; set; }
}
