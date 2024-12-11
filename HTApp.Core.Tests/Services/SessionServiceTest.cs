using HTApp.Core.API;
using HTApp.Core.API.Models.RepoModels;
using HTApp.Core.Services;
using Moq;

namespace HTApp.Core.Tests.Services;

public class SessionServiceTest
{

    private Mock<IUnitOfWork> unitOfWork;
    private Mock<ISessionRepository> sessionRepository;

    private Mock<ISessionObserver> observer;

    private Mock<IGoodHabitService> goodHabitService;
    private Mock<IBadHabitService> badHabitService;
    private Mock<ITreatService> treatService;


    private ISessionService sessionService;

    const int NOT_FOUND = 0;
    const int CANT_DELETE = -1;
    const int CANT_ADD = -2;
    const int CANT_CREDIT = -3;

    const int GH_NOTACTIVE = -5;

    const int SUCCESS = 1000;

    const int GOOD_ID = 1;
    const int BAD_ID = -1;
    const string HAS_BAD_ID = "HasBadId";

    const string OWNERSHIP = "Ownership";
    const string NOTFOUND = "NotFound";
    const string OBSERVER = "Observer";
    const string NEUTRAL = "Success";

    const string CANT_START = "CantStart";
    const string CANT_FINISH = "CantFinish";
    const string CANT_UPDATE = "CantUpdate";
    const string CANT_REFUND = "CantRefund";
    const string FINISHED = "NotFinished";

    const string NOREFUNDS = "NoRefunds";
    const string NOCREDITS = "NoCredits";
    const string NOUNITS = "NoUnits";

    const string MODEL_SUCCESS = "ModelSuccess";
    const string MODEL_FAIL = "ModelFail";

    const string TRANSACTION_FAIL = "TransactionFail";

    [SetUp]
    public void SetUp()
    {
        unitOfWork = new Mock<IUnitOfWork>();
        unitOfWork.Setup(x => x.SaveChangesAsync())
            .Returns(Task.FromResult(true));

        sessionRepository = new Mock<ISessionRepository>();
        sessionRepository.Setup(x => x.Exists(It.IsAny<int>()))
            .Returns(ValueTask.FromResult(true));
        sessionRepository.Setup(x => x.Exists(NOT_FOUND))
            .Returns(ValueTask.FromResult(false));
        sessionRepository.Setup(x => x.IsOwnerOf(It.IsAny<int>(), It.IsAny<string>()))
            .Returns(ValueTask.FromResult(true));
        sessionRepository.Setup(x => x.IsOwnerOf(It.IsAny<int>(), OWNERSHIP))
            .Returns(ValueTask.FromResult(false));
        sessionRepository.Setup(x => x.GetIsLastSessionFinished(It.IsAny<string>()))
            .Returns(Task.FromResult<SessionIsFinishedModel?>(new SessionIsFinishedModel { Id = 1, IsFinished = false }));
        sessionRepository.Setup(x => x.GetIsLastSessionFinished(FINISHED))
            .Returns(Task.FromResult<SessionIsFinishedModel?>(new SessionIsFinishedModel { Id = 1, IsFinished = true }));
        sessionRepository.Setup(x => x.StartNewSession(It.IsAny<SessionAddModel>()))
            .Returns((SessionAddModel model) => ValueTask.FromResult(model.UserId == CANT_START ? false : true));
        sessionRepository.Setup(x => x.GetGoodHabitsFailed(It.IsAny<string>()))
            .Returns(Task.FromResult<SessionHabitCreditsModel[]?>([new SessionHabitCreditsModel { Id = 1, Credits = 100 }]));
        sessionRepository.Setup(x => x.GetBadHabitsSuccess(It.IsAny<string>()))
            .Returns(Task.FromResult<SessionHabitCreditsModel[]?>([new SessionHabitCreditsModel { Id = 1, Credits = 100 }]));
        sessionRepository.Setup(x => x.FinishCurrentSession(It.IsAny<string>()))
            .Returns(ValueTask.FromResult(true));
        sessionRepository.Setup(x => x.FinishCurrentSession(CANT_FINISH))
            .Returns(ValueTask.FromResult(false));
        sessionRepository.Setup(x => x.GetGoodHabitIds(It.IsAny<string>()))
            .Returns(Task.FromResult<int[]?>([GOOD_ID, GH_NOTACTIVE]));
        sessionRepository.Setup(x => x.GetGoodHabitIds(HAS_BAD_ID))
            .Returns(Task.FromResult<int[]?>([BAD_ID]));
        sessionRepository.Setup(x => x.GetBadHabitIds(It.IsAny<string>()))
            .Returns(Task.FromResult<int[]?>([GOOD_ID]));
        sessionRepository.Setup(x => x.GetBadHabitIds(HAS_BAD_ID))
            .Returns(Task.FromResult<int[]?>([BAD_ID]));
        sessionRepository.Setup(x => x.GetTreatIds(It.IsAny<string>()))
            .Returns(Task.FromResult<int[]?>([GOOD_ID]));
        sessionRepository.Setup(x => x.GetTreatIds(HAS_BAD_ID))
            .Returns(Task.FromResult<int[]?>([BAD_ID]));
        sessionRepository.Setup(x => x.GetGoodHabitCompleted(It.IsAny<int>(), It.IsAny<string>()))
            .Returns(Task.FromResult<bool?>(true));
        sessionRepository.Setup(x => x.GetGoodHabitCompleted(It.IsAny<int>(), MODEL_FAIL))
            .Returns(Task.FromResult<bool?>(false));
        sessionRepository.Setup(x => x.GetBadHabitFailed(It.IsAny<int>(), It.IsAny<string>()))
            .Returns(Task.FromResult<bool?>(true));
        sessionRepository.Setup(x => x.GetBadHabitFailed(It.IsAny<int>(), MODEL_FAIL))
            .Returns(Task.FromResult<bool?>(false));
        sessionRepository.Setup(x => x.GetTreatUnitsLeft(It.IsAny<int>(), It.IsAny<string>()))
            .Returns(Task.FromResult<byte?>(ApplicationInvariants.TreatQuantityPerSessionMax));
        sessionRepository.Setup(x => x.GetTreatUnitsLeft(It.IsAny<int>(), NOUNITS))
            .Returns(Task.FromResult<byte?>(ApplicationInvariants.TreatQuantityPerSessionMin));
        sessionRepository.Setup(x => x.UpdateGoodHabit(It.IsAny<int>(), It.IsAny<bool>(), It.IsAny<string>()))
            .Returns(Task.FromResult(true));
        sessionRepository.Setup(x => x.UpdateGoodHabit(It.IsAny<int>(), It.IsAny<bool>(), CANT_UPDATE))
            .Returns(Task.FromResult(false));
        sessionRepository.Setup(x => x.UpdateBadHabit(It.IsAny<int>(), It.IsAny<bool>(), It.IsAny<string>()))
            .Returns(Task.FromResult(true));
        sessionRepository.Setup(x => x.UpdateBadHabit(It.IsAny<int>(), It.IsAny<bool>(), CANT_UPDATE))
            .Returns(Task.FromResult(false));
        sessionRepository.Setup(x => x.IncrementTreat(It.IsAny<int>(), It.IsAny<string>()))
            .Returns(Task.FromResult(true));
        sessionRepository.Setup(x => x.IncrementTreat(It.IsAny<int>(), CANT_UPDATE))
            .Returns(Task.FromResult(false));
        sessionRepository.Setup(x => x.DecrementTreat(It.IsAny<int>(), It.IsAny<string>()))
            .Returns(Task.FromResult(true));
        sessionRepository.Setup(x => x.DecrementTreat(It.IsAny<int>(), CANT_UPDATE))
            .Returns(Task.FromResult(false));
        sessionRepository.Setup(x => x.GetJustRefunds(It.IsAny<string>()))
            .Returns(Task.FromResult<byte?>(ApplicationInvariants.UserDataRefundsMax));
        sessionRepository.Setup(x => x.GetJustRefunds(NOREFUNDS))
            .Returns(Task.FromResult<byte?>(ApplicationInvariants.UserDataRefundsMin));
        sessionRepository.Setup(x => x.DecrementRefunds(It.IsAny<string>()))
            .Returns(Task.FromResult(true));
        sessionRepository.Setup(x => x.DecrementRefunds(CANT_REFUND))
            .Returns(Task.FromResult(false));

        var userDataService = new Mock<IUserDataService>();
        userDataService.Setup(x => x.GetRefundsPerSession(It.IsAny<string>()))
            .Returns(ValueTask.FromResult(new ResponseStruct<byte>(ResponseCode.Success, "", ApplicationInvariants.UserDataRefundsMax)));
        userDataService.Setup(x => x.GetRefundsPerSession(NOREFUNDS))
            .Returns(ValueTask.FromResult(new ResponseStruct<byte>(ResponseCode.Success, "", ApplicationInvariants.UserDataRefundsMin)));
        userDataService.Setup(x => x.GetCredits(It.IsAny<string>()))
            .Returns(ValueTask.FromResult(new ResponseStruct<int>(ResponseCode.Success, "", ApplicationInvariants.UserDataCreditsMax)));
        userDataService.Setup(x => x.GetCredits(NOCREDITS))
            .Returns(ValueTask.FromResult(new ResponseStruct<int>(ResponseCode.Success, "", ApplicationInvariants.UserDataCreditsMin)));

        goodHabitService = new Mock<IGoodHabitService>();
        goodHabitService.Setup(x => x.GetLogicModel(It.IsAny<int>(), It.IsAny<string>()))
            .Returns(ValueTask.FromResult(new Response<GoodHabitLogicModel>(ResponseCode.Success, "", new GoodHabitLogicModel { Id = 1, IsActive = true })));
        goodHabitService.Setup(x => x.GetLogicModel(It.IsAny<int>(), OWNERSHIP))
            .Returns(ValueTask.FromResult(new Response<GoodHabitLogicModel>(ResponseCode.Unauthorized, "")));
        goodHabitService.Setup(x => x.GetLogicModel(It.IsAny<int>(), NOTFOUND))
            .Returns(ValueTask.FromResult(new Response<GoodHabitLogicModel>(ResponseCode.NotFound, "")));
        goodHabitService.Setup(x => x.GetLogicModel(GH_NOTACTIVE, It.IsAny<string>()))
            .Returns(ValueTask.FromResult(new Response<GoodHabitLogicModel>(ResponseCode.Success, "", new GoodHabitLogicModel { Id = 1, IsActive = false })));
        goodHabitService.Setup(x => x.GetAllIds(It.IsAny<string>(), It.IsAny<bool>()))
            .Returns(ValueTask.FromResult(new Response<int[]>(ResponseCode.Success, "")));

        badHabitService = new Mock<IBadHabitService>();
        badHabitService.Setup(x => x.GetLogicModel(It.IsAny<int>(), It.IsAny<string>()))
            .Returns(ValueTask.FromResult(new Response<BadHabitLogicModel>(ResponseCode.Success, "", new BadHabitLogicModel { Id = 1})));
        badHabitService.Setup(x => x.GetLogicModel(It.IsAny<int>(), OWNERSHIP))
            .Returns(ValueTask.FromResult(new Response<BadHabitLogicModel>(ResponseCode.Unauthorized, "")));
        badHabitService.Setup(x => x.GetLogicModel(It.IsAny<int>(), NOTFOUND))
            .Returns(ValueTask.FromResult(new Response<BadHabitLogicModel>(ResponseCode.NotFound, "")));
        badHabitService.Setup(x => x.GetAllIds(It.IsAny<string>()))
            .Returns(ValueTask.FromResult(new Response<int[]>(ResponseCode.Success, "")));

        treatService = new Mock<ITreatService>();
        treatService.Setup(x => x.GetLogicModel(It.IsAny<int>(), It.IsAny<string>()))
            .Returns(ValueTask.FromResult(new Response<TreatLogicModel>(ResponseCode.Success, "", new TreatLogicModel { Id = 1, Price = 100, UnitsPerSession = ApplicationInvariants.TreatQuantityPerSessionMax})));
        treatService.Setup(x => x.GetLogicModel(It.IsAny<int>(), OWNERSHIP))
            .Returns(ValueTask.FromResult(new Response<TreatLogicModel>(ResponseCode.Unauthorized, "")));
        treatService.Setup(x => x.GetLogicModel(It.IsAny<int>(), NOTFOUND))
            .Returns(ValueTask.FromResult(new Response<TreatLogicModel>(ResponseCode.NotFound, "")));
        treatService.Setup(x => x.GetAllIdAndUnitsLeftPairs(It.IsAny<string>()))
            .Returns(ValueTask.FromResult(new Response<Tuple<int,byte>[]>(ResponseCode.Success, "")));

        sessionService = new SessionService(sessionRepository.Object, unitOfWork.Object, userDataService.Object,
            goodHabitService.Object, badHabitService.Object, treatService.Object);

        observer = new Mock<ISessionObserver>();
        observer.Setup(x => x.NotifyWhenMakeTransaction(It.IsAny<MakeTransactionInfo>()))
            .Returns((MakeTransactionInfo info) => ValueTask.FromResult(info.UserId == TRANSACTION_FAIL ? new Response(ResponseCode.RepositoryError, "") : new Response(ResponseCode.Success, "")));
        sessionService.SubscribeToMakeTransaction(observer.Object);

    }

    [Test]
    public void SubscriptionTest()
    {
        //add null observer
        Assert.Throws<ArgumentNullException>(() =>
        {
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            sessionService.SubscribeToMakeTransaction(null);
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
        });

        //delete null observer
        Assert.Throws<ArgumentNullException>(() =>
        {
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            sessionService.UnsubscribeToMakeTransaction(null);
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
        });

        //actually remove
        Assert.DoesNotThrow(() => sessionService.UnsubscribeToMakeTransaction(observer.Object));
    }

    [Test]
    public async Task StartNewSessionTest()
    {
        const string NOT_FINISHED = "Finished";
        sessionRepository.Setup(x => x.GetIsLastSessionFinished(It.IsAny<string>()))
            .Returns(Task.FromResult<SessionIsFinishedModel?>(new SessionIsFinishedModel { Id = 1, IsFinished = true }));
        sessionRepository.Setup(x => x.GetIsLastSessionFinished(NOT_FINISHED))
            .Returns(Task.FromResult<SessionIsFinishedModel?>(new SessionIsFinishedModel { Id = 1, IsFinished = false }));

        //happy
        var res = await sessionService.StartNewSession(NEUTRAL);
        Assert.That(res.Code, Is.EqualTo(ResponseCode.Success));

        //not finished
        res = await sessionService.StartNewSession(NOT_FINISHED);
        Assert.That(res.Code, Is.EqualTo(ResponseCode.InvalidOperation));

        //cant start repo

        res = await sessionService.StartNewSession(CANT_START);
        Assert.That(res.Code, Is.EqualTo(ResponseCode.RepositoryError));
    }

    [Test]
    public async Task FinishSessionTest()
    {

        //happy
        var res = await sessionService.FinishCurrentSession(NEUTRAL);
        Assert.That(res.Code, Is.EqualTo(ResponseCode.Success));

        //finished (invalid)
        res = await sessionService.FinishCurrentSession(FINISHED);
        Assert.That(res.Code, Is.EqualTo(ResponseCode.InvalidOperation));

        //cant start repo
        res = await sessionService.FinishCurrentSession(CANT_FINISH);
        Assert.That(res.Code, Is.EqualTo(ResponseCode.RepositoryError));

        //transaction fail
        res = await sessionService.FinishCurrentSession(TRANSACTION_FAIL);
        Assert.That(res.Code, Is.EqualTo(ResponseCode.ServiceError));

    }

    [Test]
    public async Task UpdateGoodHabitTest()
    {
        //happy
        var res = await sessionService.UpdateGoodHabit(GOOD_ID, true, MODEL_SUCCESS);
        Assert.That(res.Code, Is.EqualTo(ResponseCode.Success));
        res = await sessionService.UpdateGoodHabit(GOOD_ID, false, MODEL_SUCCESS);
        Assert.That(res.Code, Is.EqualTo(ResponseCode.Success));
        res = await sessionService.UpdateGoodHabit(GOOD_ID, true, MODEL_FAIL);
        Assert.That(res.Code, Is.EqualTo(ResponseCode.Success));
        res = await sessionService.UpdateGoodHabit(GOOD_ID, false, MODEL_FAIL);
        Assert.That(res.Code, Is.EqualTo(ResponseCode.Success));

        //finished (invalid)
        res = await sessionService.UpdateGoodHabit(GOOD_ID, true, FINISHED);
        Assert.That(res.Code, Is.EqualTo(ResponseCode.InvalidOperation));

        //id not found
        res = await sessionService.UpdateGoodHabit(GOOD_ID, true, HAS_BAD_ID);
        Assert.That(res.Code, Is.EqualTo(ResponseCode.NotFound));
        res = await sessionService.UpdateGoodHabit(BAD_ID, true, NEUTRAL);
        Assert.That(res.Code, Is.EqualTo(ResponseCode.NotFound));

        //LogicModel cannot be fetched
        res = await sessionService.UpdateGoodHabit(NOT_FOUND, true, NEUTRAL);
        Assert.That(res.Code, Is.EqualTo(ResponseCode.NotFound));
        res = await sessionService.UpdateGoodHabit(GOOD_ID, true, OWNERSHIP);
        Assert.That(res.Code, Is.EqualTo(ResponseCode.Unauthorized));

        //if not active GH
        res = await sessionService.UpdateGoodHabit(GH_NOTACTIVE, true, NEUTRAL);
        Assert.That(res.Code, Is.EqualTo(ResponseCode.InvalidOperation));

        //Transaction fail
        res = await sessionService.UpdateGoodHabit(GOOD_ID, false, TRANSACTION_FAIL);
        Assert.That(res.Code, Is.EqualTo(ResponseCode.ServiceError));

        //Can't Update
        res = await sessionService.UpdateGoodHabit(GOOD_ID, false, CANT_UPDATE);
        Assert.That(res.Code, Is.EqualTo(ResponseCode.RepositoryError));
    }

    [Test]
    public async Task UpdateBadHabitTest()
    {
        //happy
        var res = await sessionService.UpdateBadHabit(GOOD_ID, true, MODEL_SUCCESS);
        Assert.That(res.Code, Is.EqualTo(ResponseCode.Success));
        res = await sessionService.UpdateBadHabit(GOOD_ID, false, MODEL_SUCCESS);
        Assert.That(res.Code, Is.EqualTo(ResponseCode.Success));
        res = await sessionService.UpdateBadHabit(GOOD_ID, true, MODEL_FAIL);
        Assert.That(res.Code, Is.EqualTo(ResponseCode.Success));
        res = await sessionService.UpdateBadHabit(GOOD_ID, false, MODEL_FAIL);
        Assert.That(res.Code, Is.EqualTo(ResponseCode.Success));

        //finished (invalid)
        res = await sessionService.UpdateBadHabit(GOOD_ID, true, FINISHED);
        Assert.That(res.Code, Is.EqualTo(ResponseCode.InvalidOperation));

        //id not found
        res = await sessionService.UpdateBadHabit(GOOD_ID, true, HAS_BAD_ID);
        Assert.That(res.Code, Is.EqualTo(ResponseCode.NotFound));
        res = await sessionService.UpdateBadHabit(BAD_ID, true, NEUTRAL);
        Assert.That(res.Code, Is.EqualTo(ResponseCode.NotFound));

        //LogicModel cannot be fetched
        res = await sessionService.UpdateBadHabit(NOT_FOUND, true, NEUTRAL);
        Assert.That(res.Code, Is.EqualTo(ResponseCode.NotFound));
        res = await sessionService.UpdateBadHabit(GOOD_ID, true, OWNERSHIP);
        Assert.That(res.Code, Is.EqualTo(ResponseCode.Unauthorized));

        //Transaction fail
        res = await sessionService.UpdateBadHabit(GOOD_ID, false, TRANSACTION_FAIL);
        Assert.That(res.Code, Is.EqualTo(ResponseCode.ServiceError));

        //Can't Update
        res = await sessionService.UpdateBadHabit(GOOD_ID, false, CANT_UPDATE);
        Assert.That(res.Code, Is.EqualTo(ResponseCode.RepositoryError));
    }

    [Test]
    public async Task BuyTreatTest()
    {
        //happy
        var res = await sessionService.BuyTreat(GOOD_ID, NEUTRAL);
        Assert.That(res.Code, Is.EqualTo(ResponseCode.Success));

        //finished (invalid)
        res = await sessionService.BuyTreat(GOOD_ID, FINISHED);
        Assert.That(res.Code, Is.EqualTo(ResponseCode.InvalidOperation));

        //id not found
        res = await sessionService.BuyTreat(GOOD_ID, HAS_BAD_ID);
        Assert.That(res.Code, Is.EqualTo(ResponseCode.NotFound));
        res = await sessionService.BuyTreat(BAD_ID, NEUTRAL);
        Assert.That(res.Code, Is.EqualTo(ResponseCode.NotFound));

        //no units left
        res = await sessionService.BuyTreat(GOOD_ID, NOUNITS);
        Assert.That(res.Code, Is.EqualTo(ResponseCode.InvalidOperation));

        //LogicModel cannot be fetched
        res = await sessionService.BuyTreat(NOT_FOUND, NEUTRAL);
        Assert.That(res.Code, Is.EqualTo(ResponseCode.NotFound));
        res = await sessionService.BuyTreat(GOOD_ID, OWNERSHIP);
        Assert.That(res.Code, Is.EqualTo(ResponseCode.Unauthorized));

        //Not enough credits
        res = await sessionService.BuyTreat(GOOD_ID, NOCREDITS);
        Assert.That(res.Code, Is.EqualTo(ResponseCode.InvalidOperation));

        //Transaction fail
        res = await sessionService.BuyTreat(GOOD_ID, TRANSACTION_FAIL);
        Assert.That(res.Code, Is.EqualTo(ResponseCode.ServiceError));

        //Can't Update
        res = await sessionService.BuyTreat(GOOD_ID, CANT_UPDATE);
        Assert.That(res.Code, Is.EqualTo(ResponseCode.RepositoryError));
    }

    [Test]
    public async Task RefundTreatTest()
    {
        const string MAXUNITS = "MaxUnits";
        treatService.Setup(x => x.GetLogicModel(It.IsAny<int>(), It.IsAny<string>()))
            .Returns(ValueTask.FromResult(new Response<TreatLogicModel>(ResponseCode.Success, "", new TreatLogicModel { Id = 1, Price = 100, UnitsPerSession = ApplicationInvariants.TreatQuantityPerSessionMin})));
        treatService.Setup(x => x.GetLogicModel(It.IsAny<int>(), MAXUNITS))
            .Returns(ValueTask.FromResult(new Response<TreatLogicModel>(ResponseCode.Success, "", new TreatLogicModel { Id = 1, Price = 100, UnitsPerSession = ApplicationInvariants.TreatQuantityPerSessionMax})));
        treatService.Setup(x => x.GetLogicModel(It.IsAny<int>(), OWNERSHIP))
            .Returns(ValueTask.FromResult(new Response<TreatLogicModel>(ResponseCode.Unauthorized, "")));
        treatService.Setup(x => x.GetLogicModel(It.IsAny<int>(), NOTFOUND))
            .Returns(ValueTask.FromResult(new Response<TreatLogicModel>(ResponseCode.NotFound, "")));

        //happy
        var res = await sessionService.RefundTreat(GOOD_ID, NEUTRAL);
        Assert.That(res.Code, Is.EqualTo(ResponseCode.Success));

        //finished (invalid)
        res = await sessionService.RefundTreat(GOOD_ID, FINISHED);
        Assert.That(res.Code, Is.EqualTo(ResponseCode.InvalidOperation));

        //id not found
        res = await sessionService.RefundTreat(GOOD_ID, HAS_BAD_ID);
        Assert.That(res.Code, Is.EqualTo(ResponseCode.NotFound));
        res = await sessionService.RefundTreat(BAD_ID, NEUTRAL);
        Assert.That(res.Code, Is.EqualTo(ResponseCode.NotFound));

        //LogicModel cannot be fetched
        res = await sessionService.RefundTreat(NOT_FOUND, NEUTRAL);
        Assert.That(res.Code, Is.EqualTo(ResponseCode.NotFound));
        res = await sessionService.RefundTreat(GOOD_ID, OWNERSHIP);
        Assert.That(res.Code, Is.EqualTo(ResponseCode.Unauthorized));

        //max units reached
        res = await sessionService.RefundTreat(GOOD_ID, MAXUNITS);
        Assert.That(res.Code, Is.EqualTo(ResponseCode.InvalidOperation));

        //Not enough refunds
        res = await sessionService.RefundTreat(GOOD_ID, NOREFUNDS);
        Assert.That(res.Code, Is.EqualTo(ResponseCode.InvalidOperation));

        //Transaction fail
        res = await sessionService.RefundTreat(GOOD_ID, TRANSACTION_FAIL);
        Assert.That(res.Code, Is.EqualTo(ResponseCode.ServiceError));

        //Can't Update or Refund
        res = await sessionService.RefundTreat(GOOD_ID, CANT_UPDATE);
        Assert.That(res.Code, Is.EqualTo(ResponseCode.RepositoryError));
        res = await sessionService.RefundTreat(GOOD_ID, CANT_REFUND);
        Assert.That(res.Code, Is.EqualTo(ResponseCode.RepositoryError));
    }

    [Test]
    public async Task GetsTest()
    {
        ////Test Exists
        //Assert.True(await sessionService.Exists(SUCCESS));
        //Assert.False(await sessionService.Exists(NOT_FOUND));

        ////Test IsOwnerOf
        //Assert.True(await sessionService.IsOwnerOf(SUCCESS, NEUTRAL));
        //Assert.False(await sessionService.IsOwnerOf(SUCCESS, OWNERSHIP));


        ////////// Test GetAll and GetAllIds ////////

        ////GetAll
        //var r1 = await sessionService.GetAll(NEUTRAL);
        //Assert.That(r1.Code, Is.EqualTo(ResponseCode.Success));

        ////GetAllIds
        //var r2 = await sessionService.GetAllIds(NEUTRAL);
        //Assert.That(r1.Code, Is.EqualTo(ResponseCode.Success));


        ////////// Test GetInputModel ////////

        ////Success
        //var r3 = await sessionService.GetInputModel(SUCCESS, NEUTRAL);
        //Assert.That(r3.Code, Is.EqualTo(ResponseCode.Success));

        ////not exists
        //r3 = await sessionService.GetInputModel(NOT_FOUND, NEUTRAL);
        //Assert.That(r3.Code, Is.EqualTo(ResponseCode.NotFound));

        ////not exists
        //r3 = await sessionService.GetInputModel(SUCCESS, OWNERSHIP);
        //Assert.That(r3.Code, Is.EqualTo(ResponseCode.Unauthorized));


        ////////// Test GetLogicModel ////////

        ////Success
        //var r4 = await sessionService.GetLogicModel(SUCCESS, NEUTRAL);
        //Assert.That(r4.Code, Is.EqualTo(ResponseCode.Success));

        ////not exists
        //r4 = await sessionService.GetLogicModel(NOT_FOUND, NEUTRAL);
        //Assert.That(r4.Code, Is.EqualTo(ResponseCode.NotFound));

        ////not exists
        //r4 = await sessionService.GetLogicModel(SUCCESS, OWNERSHIP);
        //Assert.That(r4.Code, Is.EqualTo(ResponseCode.Unauthorized));
    }
}
