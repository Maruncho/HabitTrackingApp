using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTApp.Core.Contracts;

public interface ICanSaveChanges
{
    public Task<bool> SaveChangesAsync();
}
