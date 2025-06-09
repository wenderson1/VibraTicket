namespace Application.UseCases.Customer.CreateCustomer
{
    public class CreateCustomerInput
    {
        public string FullName { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Document { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public DateTime BirthDate { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? ZipCode { get; set; }
    }
}