using System.ComponentModel.DataAnnotations;

namespace AVMLabs.Api.DTOs.Clients
{
    public class UpdateClientDto
    {
        [Required]
        [StringLength(150)]
        public string ClientName { get; set; } = string.Empty;

        [StringLength(100)]
        public string? ContactPerson { get; set; }

        [Phone]
        [StringLength(20)]
        public string? Phone { get; set; }

        [EmailAddress]
        [StringLength(150)]
        public string? Email { get; set; }

        [StringLength(100)]
        public string? City { get; set; }

        [StringLength(100)]
        public string? Country { get; set; }

        [Range(typeof(decimal), "0.01", "79228162514264337593543950335")]
        public decimal CreditLimit { get; set; }
    }
}