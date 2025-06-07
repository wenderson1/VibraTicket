namespace Application.UseCases.Affiliate.UpdateAffiliate
{
    public class UpdateAffiliateInput
    {
        public string? Name { get; set; }
        public string? FullName { get; set; }
        public string? Document { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? BankName { get; set; }
        public string? BankAccount { get; set; }
        public string? BankBranch { get; set; }
        public decimal? DefaultCommissionRate { get; set; }
    }
}