﻿namespace HTApp.Core.Contracts;

public class BadHabitModel<ModelId>
{
    public required ModelId Id { get; set; }

    public required string Name { get; set; }

    public int CreditsSuccess { get; set; }

    public int CreditsFail { get; set; }
}
