using System.ComponentModel.DataAnnotations;

namespace AVMLabs.Api.DTOs.WorkOrders
{
    public class CreateWorkOrderDto
    {
        [Range(1, int.MaxValue)]
        public int ClientId { get; set; }

        [Required]
        public DateTime WODate { get; set; }

        [Required]
        [MaxLength(100)]
        public string CreatedBy { get; set; } = string.Empty;

        [Required]
        [MinLength(1)]
        public List<CreateWorkOrderItemDto> Items { get; set; } = new();
    }
}