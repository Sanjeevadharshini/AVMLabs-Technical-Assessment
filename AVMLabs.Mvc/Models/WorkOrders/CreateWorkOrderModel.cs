namespace AVMLabs.Mvc.Models.WorkOrders
{
    public class CreateWorkOrderModel
    {
        public int ClientId { get; set; }
        public DateTime WODate { get; set; } = DateTime.Today;
        public string CreatedBy { get; set; } = string.Empty;
        public List<CreateWorkOrderItemModel> Items { get; set; } = new();
    }
}