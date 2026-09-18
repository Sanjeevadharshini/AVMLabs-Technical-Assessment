using System.ComponentModel.DataAnnotations;

namespace AVMLabs.Api.DTOs.WorkOrders
{
    public class UpdateWorkOrderStatusDto
    {
        [Required]
        public string Status { get; set; } = string.Empty;
    }
}