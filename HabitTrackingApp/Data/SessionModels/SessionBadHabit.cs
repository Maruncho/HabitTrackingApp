using HabitTrackingApp.Data.Core;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HabitTrackingApp.Data.SessionModels
{
    [PrimaryKey(nameof(SessionId), nameof(BadHabitId))]
    public class SessionBadHabit
    {
        public int SessionId { get; set; }
        [ForeignKey(nameof(SessionId))]
        public required Session Session { get; set; }

        public int BadHabitId { get; set; }
        [ForeignKey(nameof(BadHabitId))]
        public required BadHabit BadHabit { get; set; }

        [Required]
        public bool Failed { get; set; } = false;
    }
}
