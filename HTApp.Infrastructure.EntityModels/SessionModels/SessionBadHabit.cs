using HTApp.Infrastructure.EntityModels.Core;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HTApp.Infrastructure.EntityModels.SessionModels
{
    [PrimaryKey(nameof(SessionId), nameof(BadHabitId))]
    public class SessionBadHabit
    {
        public int SessionId { get; set; }
        [ForeignKey(nameof(SessionId))]
        public Session Session { get; set; } = null!;

        public int BadHabitId { get; set; }
        [ForeignKey(nameof(BadHabitId))]
        public BadHabit BadHabit { get; set; } = null!;

        [Required]
        public bool Failed { get; set; } = false;
    }
}
