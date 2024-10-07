﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Budget_Tracker.Models
{
    public class Transaction
    {
        [Key]
        public int TransactionId { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public int Amount { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        public string Note { get; set; }

        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; } = DateTime.Now;

        [NotMapped]
        public string? CategoryTitleWithIcon
        {
            get
            {
                return Category == null ? "" : $"{Category.Title}{Category.Icon}";
            }
        }


        [NotMapped]
        public string? FormattedAmount
        {
            get
            {
                return (Category == null || Category.Type == "Expense") ? "- " + Amount.ToString("C") : "+ " + Amount.ToString("C");
            }
        }

    }
}
