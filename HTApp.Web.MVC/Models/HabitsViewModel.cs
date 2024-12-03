using HTApp.Core.API;

namespace HTApp.Web.MVC.Models;

public class HabitsViewModel
{
    public required GoodHabitModel[] GoodHabits { get; set; }
    public required BadHabitModel[] BadHabits { get; set; }
}

