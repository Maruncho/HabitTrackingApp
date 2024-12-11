using HTApp.Core.API;
using HTApp.Core.Services;
using Moq;

namespace HTApp.Core.Tests.Services;

public class GoodHabitServiceTest
{

    private Mock<IUnitOfWork> unitOfWork;
    private Mock<IGoodHabitRepository> goodHabitRepository;

    private Mock<IGoodHabitObserver> observer;

    private IGoodHabitService goodHabitService;

    const int NOT_FOUND = 0;
    const int CANT_DELETE = -1;
    const int CANT_UPDATE = -2;
    const int SUCCESS = 1000;

    const string OWNERSHIP = "Ownership";
    const string OBSERVER = "Observer";
    const string NEUTRAL = "Success";

    [SetUp]
    public void SetUp()
    {
        unitOfWork = new Mock<IUnitOfWork>();
        unitOfWork.Setup(x => x.SaveChangesAsync())
            .Returns(Task.FromResult(true));

        goodHabitRepository = new Mock<IGoodHabitRepository>();
        goodHabitRepository.Setup(x => x.Exists(It.IsAny<int>()))
            .Returns(ValueTask.FromResult(true));
        goodHabitRepository.Setup(x => x.Exists(NOT_FOUND))
            .Returns(ValueTask.FromResult(false));
        goodHabitRepository.Setup(x => x.IsOwnerOf(It.IsAny<int>(), It.IsAny<string>()))
            .Returns(ValueTask.FromResult(true));
        goodHabitRepository.Setup(x => x.IsOwnerOf(It.IsAny<int>(), OWNERSHIP))
            .Returns(ValueTask.FromResult(false));

        goodHabitService = new GoodHabitService(goodHabitRepository.Object, unitOfWork.Object);

        observer = new Mock<IGoodHabitObserver>();
        observer.Setup(x => x.NotifyWhenStatusChange(It.IsAny<bool>(), It.IsAny<string>()))
            .Returns(ValueTask.FromResult(new Response(ResponseCode.Success, "")));
        observer.Setup(x => x.NotifyWhenStatusChange(It.IsAny<bool>(), OBSERVER))
            .Returns(ValueTask.FromResult(new Response(ResponseCode.RepositoryError, "")));
        goodHabitService.SubscribeToStatusChange(observer.Object);

    }

    [Test]
    public void SubscriptionTest()
    {
        //add null observer
        Assert.Throws<ArgumentNullException>(() =>
        {
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            goodHabitService.SubscribeToStatusChange(null);
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
        });

        //delete null observer
        Assert.Throws<ArgumentNullException>(() =>
        {
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            goodHabitService.UnsubscribeToStatusChange(null);
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
        });

        //actually remove
        Assert.DoesNotThrow(() => goodHabitService.UnsubscribeToStatusChange(observer.Object));
    }

    [Test]
    public async Task AddTest()
    {

        string userId = NEUTRAL;
        var model = new GoodHabitInputModel
        {
            Name = "name",
            CreditsSuccess = ApplicationInvariants.GoodHabitCreditsSuccessMax,
            CreditsFail = ApplicationInvariants.GoodHabitCreditsFailMax,
            IsActive = true,
            UserId = userId,
        };

        //happy
        goodHabitRepository.Setup(x => x.Add(model)).Returns(ValueTask.FromResult(true));
        var res = await goodHabitService.Add(model, userId);
        Assert.That(res.Code, Is.EqualTo(ResponseCode.Success));

        //still happy
        goodHabitRepository.Setup(x => x.Add(model)).Returns(ValueTask.FromResult(true));
        res = await goodHabitService.Add(model, OBSERVER);
        Assert.That(res.Code, Is.EqualTo(ResponseCode.Success));

        //can't add
        goodHabitRepository.Setup(x => x.Add(model)).Returns(ValueTask.FromResult(false));
        res = await goodHabitService.Add(model, userId);
        Assert.That(res.Code, Is.EqualTo(ResponseCode.RepositoryError));

        //Invalid Name (hopefully)
        goodHabitRepository.Setup(x => x.Add(model)).Returns(ValueTask.FromResult(true));
        model.Name = "";
        res = await goodHabitService.Add(model, userId);
        Assert.That(res.Code, Is.EqualTo(ResponseCode.InvalidField));
    }

    [Test]
    public async Task DeleteTest()
    {
        goodHabitRepository.Setup(x => x.Delete(CANT_DELETE))
            .Returns(ValueTask.FromResult(false));
        goodHabitRepository.Setup(x => x.Delete(SUCCESS))
            .Returns(ValueTask.FromResult(true));

        //happy
        var res = await goodHabitService.Delete(SUCCESS, NEUTRAL);
        Assert.That(res.Code, Is.EqualTo(ResponseCode.Success));

        //still happy
        res = await goodHabitService.Delete(SUCCESS, OBSERVER);
        Assert.That(res.Code, Is.EqualTo(ResponseCode.Success));

        //not exist
        res = await goodHabitService.Delete(NOT_FOUND, OBSERVER);
        Assert.That(res.Code, Is.EqualTo(ResponseCode.NotFound));

        //not authorized
        res = await goodHabitService.Delete(SUCCESS, OWNERSHIP);
        Assert.That(res.Code, Is.EqualTo(ResponseCode.Unauthorized));

        //Can't delete
        res = await goodHabitService.Delete(CANT_DELETE, NEUTRAL);
        Assert.That(res.Code, Is.EqualTo(ResponseCode.RepositoryError));
    }

    [Test]
    public async Task UpdateTest()
    {

        var model = new GoodHabitInputModel
        {
            Name = "name",
            CreditsSuccess = ApplicationInvariants.GoodHabitCreditsSuccessMax,
            CreditsFail = ApplicationInvariants.GoodHabitCreditsFailMax,
            IsActive = true,
            UserId = "doesn't matter",
        };

        goodHabitRepository.Setup(x => x.Update(CANT_UPDATE, model))
            .Returns(ValueTask.FromResult(false));
        goodHabitRepository.Setup(x => x.Update(SUCCESS, model))
            .Returns(ValueTask.FromResult(true));

        //happy
        var res = await goodHabitService.Update(SUCCESS, model, NEUTRAL);
        Assert.That(res.Code, Is.EqualTo(ResponseCode.Success));

        //still happy
        res = await goodHabitService.Update(SUCCESS, model, OBSERVER);
        Assert.That(res.Code, Is.EqualTo(ResponseCode.Success));

        //not exist
        res = await goodHabitService.Update(NOT_FOUND, model, NEUTRAL);
        Assert.That(res.Code, Is.EqualTo(ResponseCode.NotFound));

        //not authorized
        res = await goodHabitService.Update(SUCCESS, model, OWNERSHIP);
        Assert.That(res.Code, Is.EqualTo(ResponseCode.Unauthorized));

        //Can't update
        res = await goodHabitService.Update(CANT_UPDATE, model, NEUTRAL);
        Assert.That(res.Code, Is.EqualTo(ResponseCode.RepositoryError));
    }

    [Test]
    public async Task GetsTest()
    {
        //Test Exists
        Assert.True(await goodHabitService.Exists(SUCCESS));
        Assert.False(await goodHabitService.Exists(NOT_FOUND));

        //Test IsOwnerOf
        Assert.True(await goodHabitService.IsOwnerOf(SUCCESS, NEUTRAL));
        Assert.False(await goodHabitService.IsOwnerOf(SUCCESS, OWNERSHIP));


        //////// Test GetAll and GetAllIds ////////

        //GetAll
        var r1 = await goodHabitService.GetAll(NEUTRAL);
        Assert.That(r1.Code, Is.EqualTo(ResponseCode.Success));

        //GetAllIds
        var r2 = await goodHabitService.GetAllIds(NEUTRAL);
        Assert.That(r1.Code, Is.EqualTo(ResponseCode.Success));


        //////// Test GetInputModel ////////

        //Success
        var r3 = await goodHabitService.GetInputModel(SUCCESS, NEUTRAL);
        Assert.That(r3.Code, Is.EqualTo(ResponseCode.Success));

        //not exists
        r3 = await goodHabitService.GetInputModel(NOT_FOUND, NEUTRAL);
        Assert.That(r3.Code, Is.EqualTo(ResponseCode.NotFound));

        //not exists
        r3 = await goodHabitService.GetInputModel(SUCCESS, OWNERSHIP);
        Assert.That(r3.Code, Is.EqualTo(ResponseCode.Unauthorized));


        //////// Test GetLogicModel ////////

        //Success
        var r4 = await goodHabitService.GetLogicModel(SUCCESS, NEUTRAL);
        Assert.That(r4.Code, Is.EqualTo(ResponseCode.Success));

        //not exists
        r4 = await goodHabitService.GetLogicModel(NOT_FOUND, NEUTRAL);
        Assert.That(r4.Code, Is.EqualTo(ResponseCode.NotFound));

        //not exists
        r4 = await goodHabitService.GetLogicModel(SUCCESS, OWNERSHIP);
        Assert.That(r4.Code, Is.EqualTo(ResponseCode.Unauthorized));
    }
}
