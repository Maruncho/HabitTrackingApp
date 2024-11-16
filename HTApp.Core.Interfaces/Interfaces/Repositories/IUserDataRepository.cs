using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTApp.Core.Contracts;

// This repository is meant to be more granular.
// Note that it does not inherit IGenericRepository
// The setters are responsible for validation
// The setters do not save updates, but 'stage' them. That's what the SaveChanges is for.
public interface IUserDataRepository<UserIdType>
{
    public Task<UserDataDump?> GetAll(UserIdType userId);

    public Task<int> GetCredits(UserIdType userId);
    public Task<byte> GetRefundsPerSession(UserIdType userId);

    public ValueTask<bool> SetCredits(UserIdType userId, int newValue);
    public ValueTask<bool> SetRefundsPerSession(UserIdType userId, byte newValue);
}
