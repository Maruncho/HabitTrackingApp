using HabitTrackingApp.Data.Core;
using HabitTrackingApp.Data.SessionModels;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HabitTrackingApp.Data
{
    public class AppUser : IdentityUser
    {
        public AppUser()
        {
            GoodHabits = new HashSet<GoodHabit>();
            BadHabits = new HashSet<BadHabit>();
            Treats = new HashSet<Treat>();
            Transactions = new HashSet<Transaction>();
            Sessions = new HashSet<Session>();
        }

        [Required]
        public required int Credits { get; set; }

        [Required]
        public required byte RefundsPerSession { get; set; }

        public ICollection<GoodHabit> GoodHabits { get; set; } = null!;
        public ICollection<BadHabit> BadHabits { get; set;} = null!;
        public ICollection<Treat> Treats { get; set; } = null!;
        public ICollection<Transaction> Transactions { get; set; } = null!;
        public ICollection<Session> Sessions { get; set; } = null!;
    }
}
