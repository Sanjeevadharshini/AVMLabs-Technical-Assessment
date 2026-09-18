namespace AVMLabs.Api.DTOs.Reports
{
    public class GatewayFeeDto
    {
        public string Mode { get; set; } = string.Empty;
        public decimal TotalGatewayFees { get; set; }
    }
}