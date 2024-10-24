using HabitTrackingApp.Data.Core;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace HabitTrackingApp.Data.SessionModels
{
    [PrimaryKey(nameof(SessionId), nameof(TransactionId))]
    public class SessionTransaction
    {
        public int SessionId { get; set; }
        [ForeignKey(nameof(SessionId))]
        public required Session Session { get; set; }

        public int TransactionId { get; set; }
        [ForeignKey(nameof(TransactionId))]
        public required Transaction Transaction { get; set; }
    }
}
