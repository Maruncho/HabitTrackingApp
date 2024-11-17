using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HTApp.Infrastructure.EntityModels.Core
{
    public class Transaction
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int Amount { get; set; }

        public int? TypeId { get; set; }
        [ForeignKey(nameof(TypeId))]
        public TransactionType? Type { get; set; }

        [Required]
        public string UserId { get; set; } = null!;
        [ForeignKey(nameof(UserId))]
        public AppUser User { get; set; } = null!;
    }
}
