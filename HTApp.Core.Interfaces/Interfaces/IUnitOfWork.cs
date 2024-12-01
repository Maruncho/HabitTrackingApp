using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTApp.Core.Contracts;

// You can use the repositories only, if you wish so, but then the individual
// repository methods should do their own SaveChanges whenever needed. You still
// need to implement this interface, but leave the method implementations empty.
// EF Core works like a Unit of Work, so that's why I chose to use the pattern.
public interface IUnitOfWork
{
    public IGoodHabitRepository GoodHabitRepository { get; init; }
    public IBadHabitRepository BadHabitRepository { get; init; }
    public ITreatRepository TreatRepository { get; init; }
    public ITransactionRepository TransactionRepository { get; init; }
    public ISessionRepository SessionRepository { get; init; }
    public IUserDataRepository UserDataRepository { get; init; }

    public Task<bool> SaveChangesAsync();
}
