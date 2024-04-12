using System.ComponentModel.DataAnnotations;

namespace LateNight.Models
{
    public class Expense
    {
        [Required(ErrorMessage = "The name of the expense is required.")]
        [StringLength(100, ErrorMessage = "The name must be less than 100 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "The amount of the expense is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "The amount must be greater than zero.")]
        public decimal Amount { get; set; }
    }
}