using HTApp.Core.API;
using HTApp.Core.Services;
using Moq;

namespace HTApp.Core.Tests.Services;

public class TreatServiceTest
{

    private Mock<IUnitOfWork> unitOfWork;
    private Mock<ITreatRepository> treatRepository;

    private Mock<ITreatObserver> observer;

    private ITreatService treatService;

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

        treatRepository = new Mock<ITreatRepository>();
        treatRepository.Setup(x => x.Exists(It.IsAny<int>()))
            .Returns(ValueTask.FromResult(true));
        treatRepository.Setup(x => x.Exists(NOT_FOUND))
            .Returns(ValueTask.FromResult(false));
        treatRepository.Setup(x => x.IsOwnerOf(It.IsAny<int>(), It.IsAny<string>()))
            .Returns(ValueTask.FromResult(true));
        treatRepository.Setup(x => x.IsOwnerOf(It.IsAny<int>(), OWNERSHIP))
            .Returns(ValueTask.FromResult(false));

        treatService = new TreatService(treatRepository.Object, unitOfWork.Object);

        observer = new Mock<ITreatObserver>();
        observer.Setup(x => x.NotifyWhenStatusChange(It.IsAny<string>()))
            .Returns(ValueTask.FromResult(new Response(ResponseCode.Success, "")));
        observer.Setup(x => x.NotifyWhenStatusChange(OBSERVER))
            .Returns(ValueTask.FromResult(new Response(ResponseCode.RepositoryError, "")));
        treatService.SubscribeToStatusChange(observer.Object);

    }

    [Test]
    public void SubscriptionTest()
    {
        //add null observer
        Assert.Throws<ArgumentNullException>(() =>
        {
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            treatService.SubscribeToStatusChange(null);
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
        });

        //delete null observer
        Assert.Throws<ArgumentNullException>(() =>
        {
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            treatService.UnsubscribeToStatusChange(null);
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
        });

        //actually remove
        Assert.DoesNotThrow(() => treatService.UnsubscribeToStatusChange(observer.Object));
    }

    [Test]
    public async Task AddTest()
    {

        string userId = NEUTRAL;
        var model = new TreatInputModel
        {
            Name = "name",
            Price = ApplicationInvariants.TreatPriceMax,
            QuantityPerSession = ApplicationInvariants.TreatQuantityPerSessionMax,
            UserId = userId,
        };

        //happy
        treatRepository.Setup(x => x.Add(model)).Returns(ValueTask.FromResult(true));
        var res = await treatService.Add(model, userId);
        Assert.That(res.Code, Is.EqualTo(ResponseCode.Success));

        //still happy
        treatRepository.Setup(x => x.Add(model)).Returns(ValueTask.FromResult(true));
        res = await treatService.Add(model, OBSERVER);
        Assert.That(res.Code, Is.EqualTo(ResponseCode.Success));

        //can't add
        treatRepository.Setup(x => x.Add(model)).Returns(ValueTask.FromResult(false));
        res = await treatService.Add(model, userId);
        Assert.That(res.Code, Is.EqualTo(ResponseCode.RepositoryError));

        //Invalid Name (hopefully)
        treatRepository.Setup(x => x.Add(model)).Returns(ValueTask.FromResult(true));
        model.Name = "";
        res = await treatService.Add(model, userId);
        Assert.That(res.Code, Is.EqualTo(ResponseCode.InvalidField));
    }

    [Test]
    public async Task DeleteTest()
    {
        treatRepository.Setup(x => x.Delete(CANT_DELETE))
            .Returns(ValueTask.FromResult(false));
        treatRepository.Setup(x => x.Delete(SUCCESS))
            .Returns(ValueTask.FromResult(true));

        //happy
        var res = await treatService.Delete(SUCCESS, NEUTRAL);
        Assert.That(res.Code, Is.EqualTo(ResponseCode.Success));

        //still happy
        res = await treatService.Delete(SUCCESS, OBSERVER);
        Assert.That(res.Code, Is.EqualTo(ResponseCode.Success));

        //not exist
        res = await treatService.Delete(NOT_FOUND, OBSERVER);
        Assert.That(res.Code, Is.EqualTo(ResponseCode.NotFound));

        //not authorized
        res = await treatService.Delete(SUCCESS, OWNERSHIP);
        Assert.That(res.Code, Is.EqualTo(ResponseCode.Unauthorized));

        //Can't delete
        res = await treatService.Delete(CANT_DELETE, NEUTRAL);
        Assert.That(res.Code, Is.EqualTo(ResponseCode.RepositoryError));
    }

    [Test]
    public async Task UpdateTest()
    {

        var model = new TreatInputModel
        {
            Name = "name",
            Price = ApplicationInvariants.TreatPriceMax,
            QuantityPerSession = ApplicationInvariants.TreatQuantityPerSessionMax,
            UserId = "doesn't matter",
        };

        treatRepository.Setup(x => x.Update(CANT_UPDATE, model))
            .Returns(ValueTask.FromResult(false));
        treatRepository.Setup(x => x.Update(SUCCESS, model))
            .Returns(ValueTask.FromResult(true));

        //happy
        var res = await treatService.Update(SUCCESS, model, NEUTRAL);
        Assert.That(res.Code, Is.EqualTo(ResponseCode.Success));

        //still happy
        res = await treatService.Update(SUCCESS, model, OBSERVER);
        Assert.That(res.Code, Is.EqualTo(ResponseCode.Success));

        //not exist
        res = await treatService.Update(NOT_FOUND, model, NEUTRAL);
        Assert.That(res.Code, Is.EqualTo(ResponseCode.NotFound));

        //not authorized
        res = await treatService.Update(SUCCESS, model, OWNERSHIP);
        Assert.That(res.Code, Is.EqualTo(ResponseCode.Unauthorized));

        //Can't update
        res = await treatService.Update(CANT_UPDATE, model, NEUTRAL);
        Assert.That(res.Code, Is.EqualTo(ResponseCode.RepositoryError));
    }

    [Test]
    public async Task GetsTest()
    {
        //Test Exists
        Assert.True(await treatService.Exists(SUCCESS));
        Assert.False(await treatService.Exists(NOT_FOUND));

        //Test IsOwnerOf
        Assert.True(await treatService.IsOwnerOf(SUCCESS, NEUTRAL));
        Assert.False(await treatService.IsOwnerOf(SUCCESS, OWNERSHIP));


        //////// Test GetAll and GetAllIds ////////

        //GetAll
        var r1 = await treatService.GetAll(NEUTRAL);
        Assert.That(r1.Code, Is.EqualTo(ResponseCode.Success));

        //GetAllIds
        var r2 = await treatService.GetAllIdAndUnitsLeftPairs(NEUTRAL);
        Assert.That(r1.Code, Is.EqualTo(ResponseCode.Success));


        //////// Test GetInputModel ////////

        //Success
        var r3 = await treatService.GetInputModel(SUCCESS, NEUTRAL);
        Assert.That(r3.Code, Is.EqualTo(ResponseCode.Success));

        //not exists
        r3 = await treatService.GetInputModel(NOT_FOUND, NEUTRAL);
        Assert.That(r3.Code, Is.EqualTo(ResponseCode.NotFound));

        //not exists
        r3 = await treatService.GetInputModel(SUCCESS, OWNERSHIP);
        Assert.That(r3.Code, Is.EqualTo(ResponseCode.Unauthorized));


        //////// Test GetLogicModel ////////

        //Success
        var r4 = await treatService.GetLogicModel(SUCCESS, NEUTRAL);
        Assert.That(r4.Code, Is.EqualTo(ResponseCode.Success));

        //not exists
        r4 = await treatService.GetLogicModel(NOT_FOUND, NEUTRAL);
        Assert.That(r4.Code, Is.EqualTo(ResponseCode.NotFound));

        //not exists
        r4 = await treatService.GetLogicModel(SUCCESS, OWNERSHIP);
        Assert.That(r4.Code, Is.EqualTo(ResponseCode.Unauthorized));
    }
}
