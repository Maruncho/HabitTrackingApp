﻿namespace HTApp.Core.Contracts;

public class SessionModel<ModelId, GdHId, BdHId, TrsId, TrtId>
{
    public required ModelId Id { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime? EndData { get; set; }

    public byte Refunds { get; set; }

    public required (GdHId, bool)[] GoodHabitIdSuccessPairs { get; set; }

    public required (BdHId, bool)[] BadHabitIdFailPairs { get; set; }

    public required TrsId[] TransactionIds { get; set; }

    public required (TrtId, byte)[] TreatIdUnitsLeftPairs { get; set; }
}
