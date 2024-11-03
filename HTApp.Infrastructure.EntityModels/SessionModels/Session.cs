using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HTApp.Infrastructure.EntityModels.SessionModels
{
    public class Session
    {
        public Session()
        {
            SessionGoodHabits = new HashSet<SessionGoodHabit>();
            SessionBadHabits = new HashSet<SessionBadHabit>();
            SessionTransactions = new HashSet<SessionTransaction>();
            SessionTreats = new HashSet<SessionTreat>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        [Required]
        public byte Refunds { get; set; }

        [Required]
        public required string UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public required AppUser User { get; set; }

        public ICollection<SessionGoodHabit> SessionGoodHabits { get; set; } = null!;
        public ICollection<SessionBadHabit> SessionBadHabits { get; set; } = null!;
        public ICollection<SessionTransaction> SessionTransactions { get; set; } = null!;
        public ICollection<SessionTreat> SessionTreats { get; set; } = null!;
    }
}
