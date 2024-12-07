using HTApp.Core.API;
using HTApp.Infrastructure.EntityModels;
using HTApp.Infrastructure.EntityModels.Core;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace HTApp.Infrastructure.Repositories;

public class TransactionRepository
    : RepositoryImmutableBase<Transaction, int>
    , ITransactionRepository
{
    private static Dictionary<int, string> intToStringEnum =
        Enum.GetValues(typeof(TransactionEnum))
            .Cast<TransactionEnum>()
            .ToDictionary(t => (int)t, t => t.ToString());

    private static Dictionary<string, int> stringToIntEnum =
        Enum.GetValues(typeof(TransactionEnum))
            .Cast<TransactionEnum>()
            .ToDictionary(t => t.ToString(), t => (int)t);


    public TransactionRepository(ApplicationDbContext db) : base(db)
    {
    }

    public Task<TransactionModel[]> GetAll(string userId)
    {
        return GetAll()
            .Where(t => t.UserId == userId)
            .Select(t => new TransactionModel
            {
                Id = t.Id,
                Type = intToStringEnum[t.TypeId],
                Message = t.Type!.Message ?? string.Empty,
                Amount = t.Amount
            })
            .ToArrayAsync();
    }

    public Task<TransactionModel[]> GetAll(string userId, int pageCount, int pageNumber, TransactionOptions? extra = null)
    {
        TransactionOptions opt = extra ?? new TransactionOptions();

        var models = GetAll().Where(t => t.UserId == userId);

        if(opt.FromSessionId is not null)
        {
            models = models.Where(t => t.SessionId == opt.FromSessionId);
        }

        if(!string.IsNullOrEmpty(opt.FilterTypeName))
        {
            models = models.Where(t => t.TypeId == stringToIntEnum[opt.FilterTypeName]);
        }

        return models
            .OrderByDescending(t => t.Id) //that's the order for now
            .Skip(pageCount * (pageNumber - 1))
            .Take(pageCount + opt.AdditionalEntries)
            .Select(t => new TransactionModel
            {
                Id = t.Id,
                Type = intToStringEnum[t.TypeId],
                Message = t.Type!.Message ?? string.Empty,
                Amount = t.Amount
            })
            .ToArrayAsync();
    }

    public Task<int> GetCount(string userId)
    {
        return GetAll().Where(t => t.UserId == userId).CountAsync();
    }

    public Task<string[]> GetUsedTypeNames(string userId)
    {
        return GetAll()
            .Where(t => t.UserId == userId)
            .Select(t => intToStringEnum[t.TypeId])
            .Distinct()
            .ToArrayAsync();
    }

    public ValueTask<bool> Add(TransactionInputModel model)
    {
        Transaction entity = new Transaction
        {
            Amount = model.Amount,
            TypeId = stringToIntEnum[model.Type],
            UserId = model.UserId,
            SessionId = model.SessionId,
        };

        Add(entity);
        return ValueTask.FromResult(true);
    }
    public async ValueTask<bool> IsOwnerOf(int id, string userId)
    {
        var model = await Get(id);
        return model is not null && model.UserId == userId;
    }

    public async ValueTask<bool> Exists(int id)
    {
        return (await Get(id)) is not null;
    }
}
