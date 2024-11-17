using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTApp.Core.Contracts;

// You can use the repositories only, if you wish so.
// But EF Core works like a Unit of Work.
public interface IUnitOfWork
{
    public Task<bool> SaveChangesAsync();
}
