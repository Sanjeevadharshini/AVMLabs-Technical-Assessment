namespace AVMLabs.Api.Models
{
    public class Test
    {
        public int TestId { get; set; }
        public string TestCode { get; set; } = string.Empty;
        public string TestName { get; set; } = string.Empty;
        public string SampleType { get; set; } = string.Empty;
        public int TATHours { get; set; }
        public decimal Rate { get; set; }
        public bool IsOST { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }

        public ICollection<WorkOrderItem> WorkOrderItems { get; set; } = new List<WorkOrderItem>();
    }
}