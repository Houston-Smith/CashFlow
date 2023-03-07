using System.ComponentModel.DataAnnotations;

namespace CashFlow.Models
{
    public class TransactionCategory
    {
        public int Id { get; set; }

        [Required]
        public int TransactionId { get; set; }

        [Required]
        public int CateagoryId { get; set;}
    }
}
