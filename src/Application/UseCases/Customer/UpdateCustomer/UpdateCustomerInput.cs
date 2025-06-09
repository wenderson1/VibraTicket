namespace Application.UseCases.Customer.UpdateCustomer
{
    public class UpdateCustomerInput
    {
        public string? FullName { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Document { get; set; }
        public string? Phone { get; set; }
        public DateTime? BirthDate { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? ZipCode { get; set; }
        public bool? IsActive { get; set; }
    }
}