using HTApp.Core.Contracts;
using HTApp.Infrastructure.EntityModels.SessionModels;

namespace HTApp.Infrastructure.Tests.Repositories;

class SessionRepositoryTest : DbContextSetupBase
{
    [SetUp]
    public async Task GetCurrentSessionTest()
    {
        foreach(var userId in new string[] {user1.Id, user2.Id})
        {
            Session ex = DbSessions.First(x => x.UserId == user1.Id && x.EndDate == null);
            SessionModel<int, int, int, int, int>? re = await SessionRepository.GetCurrentSession(userId);

            //bool result = ex.UserId == re.UserId && ex.Name == re.Name && ex.CreditsSuccess == re.CreditsSuccess && ex.CreditsFail == re.CreditsFail && ex.IsActive == re.IsActive;
            //Assert.That(result, Is.True);
        }
    }
}
