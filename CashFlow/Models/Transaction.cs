using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CashFlow.Models
{
    public class Transaction
    {
        public int Id { get; set; }

        [Required]
        public int Ammount { get; set; }

        public string Note { get; set; }

        public DateTime Date { get; set; }

        public int UserProfileId { get; set; }

        public Category Category { get; set; }

    }
}
