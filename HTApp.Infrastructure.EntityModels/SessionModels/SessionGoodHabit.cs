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
        public Session Session { get; set; } = null!;

        public int GoodHabitId { get; set; }
        [ForeignKey(nameof(GoodHabitId))]
        public GoodHabit GoodHabit { get; set; } = null!;

        [Required]
        public bool Completed { get; set; } = false;
    }
}
