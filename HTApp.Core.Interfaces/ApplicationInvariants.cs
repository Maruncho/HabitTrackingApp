namespace HTApp.Core.API;

public static class ApplicationInvariants
{
    //UserData
    public const int UserDataCreditsMin = int.MinValue; // we will allow negative credits and call it a feature. It's not lazy programmer stuff.
    public const int UserDataCreditsMax = int.MaxValue;
    public static readonly string UserDataCreditsError = $"Credits should be in the range [{UserDataCreditsMin}:{UserDataCreditsMax}]";

    public const byte UserDataRefundsMin = 0;
    public const byte UserDataRefundsMax = byte.MaxValue;
    public static readonly string UserDataRefundsError = $"Refunds should be in the range [{UserDataCreditsMin}:{UserDataCreditsMax}]";


    //GoodHabit
    public const int GoodHabitNameLengthMax = 64;
    public const int GoodHabitNameLengthMin = 1;
    public static readonly string GoodHabitNameLengthError = $"Name length should be in the range [{GoodHabitNameLengthMin}:{GoodHabitNameLengthMax}]";

    public const int GoodHabitCreditsSuccessMin = 0;
    public const int GoodHabitCreditsSuccessMax = int.MaxValue;
    public static readonly string GoodHabitNameCreditsSuccessError = $"Credits should be in the range [{GoodHabitCreditsSuccessMin}:{GoodHabitCreditsSuccessMax}]";

    public const int GoodHabitCreditsFailMin = 0;
    public const int GoodHabitCreditsFailMax = int.MaxValue;
    public static readonly string GoodHabitNameCreditsFailError = $"Credits should be in the range [{GoodHabitCreditsFailMin}:{GoodHabitCreditsFailMax}]";


    //BadHabit
    public const int BadHabitNameLengthMax = 64;
    public const int BadHabitNameLengthMin = 1;
    public static readonly string BadHabitNameLengthError = $"Name length should be in the range [{BadHabitNameLengthMin}:{BadHabitNameLengthMax}]";

    public const int BadHabitCreditsSuccessMin = 0;
    public const int BadHabitCreditsSuccessMax = int.MaxValue;
    public static readonly string BadHabitNameCreditsSuccessError = $"Credits should be in the range [{BadHabitCreditsSuccessMin}:{BadHabitCreditsSuccessMax}]";

    public const int BadHabitCreditsFailMin = 0;
    public const int BadHabitCreditsFailMax = int.MaxValue;
    public static readonly string BadHabitNameCreditsFailError = $"Credits should be in the range [{BadHabitCreditsFailMin}:{BadHabitCreditsFailMax}]";


    //Treat
    public const int TreatNameLengthMax = 64;
    public const int TreatNameLengthMin = 1;
    public static readonly string TreatNameLengthError = $"Name length should be in the range [{TreatNameLengthMin}:{TreatNameLengthMax}]";

    public const byte TreatQuantityPerSessionMin = 0;
    public const byte TreatQuantityPerSessionMax = byte.MaxValue;
    public static readonly string TreatQuantityPerSessionError = $"Quantity should be in the range [{TreatQuantityPerSessionMin}:{TreatQuantityPerSessionMax}]";

    public const int TreatPriceMin = 0;
    public const int TreatPriceMax = int.MaxValue;
    public static readonly string TreatPriceError = $"Price should be in the range [{TreatPriceMin}:{TreatPriceMax}]";


    //Transaction
    public const int TransactionMessageLengthMin = 1;
    public const int TransactionMessageLengthMax = 64;
    public static readonly string TransactionMessageLengthError = $"Message length should be in the range [{TransactionMessageLengthMin}:{TransactionMessageLengthMax}]";

    public const int TransactionAmountMin = 0;
    public const int TransactionAmountMax = int.MaxValue;
    public static readonly string TransactionAmountError = $"Amount should be in the range [{TransactionAmountMin}:{TransactionAmountMax}]";
}
