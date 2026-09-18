namespace AVMLabs.Mvc.Models.Tests
{
    public class TestResponseModel
    {
        public int TestId { get; set; }
        public string TestName { get; set; } = string.Empty;
        public decimal Rate { get; set; }
        public bool IsOST { get; set; }
        public string SampleType { get; set; } = string.Empty;
    }
}