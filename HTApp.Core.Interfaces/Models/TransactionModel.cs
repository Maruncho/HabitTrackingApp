﻿namespace HTApp.Core.Contracts;

public class TransactionModel
{
    public required int Id { get; set; }

    public required string Type { get; set; }

    public required string Message { get; set; }

    public int Amount { get; set; }
}
