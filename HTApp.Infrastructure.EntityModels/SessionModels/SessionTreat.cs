using HTApp.Infrastructure.EntityModels.Core;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HTApp.Infrastructure.EntityModels.SessionModels
{
    [PrimaryKey(nameof(SessionId), nameof(TreatId))]
    public class SessionTreat
    {
        public int SessionId { get; set; }
        [ForeignKey(nameof(SessionId))]
        public Session Session { get; set; } = null!;

        public int TreatId { get; set; }
        [ForeignKey(nameof(TreatId))]
        public Treat Treat { get; set; } = null!;

        [Required]
        public byte UnitsLeft { get; set; }
    }
}
