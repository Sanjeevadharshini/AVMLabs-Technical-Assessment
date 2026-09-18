namespace AVMLabs.Mvc.Models.Clients
{
    public class ClientFormModel
    {
        public int ClientId { get; set; }
        public string ClientName { get; set; } = string.Empty;
        public string ContactPerson { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public decimal CreditLimit { get; set; }
    }
}