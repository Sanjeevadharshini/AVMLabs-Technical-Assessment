using System.ComponentModel.DataAnnotations;

namespace AVMLabs.Api.DTOs.WorkOrders
{
    public class CreateWorkOrderItemDto
    {
        [Range(1, int.MaxValue)]
        public int TestId { get; set; }

        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }
    }
}