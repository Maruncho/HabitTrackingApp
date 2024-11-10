using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HTApp.Infrastructure.EntityModels.Core
{
    public class GoodHabitSimple
    {
        public int Id { get; set; }

        public required string Name { get; set; }

        public int CreditsSuccess { get; set; }

        public int CreditsFail { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
