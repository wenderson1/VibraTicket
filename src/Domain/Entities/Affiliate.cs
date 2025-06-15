namespace Domain.Entities;

public class Affiliate
{
    public int Id { get; private set; } // Usar long pode ser bom para IDs que podem crescer muito
    public required string Name { get; set; } // Nome fantasia/público
    public required string FullName { get; set; } // Razão Social / Nome Completo
    public required string Document { get; set; } // CNPJ ou CPF
    public string? Email { get; set; }
    public string? Phone { get; set; }
    // Informações bancárias podem ser uma entidade separada ou Value Object depois
    public string? BankName { get; set; }
    public string? BankAccount { get; set; }
    public string? BankBranch { get; set; }
    public decimal DefaultCommissionRate { get; set; }
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
    public bool IsActive { get; set; } = true;

    // Navigation Property
    public ICollection<Event> Events { get; private set; } = new List<Event>();
}