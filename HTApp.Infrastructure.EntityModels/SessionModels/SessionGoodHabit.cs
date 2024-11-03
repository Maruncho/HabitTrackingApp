using HTApp.Infrastructure.EntityModels.Core;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HTApp.Infrastructure.EntityModels.SessionModels
{
    [PrimaryKey(nameof(SessionId), nameof(GoodHabitId))]
    public class SessionGoodHabit
    {
        public int SessionId { get; set; }
        [ForeignKey(nameof(SessionId))]
        public required Session Session { get; set; }

        public int GoodHabitId { get; set; }
        [ForeignKey(nameof(GoodHabitId))]
        public required GoodHabit GoodHabit { get; set; }

        [Required]
        public bool Completed { get; set; } = false;
    }
}
