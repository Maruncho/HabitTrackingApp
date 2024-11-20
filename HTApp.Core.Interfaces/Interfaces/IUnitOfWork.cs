﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTApp.Core.Contracts;

// You can use the repositories only, if you wish so, but then the individual
// repository methods should do their own SaveChanges whenever needed. You still
// need to implement this interface, but leave the method implementations empty.
// EF Core works like a Unit of Work, so that's why I chose to use the pattern.
public interface IUnitOfWork<UserIdType, GoodHabitModelId, BadHabitModelId, TreatModelId, TransactionId, SessionModelId>
{
    public IGoodHabitRepository<UserIdType, GoodHabitModelId> GoodHabitRepository { get; init; }
    public IBadHabitRepository<UserIdType, BadHabitModelId> BadHabitRepository { get; init; }
    public ITreatRepository<UserIdType, TreatModelId> TreatRepository { get; init; }
    public ITransactionRepository<UserIdType, TransactionId> TransactionRepository { get; init; }
    public ISessionRepository<UserIdType, SessionModelId, GoodHabitModelId, BadHabitModelId, TransactionId, TreatModelId> sessionRepository { get; init; }

    public Task<bool> SaveChangesAsync();
}
