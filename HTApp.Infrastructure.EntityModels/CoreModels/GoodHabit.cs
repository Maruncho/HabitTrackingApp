using HTApp.Infrastructure.EntityModels.SessionModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HTApp.Infrastructure.EntityModels.Core
{
    internal class GoodHabit
    {
        public GoodHabit()
        {
            SessionGoodHabits = new HashSet<SessionGoodHabit>();
        }


        [Key]
        public int Id { get; set; }

        [MaxLength(32)]
        [Required]
        public required string Name { get; set; }

        [Required]
        public int CreditsSuccess { get; set; }

        [Required]
        public int CreditsFail { get; set; }

        public bool IsActive { get; set; } = true;

        public bool IsDeleted { get; set; } = false;

        [Required]
        public required string UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public required AppUser User { get; set; }

        public ICollection<SessionGoodHabit> SessionGoodHabits { get; set; } = null!;
    }
}
