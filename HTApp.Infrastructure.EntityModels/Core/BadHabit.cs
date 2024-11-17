using HTApp.Infrastructure.EntityModels.SessionModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HTApp.Infrastructure.EntityModels.Core
{
    public class BadHabit : SoftDeletable
    {
        public BadHabit()
        {
            SessionBadHabits = new HashSet<SessionBadHabit>();
        }

        [Key]
        public int Id { get; set; }

        [MaxLength(32)]
        [Required]
        public string Name { get; set; } = null!;

        [Required]
        public int CreditsSuccess { get; set; }

        [Required]
        public int CreditsFail { get; set; }

        public override bool IsDeleted { get; set; } = false;

        [Required]
        public string UserId { get; set; } = null!;
        [ForeignKey(nameof(UserId))]
        public AppUser User { get; set; } = null!;

        public ICollection<SessionBadHabit> SessionBadHabits { get; set; } = null!;
    }
}
