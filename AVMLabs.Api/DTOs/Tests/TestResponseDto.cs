using System.ComponentModel.DataAnnotations;

namespace AVMLabs.Api.DTOs.Tests
{
    public class TestResponseDto
    {
        public int TestId { get; set; }
        public string TestCode { get; set; } = string.Empty;
        public string TestName { get; set; } = string.Empty;
        public string SampleType { get; set; } = string.Empty;
        public decimal Rate { get; set; }
        public bool IsOST { get; set; }
    }
}