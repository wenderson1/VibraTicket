using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities; // Namespace da entidade Ticket
using Domain.Enums;   // Namespace do Enum TicketStatus

namespace Infrastructure.Persistence.Configurations
{
    public class TicketConfiguration : IEntityTypeConfiguration<Ticket>
    {
        public void Configure(EntityTypeBuilder<Ticket> builder)
        {
            // Define a chave primária (Guid)
            builder.HasKey(t => t.Id);

            // Configura a propriedade TicketNumber
            builder.Property(t => t.TicketNumber)
                .IsRequired()
                .HasMaxLength(50); // Ajuste o tamanho conforme o formato do número do ticket

            // Adiciona um índice único para TicketNumber DENTRO de um Evento
            builder.HasIndex(t => new { t.EventId, t.TicketNumber })
                   .IsUnique();

            // Configura a propriedade Price
            builder.Property(t => t.Price)
                .IsRequired()
                .HasPrecision(10, 2); // Ajuste a precisão conforme necessário

            // Configura a propriedade Status (Enum)
            builder.Property(t => t.Status)
                .IsRequired()
                .HasConversion<string>() // Armazena como string no banco
                .HasMaxLength(20);       // Tamanho suficiente para os nomes dos status

            // Configura a propriedade QrCodeData (opcional)
            // Pode armazenar o próprio dado ou uma referência/hash
            builder.Property(t => t.QrCodeData)
                .HasMaxLength(500); // Ajuste o tamanho conforme necessário

            // Configura a propriedade IsUsed
            builder.Property(t => t.IsUsed)
                .IsRequired()
                .HasDefaultValue(false); // Garante que novos tickets não estão usados

            // Configura a propriedade UsedDate (opcional)
            builder.Property(t => t.UsedDate); // Já é nullable DateTime?

            // Configura a propriedade CreatedAt
            // O valor padrão já é definido na entidade
            builder.Property(t => t.CreatedAt)
                .IsRequired();

            // --- Relacionamentos ---

            // Relacionamento com Event (Muitos Tickets pertencem a 1 Event)
            builder.HasOne(t => t.Event)
                   .WithMany(e => e.Tickets) // 1 Event tem muitos Tickets
                   .HasForeignKey(t => t.EventId) // Chave estrangeira em Ticket
                   .IsRequired() // EventId não pode ser nulo
                   .OnDelete(DeleteBehavior.Restrict); // Impede deletar Event se tiver Tickets

            // Relacionamento com Sector (Muitos Tickets pertencem a 1 Sector)
            builder.HasOne(t => t.Sector)
                   .WithMany(s => s.Tickets) // 1 Sector tem muitos Tickets
                   .HasForeignKey(t => t.SectorId) // Chave estrangeira em Ticket
                   .IsRequired() // SectorId não pode ser nulo
                   .OnDelete(DeleteBehavior.Restrict); // Impede deletar Sector se tiver Tickets

            // Relacionamento com Customer (Muitos Tickets podem pertencer a 1 Customer)
            builder.HasOne(t => t.Customer)
                   .WithMany() // Não precisamos de uma coleção de Tickets em Customer por padrão
                   .HasForeignKey(t => t.CustomerId) // Chave estrangeira em Ticket (nullable)
                   .IsRequired(false) // CustomerId PODE ser nulo
                   .OnDelete(DeleteBehavior.SetNull); // Se o Customer for deletado (soft delete), desvincula o ticket (define CustomerId = NULL)

            // Relacionamento com Order (Muitos Tickets podem pertencer a 1 Order)
            builder.HasOne(t => t.Order)
                   .WithMany(o => o.Tickets) // 1 Order tem muitos Tickets
                   .HasForeignKey(t => t.OrderId) // Chave estrangeira em Ticket (nullable)
                   .IsRequired(false) // OrderId PODE ser nulo
                   .OnDelete(DeleteBehavior.SetNull); // Se a Order for deletada (soft delete), desvincula o ticket (define OrderId = NULL)

            // --- Índices Adicionais (Opcionais) ---
            builder.HasIndex(t => t.Status); // Para buscar tickets por status
            builder.HasIndex(t => t.CustomerId); // Para buscar tickets de um cliente
            builder.HasIndex(t => t.OrderId); // Para buscar tickets de uma ordem
        }
    }
}