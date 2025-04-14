using System;

namespace Domain.Entities;

public class Customer
{
    public int Id { get; private set; }
    public required string FullName { get; set; }
    public required string Email { get; set; } // Email será o login principal?
    public required string Document { get; set; } // CPF
    public string? Phone { get; set; }
    public DateTime BirthDate { get; set; }
    // Endereço pode ser um Value Object depois
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? ZipCode { get; set; }
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

    // Navigation Properties
    public ICollection<Order> Orders { get; private set; } = new List<Order>();
    // Poderíamos ter Tickets aqui também, mas talvez seja melhor acessar via Order
}