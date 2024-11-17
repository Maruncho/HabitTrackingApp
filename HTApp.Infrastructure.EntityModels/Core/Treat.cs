using HTApp.Infrastructure.EntityModels.SessionModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HTApp.Infrastructure.EntityModels.Core
{
    public class Treat : SoftDeletable
    {
        public Treat()
        {
            SessionTreats = new HashSet<SessionTreat>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(32)]
        public string Name { get; set; } = null!;

        [Required]
        public byte QuantityPerSession { get; set; }

        [Required]
        public int CreditsPrice { get; set; }

        public override bool IsDeleted { get; set; } = false;

        [Required]
        public string UserId { get; set; } = null!;
        [ForeignKey(nameof(UserId))]
        public AppUser User { get; set; } = null!;

        public ICollection<SessionTreat> SessionTreats { get; set; } = null!;
    }
}
