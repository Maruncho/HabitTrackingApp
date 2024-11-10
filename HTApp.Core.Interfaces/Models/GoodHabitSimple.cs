﻿namespace HTApp.Core.Contracts;

public class GoodHabitSimple
{
    public int Id { get; set; }

    public required string Name { get; set; }

    public int CreditsSuccess { get; set; }

    public int CreditsFail { get; set; }

    public bool IsActive { get; set; } = true;
}
