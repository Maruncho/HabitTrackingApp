using HTApp.Infrastructure.EntityModels.Core;
using HTApp.Infrastructure.EntityModels.SessionModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HTApp.Infrastructure.EntityModels
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {
        public virtual DbSet<AppUser> AppUsers { get; set; }

        public virtual DbSet<GoodHabit> GoodHabits { get; set; }
        public virtual DbSet<BadHabit> BadHabits { get; set; }
        public virtual DbSet<Treat> Treats { get; set; }
        public virtual DbSet<Transaction> Transactions { get; set; }
        public virtual DbSet<TransactionType> TransactionTypes { get; set; }

        public virtual DbSet<Session> Sessions { get; set; }
        public virtual DbSet<SessionGoodHabit> SessionGoodHabits { get; set; }
        public virtual DbSet<SessionBadHabit> SessionBadHabits { get; set; }
        public virtual DbSet<SessionTransaction> SessionTransactions { get; set; }
        public virtual DbSet<SessionTreat> SessionTreats { get; set; }


        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<AppUser>()
                .Property(a => a.Credits)
                .HasDefaultValue(0);

            modelBuilder.Entity<AppUser>()
                .Property(a => a.RefundsPerSession)
                .HasDefaultValue(5);

            modelBuilder.Entity<GoodHabit>()
                .Property(g => g.IsActive)
                .HasDefaultValue(true);

            modelBuilder.Entity<GoodHabit>()
                .Property(g => g.IsDeleted)
                .HasDefaultValue(false);

            modelBuilder.Entity<BadHabit>()
                .Property(g => g.IsDeleted)
                .HasDefaultValue(false);

            modelBuilder.Entity<Treat>()
                .Property(t => t.QuantityPerSession)
                .HasDefaultValue(1);

            modelBuilder.Entity<Treat>()
                .Property(t => t.IsDeleted)
                .HasDefaultValue(0);

            modelBuilder.Entity<Transaction>()
                .Property(t => t.TypeId)
                .HasDefaultValue(0);

            modelBuilder.ApplyConfiguration(new TransactionTypeConfiguration());


            modelBuilder.Entity<SessionGoodHabit>()
                .Property(s => s.Completed)
                .HasDefaultValue(0);

            modelBuilder.Entity<SessionGoodHabit>()
                .HasOne(s => s.GoodHabit)
                .WithMany(s => s.SessionGoodHabits)
                .OnDelete(DeleteBehavior.NoAction);


            modelBuilder.Entity<SessionBadHabit>()
                .Property(s => s.Failed)
                .HasDefaultValue(0);

            modelBuilder.Entity<SessionBadHabit>()
                .HasOne(s => s.BadHabit)
                .WithMany(s => s.SessionBadHabits)
                .OnDelete(DeleteBehavior.NoAction);


            modelBuilder.Entity<SessionTreat>()
                .HasOne(s => s.Treat)
                .WithMany(t => t.SessionTreats)
                .OnDelete(DeleteBehavior.NoAction);


            modelBuilder.Entity<SessionTransaction>()
                .HasOne(s => s.Transaction)
                .WithMany(s => s.SessionTransactions)
                .OnDelete(DeleteBehavior.NoAction);

            base.OnModelCreating(modelBuilder);
        }
    }
}
