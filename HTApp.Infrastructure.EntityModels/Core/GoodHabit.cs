using HTApp.Infrastructure.EntityModels.SessionModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using static HTApp.Core.API.ApplicationInvariants;

namespace HTApp.Infrastructure.EntityModels.Core
{
    public class GoodHabit : SoftDeletable
    {
        public GoodHabit()
        {
            SessionGoodHabits = new HashSet<SessionGoodHabit>();
        }


        [Key]
        public int Id { get; set; }

        [MaxLength(GoodHabitNameLengthMax)]
        [Required]
        public string Name { get; set; } = null!;

        [Required]
        public int CreditsSuccess { get; set; }

        [Required]
        public int CreditsFail { get; set; }

        public bool IsActive { get; set; } = true;

        public override bool IsDeleted { get; set; }

        [Required]
        public string UserId { get; set; } = null!;
        [ForeignKey(nameof(UserId))]
        public AppUser User { get; set; } = null!;

        public ICollection<SessionGoodHabit> SessionGoodHabits { get; set; } = null!;
    }
}
