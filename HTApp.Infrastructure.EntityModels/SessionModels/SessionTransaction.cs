using HTApp.Infrastructure.EntityModels.Core;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace HTApp.Infrastructure.EntityModels.SessionModels
{
    [PrimaryKey(nameof(SessionId), nameof(TransactionId))]
    public class SessionTransaction
    {
        public int SessionId { get; set; }
        [ForeignKey(nameof(SessionId))]
        public Session Session { get; set; } = null!;

        public int TransactionId { get; set; }
        [ForeignKey(nameof(TransactionId))]
        public Transaction Transaction { get; set; } = null!;
    }
}
