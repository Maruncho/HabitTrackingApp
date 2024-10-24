using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HabitTrackingApp.Data.Core
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
        public required string UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public required AppUser User { get; set; }
    }
}
