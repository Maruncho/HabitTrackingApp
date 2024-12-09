using HTApp.Core.API;

namespace HTApp.Web.MVC.Models;

public class TransactionsViewModel
{
    public int UserCredits { get; set; }

    public int PageBeginIndex { get; set; }

    public int PageCount { get; set; }

    public int PageNumber { get; set; }

    public bool HasNext { get; set; }

    public string FilterTypeName { get; set; } = null!;

    public bool FromLastSession { get; set; }

    public TransactionModel[] Models { get; set; } = null!;

    public string[] TypeNames { get; set; } = null!;
}
