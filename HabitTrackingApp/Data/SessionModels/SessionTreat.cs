using HabitTrackingApp.Data.Core;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HabitTrackingApp.Data.SessionModels
{
    [PrimaryKey(nameof(SessionId), nameof(TreatId))]
    public class SessionTreat
    {
        public int SessionId { get; set; }
        [ForeignKey(nameof(SessionId))]
        public required Session Session { get; set; }

        public int TreatId { get; set; }
        [ForeignKey(nameof(TreatId))]
        public required Treat Treat { get; set; }

        [Required]
        public byte UnitsBought { get; set; } = 0;
    }
}
