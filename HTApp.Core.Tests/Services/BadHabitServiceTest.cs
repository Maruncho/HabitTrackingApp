using HTApp.Core.API;
using HTApp.Core.Services;
using Moq;

namespace HTApp.Core.Tests.Services;

public class BadHabitServiceTest
{

    private Mock<IUnitOfWork> unitOfWork;
    private Mock<IBadHabitRepository> badHabitRepository;

    private Mock<IBadHabitObserver> observer;

    private IBadHabitService badHabitService;

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

        badHabitRepository = new Mock<IBadHabitRepository>();
        badHabitRepository.Setup(x => x.Exists(It.IsAny<int>()))
            .Returns(ValueTask.FromResult(true));
        badHabitRepository.Setup(x => x.Exists(NOT_FOUND))
            .Returns(ValueTask.FromResult(false));
        badHabitRepository.Setup(x => x.IsOwnerOf(It.IsAny<int>(), It.IsAny<string>()))
            .Returns(ValueTask.FromResult(true));
        badHabitRepository.Setup(x => x.IsOwnerOf(It.IsAny<int>(), OWNERSHIP))
            .Returns(ValueTask.FromResult(false));

        badHabitService = new BadHabitService(badHabitRepository.Object, unitOfWork.Object);

        observer = new Mock<IBadHabitObserver>();
        observer.Setup(x => x.NotifyWhenStatusChange(It.IsAny<string>()))
            .Returns(ValueTask.FromResult(new Response(ResponseCode.Success, "")));
        observer.Setup(x => x.NotifyWhenStatusChange(OBSERVER))
            .Returns(ValueTask.FromResult(new Response(ResponseCode.RepositoryError, "")));
        badHabitService.SubscribeToStatusChange(observer.Object);

    }

    [Test]
    public void SubscriptionTest()
    {
        //add null observer
        Assert.Throws<ArgumentNullException>(() =>
        {
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            badHabitService.SubscribeToStatusChange(null);
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
        });

        //delete null observer
        Assert.Throws<ArgumentNullException>(() =>
        {
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            badHabitService.UnsubscribeToStatusChange(null);
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
        });

        //actually remove
        Assert.DoesNotThrow(() => badHabitService.UnsubscribeToStatusChange(observer.Object));
    }

    [Test]
    public async Task AddTest()
    {

        string userId = NEUTRAL;
        var model = new BadHabitInputModel
        {
            Name = "name",
            CreditsSuccess = ApplicationInvariants.BadHabitCreditsSuccessMax,
            CreditsFail = ApplicationInvariants.BadHabitCreditsFailMax,
            UserId = userId,
        };

        //happy
        badHabitRepository.Setup(x => x.Add(model)).Returns(ValueTask.FromResult(true));
        var res = await badHabitService.Add(model, userId);
        Assert.That(res.Code, Is.EqualTo(ResponseCode.Success));

        //still happy
        badHabitRepository.Setup(x => x.Add(model)).Returns(ValueTask.FromResult(true));
        res = await badHabitService.Add(model, OBSERVER);
        Assert.That(res.Code, Is.EqualTo(ResponseCode.Success));

        //can't add
        badHabitRepository.Setup(x => x.Add(model)).Returns(ValueTask.FromResult(false));
        res = await badHabitService.Add(model, userId);
        Assert.That(res.Code, Is.EqualTo(ResponseCode.RepositoryError));

        //Invalid Name (hopefully)
        badHabitRepository.Setup(x => x.Add(model)).Returns(ValueTask.FromResult(true));
        model.Name = "";
        res = await badHabitService.Add(model, userId);
        Assert.That(res.Code, Is.EqualTo(ResponseCode.InvalidField));
    }

    [Test]
    public async Task DeleteTest()
    {
        badHabitRepository.Setup(x => x.Delete(CANT_DELETE))
            .Returns(ValueTask.FromResult(false));
        badHabitRepository.Setup(x => x.Delete(SUCCESS))
            .Returns(ValueTask.FromResult(true));

        //happy
        var res = await badHabitService.Delete(SUCCESS, NEUTRAL);
        Assert.That(res.Code, Is.EqualTo(ResponseCode.Success));

        //still happy
        res = await badHabitService.Delete(SUCCESS, OBSERVER);
        Assert.That(res.Code, Is.EqualTo(ResponseCode.Success));

        //not exist
        res = await badHabitService.Delete(NOT_FOUND, OBSERVER);
        Assert.That(res.Code, Is.EqualTo(ResponseCode.NotFound));

        //not authorized
        res = await badHabitService.Delete(SUCCESS, OWNERSHIP);
        Assert.That(res.Code, Is.EqualTo(ResponseCode.Unauthorized));

        //Can't delete
        res = await badHabitService.Delete(CANT_DELETE, NEUTRAL);
        Assert.That(res.Code, Is.EqualTo(ResponseCode.RepositoryError));
    }

    [Test]
    public async Task UpdateTest()
    {

        var model = new BadHabitInputModel
        {
            Name = "name",
            CreditsSuccess = ApplicationInvariants.BadHabitCreditsSuccessMax,
            CreditsFail = ApplicationInvariants.BadHabitCreditsFailMax,
            UserId = "doesn't matter",
        };

        badHabitRepository.Setup(x => x.Update(CANT_UPDATE, model))
            .Returns(ValueTask.FromResult(false));
        badHabitRepository.Setup(x => x.Update(SUCCESS, model))
            .Returns(ValueTask.FromResult(true));

        //happy
        var res = await badHabitService.Update(SUCCESS, model, NEUTRAL);
        Assert.That(res.Code, Is.EqualTo(ResponseCode.Success));

        //still happy
        res = await badHabitService.Update(SUCCESS, model, OBSERVER);
        Assert.That(res.Code, Is.EqualTo(ResponseCode.Success));

        //not exist
        res = await badHabitService.Update(NOT_FOUND, model, NEUTRAL);
        Assert.That(res.Code, Is.EqualTo(ResponseCode.NotFound));

        //not authorized
        res = await badHabitService.Update(SUCCESS, model, OWNERSHIP);
        Assert.That(res.Code, Is.EqualTo(ResponseCode.Unauthorized));

        //Can't update
        res = await badHabitService.Update(CANT_UPDATE, model, NEUTRAL);
        Assert.That(res.Code, Is.EqualTo(ResponseCode.RepositoryError));
    }

    [Test]
    public async Task GetsTest()
    {
        //Test Exists
        Assert.True(await badHabitService.Exists(SUCCESS));
        Assert.False(await badHabitService.Exists(NOT_FOUND));

        //Test IsOwnerOf
        Assert.True(await badHabitService.IsOwnerOf(SUCCESS, NEUTRAL));
        Assert.False(await badHabitService.IsOwnerOf(SUCCESS, OWNERSHIP));


        //////// Test GetAll and GetAllIds ////////

        //GetAll
        var r1 = await badHabitService.GetAll(NEUTRAL);
        Assert.That(r1.Code, Is.EqualTo(ResponseCode.Success));

        //GetAllIds
        var r2 = await badHabitService.GetAllIds(NEUTRAL);
        Assert.That(r1.Code, Is.EqualTo(ResponseCode.Success));


        //////// Test GetInputModel ////////

        //Success
        var r3 = await badHabitService.GetInputModel(SUCCESS, NEUTRAL);
        Assert.That(r3.Code, Is.EqualTo(ResponseCode.Success));

        //not exists
        r3 = await badHabitService.GetInputModel(NOT_FOUND, NEUTRAL);
        Assert.That(r3.Code, Is.EqualTo(ResponseCode.NotFound));

        //not exists
        r3 = await badHabitService.GetInputModel(SUCCESS, OWNERSHIP);
        Assert.That(r3.Code, Is.EqualTo(ResponseCode.Unauthorized));


        //////// Test GetLogicModel ////////

        //Success
        var r4 = await badHabitService.GetLogicModel(SUCCESS, NEUTRAL);
        Assert.That(r4.Code, Is.EqualTo(ResponseCode.Success));

        //not exists
        r4 = await badHabitService.GetLogicModel(NOT_FOUND, NEUTRAL);
        Assert.That(r4.Code, Is.EqualTo(ResponseCode.NotFound));

        //not exists
        r4 = await badHabitService.GetLogicModel(SUCCESS, OWNERSHIP);
        Assert.That(r4.Code, Is.EqualTo(ResponseCode.Unauthorized));
    }
}
