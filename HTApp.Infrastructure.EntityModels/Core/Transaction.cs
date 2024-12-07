using HTApp.Infrastructure.EntityModels.SessionModels;
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

        [Required]
        public int TypeId { get; set; }
        [ForeignKey(nameof(TypeId))]
        public TransactionType Type { get; set; } = null!;

        [Required]
        public string UserId { get; set; } = null!;
        [ForeignKey(nameof(UserId))]
        public AppUser User { get; set; } = null!;

        //Manual Transactions don't have a session, so it's not required.
        public int? SessionId { get; set; }
        [ForeignKey(nameof(SessionId))]
        public Session? Session { get; set; }
    }
}
