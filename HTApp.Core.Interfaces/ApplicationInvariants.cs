using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTApp.Core.Contracts;

public static class ApplicationInvariants
{
    public static readonly int UserDataCreditsMin = 0;
    public static readonly int UserDataCreditsMax = int.MaxValue;

    public static readonly byte UserDataRefundsMin = 0;
    public static readonly byte UserDataRefundsMax = byte.MaxValue;
}
