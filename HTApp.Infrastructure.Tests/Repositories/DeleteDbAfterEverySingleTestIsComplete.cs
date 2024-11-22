using HTApp.Infrastructure.EntityModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTApp.Infrastructure.Tests.Repositories
{
    [SetUpFixture]
    internal class TestsSetupClass
    {
        [OneTimeSetUp]
        public void GlobalSetup()
        {
        }

        [OneTimeTearDown]
        public void GlobalTeardown()
        {
            DbContextSetupBase.SetUpAndGetDbContext().Database.EnsureDeleted();
        }
    }
}
