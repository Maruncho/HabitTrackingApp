using HTApp.Core.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTApp.Infrastructure.Tests.Repositories
{
    internal class UserDataRepositoryTest : DbContextSetupBase
    {
        [Test]
        public async Task GetEverythingTestAsync()
        {
            foreach (var user in new [] {user1, user2})
            {
                UserDataDump? data = await UserDataRepository.GetEverything(user.Id);

                Assert.That(data, Is.Not.Null);
                bool result = data.Credits == user.Credits && data.RefundsPerSession == user.RefundsPerSession;
                Assert.That(result, Is.True);
            }
            UserDataDump? nullData = await UserDataRepository.GetEverything(Guid.NewGuid().ToString());
            Assert.That(nullData, Is.Null);
        }

        [Test]
        public async Task GetCreditsTest()
        {
            foreach (var user in new [] {user1, user2})
            {
                int data = await UserDataRepository.GetCredits(user.Id);

                Assert.That(data, Is.EqualTo(user.Credits));
            }
        }

        [Test]
        public async Task GetRefundsPerSessionTest()
        {
            foreach (var user in new [] {user1, user2})
            {
                int data = await UserDataRepository.GetRefundsPerSession(user.Id);

                Assert.That(data, Is.EqualTo(user.RefundsPerSession));
            }
        }

        [Test]
        public async Task SetCreditsTest()
        {
            foreach (var user in new [] {user1, user2})
            {
                int newCredits = (int)Math.Truncate(123*Random.Shared.NextDouble());

                await UserDataRepository.SetCredits(user.Id, newCredits);
                db.SaveChanges();

                var updUser = db.AppUsers.Find(user.Id);

                Assert.That(newCredits, Is.EqualTo(updUser!.Credits));
            }
        }

        [Test]
        public async Task SetRefundsPerSessionTest()
        {
            foreach (var user in new [] {user1, user2})
            {
                int newRefunds = (int)Math.Truncate(123*Random.Shared.NextDouble());

                await UserDataRepository.SetCredits(user.Id, newRefunds);
                db.SaveChanges();

                var updUser = db.AppUsers.Find(user.Id);

                Assert.That(newRefunds, Is.EqualTo(updUser!.Credits));
            }
        }
    }
}
