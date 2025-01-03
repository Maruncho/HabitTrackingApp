﻿using HTApp.Infrastructure.EntityModels.Core;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HTApp.Infrastructure.EntityModels.SessionModels
{
    public class Session
    {
        public Session()
        {
            SessionGoodHabits = new HashSet<SessionGoodHabit>();
            SessionBadHabits = new HashSet<SessionBadHabit>();
            Transactions = new HashSet<Transaction>();
            SessionTreats = new HashSet<SessionTreat>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        public bool Last { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        [Required]
        public byte Refunds { get; set; }

        [Required]
        public string UserId { get; set; } = null!;
        [ForeignKey(nameof(UserId))]
        public AppUser User { get; set; } = null!;

        public int? PreviousSessionId { get; set; }
        [ForeignKey(nameof(PreviousSessionId))]
        public Session? PreviousSession { get; set; }

        public ICollection<SessionGoodHabit> SessionGoodHabits { get; set; } = null!;
        public ICollection<SessionBadHabit> SessionBadHabits { get; set; } = null!;
        public ICollection<Transaction> Transactions { get; set; } = null!;
        public ICollection<SessionTreat> SessionTreats { get; set; } = null!;
    }
}
