using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTApp.Core.Contracts;

public static class ApplicationInvariants
{
    //UserData
    public const int UserDataCreditsMin = 0;
    public const int UserDataCreditsMax = int.MaxValue;

    public const byte UserDataRefundsMin = 0;
    public const byte UserDataRefundsMax = byte.MaxValue;


    //GoodHabit
    public const int GoodHabitNameLengthMax = 64;
    public const int GoodHabitNameLengthMin = 1;

    public const int GoodHabitCreditsSuccessMin = 0;
    public const int GoodHabitCreditsSuccessMax = int.MaxValue;

    public const int GoodHabitCreditsFailMin = 0;
    public const int GoodHabitCreditsFailMax = int.MaxValue;


    //BadHabit
    public const int BadHabitNameLengthMax = 64;
    public const int BadHabitNameLengthMin = 1;

    public const int BadHabitCreditsSuccessMin = 0;
    public const int BadHabitCreditsSuccessMax = int.MaxValue;

    public const int BadHabitCreditsFailMin = 0;
    public const int BadHabitCreditsFailMax = int.MaxValue;


    //Treat
    public const int TreatNameLengthMax = 64;
    public const int TreatNameLengthMin = 1;

    public const byte TreatQuantityPerSessionMin = 0;
    public const byte TreatQuantityPerSessionMax = byte.MaxValue;

    public const int TreatPriceMin = 0;
    public const int TreatPriceMax = int.MaxValue;


    //Transaction
    public const int TransactionMessageLengthMin = 1;
    public const int TransactionMessageLengthMax = 64;

    public const int TransactionAmountMin = 0;
    public const int TransactionAmountMax = int.MaxValue;
}
