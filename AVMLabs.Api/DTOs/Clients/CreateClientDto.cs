using System.ComponentModel.DataAnnotations;

namespace AVMLabs.Api.DTOs.Clients
{
    public class CreateClientDto
    {
        [Required]
        [MaxLength(150)]
        public string ClientName { get; set; } = string.Empty;

        [Required]
        [MaxLength(150)]
        public string ContactPerson { get; set; } = string.Empty;

        [Required]
        [MaxLength(20)]
        public string Phone { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [MaxLength(150)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string City { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string Country { get; set; } = string.Empty;

        [Range(typeof(decimal), "0.01", "79228162514264337593543950335")]
        public decimal CreditLimit { get; set; }
    }
}