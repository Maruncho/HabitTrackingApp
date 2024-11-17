using HTApp.Infrastructure.EntityModels.SessionModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HTApp.Infrastructure.EntityModels.Core
{
    public class Treat : ISoftDeletable
    {
        public Treat()
        {
            SessionTreats = new HashSet<SessionTreat>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(32)]
        public required string Name { get; set; }

        [Required]
        public byte QuantityPerSession { get; set; }

        [Required]
        public int CreditsPrice { get; set; }

        public bool IsDeleted { get; set; } = false;

        [Required]
        public required string UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public required AppUser User { get; set; }

        public ICollection<SessionTreat> SessionTreats { get; set; } = null!;
    }
}
