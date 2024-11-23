using HTApp.Infrastructure.EntityModels.SessionModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HTApp.Infrastructure.EntityModels.Core
{
    public class Transaction
    {
        public Transaction()
        {
            SessionTransactions = new HashSet<SessionTransaction>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        public int Amount { get; set; }

        [Required]
        public int TypeId { get; set; }
        [ForeignKey(nameof(TypeId))]
        public TransactionType Type { get; set; } = null!;

        [Required]
        public string UserId { get; set; } = null!;
        [ForeignKey(nameof(UserId))]
        public AppUser User { get; set; } = null!;

        public ICollection<SessionTransaction> SessionTransactions { get; set; }
    }
}
