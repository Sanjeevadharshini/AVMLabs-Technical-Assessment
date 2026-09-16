using System.ComponentModel.DataAnnotations;

namespace AVMLabs.Api.DTOs.Payments
{
    public class CreatePaymentDto
    {
        [Range(1, int.MaxValue)]
        public int InvoiceId { get; set; }

        [Required]
        public DateTime PaymentDate { get; set; }

        [Range(typeof(decimal), "0.01", "79228162514264337593543950335")]
        public decimal Amount { get; set; }

        [Required]
        public string Mode { get; set; } = string.Empty;
    }
}