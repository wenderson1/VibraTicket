namespace Application.Query.Affiliate.GetAffiliateByDocument
{
    public class GetAffiliateByDocumentOutput
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Document { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? BankName { get; set; }
        public string? BankAccount { get; set; }
        public string? BankBranch { get; set; }
        public decimal DefaultCommissionRate { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}