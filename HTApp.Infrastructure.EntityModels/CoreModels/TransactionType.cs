using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HTApp.Infrastructure.EntityModels.Core
{
    internal class TransactionType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [MaxLength(64)]
        public string? Message { get; set; }
    }

    ////////////////////////////////////////////////////////
    //   WARNING / ATTENTION / NOTE / DANGER / IMPORTANT  //
    ////////////////////////////////////////////////////////
    // PLEASE KEEP THE ENUM AND THE CONFIGURATION IN SYNC //
    ////////////////////////////////////////////////////////


    //Bad name, sorry! ;) Oh well! ;]
    public enum TransactionEnum
    {
        Unknown = 0,
        Manual = 1,
        DeletedTransactionType = 2, // Wow, backwards compatibility is not your cup of tea! Don't forget to change the nulls in the DB!

        GoodHabitSuccess = 100,
        GoodHabitFail = 101,
        GoodHabitSuccessChangeOfMind = 102,

        BadHabitSuccess = 200,
        BadHabitFail = 201,
        BadHabitFailChangeOfMind = 202,

        BuyingTreat = 300,
        RefundTreat = 301,
    }

    public class TransactionTypeConfiguration
        : IEntityTypeConfiguration<TransactionType>
    {
        private static TransactionType newTransactionType(int id, string desc)
        {
            return new TransactionType { Id = id, Message = desc };
        }

        public void Configure(EntityTypeBuilder<TransactionType> builder)
        {
            builder.HasData(
                newTransactionType(000, "Credits From Unknown Source"),
                newTransactionType(001, "Credits From Manual Insertion"),
                newTransactionType(002, "Credits From ... AHHHH I DON'T REMEMBER"), //HOPEFULLY NEVER, FOR GOODNESS SAKE
                newTransactionType(100, "Credits Earned From Finishing a Good Habit"),
                newTransactionType(101, "Credits Lost From Failing a Good Habit"),
                newTransactionType(102, "Credits Lost From Change of Status of a Good Habit"),
                newTransactionType(200, "Credits Eearned From Quitting a Bad Habit"),
                newTransactionType(201, "Credits Lost From Failing To Quit a Bad Habit"),
                newTransactionType(202, "Credits Earned From Change Of Status of a Bad Habit"),
                newTransactionType(300, "Credits Lost From Buying a Treat. Cheers!"),
                newTransactionType(301, "Credits Earned From Refunding a Treat. Uncheers!")
            );
        }
    }
}
