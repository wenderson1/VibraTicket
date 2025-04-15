using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities; // Namespace da entidade Sector

namespace Infrastructure.Persistence.Configurations
{
    public class SectorConfiguration : IEntityTypeConfiguration<Sector>
    {
        public void Configure(EntityTypeBuilder<Sector> builder)
        {
            // Define a chave primária
            builder.HasKey(s => s.Id);

            // Configura a propriedade Name
            builder.Property(s => s.Name)
                .IsRequired()
                .HasMaxLength(100); // Ajuste o tamanho conforme necessário

            // Configura a propriedade Price
            builder.Property(s => s.Price)
                .IsRequired() // Implícito para decimal não anulável
                .HasPrecision(10, 2); // Ajuste a precisão (ex: 10, 2 para até 99.999.999,99)

            // Configura a propriedade Capacity
            builder.Property(s => s.Capacity)
                .IsRequired();

            // Configura a propriedade AvailableTickets
            // Nota: Esta propriedade pode ser calculada em tempo real em vez de armazenada.
            // Se for armazenada, a configuração abaixo é válida.
            // Se for calculada, remova-a da entidade ou marque-a como não mapeada: builder.Ignore(s => s.AvailableTickets);
            builder.Property(s => s.AvailableTickets)
                .IsRequired();

            // Configura o relacionamento com Event (Muitos Sectors pertencem a 1 Event)
            builder.HasOne(s => s.Event)
                   .WithMany(e => e.Sectors) // 1 Event tem muitos Sectors
                   .HasForeignKey(s => s.EventId) // Chave estrangeira em Sector
                   .IsRequired() // Garante que EventId não pode ser nulo
                   .OnDelete(DeleteBehavior.Cascade); // Se o Event for deletado, deletar os Sectors associados
                                                      // (Faz sentido, pois um setor não existe sem um evento)

            // Configura o relacionamento com Ticket (1 Sector tem muitos Tickets)
            builder.HasMany(s => s.Tickets)
                   .WithOne(t => t.Sector) // 1 Ticket pertence a 1 Sector
                   .HasForeignKey(t => t.SectorId) // Chave estrangeira em Ticket
                   .IsRequired() // Garante que SectorId não pode ser nulo em Ticket
                   .OnDelete(DeleteBehavior.Restrict); // Impede deletar Sector se ele tiver Tickets associados
                                                       // (Geralmente não queremos perder tickets vendidos/gerados)

            // Adicionar índice para garantir que o nome do setor seja único dentro de um evento
            builder.HasIndex(s => new { s.EventId, s.Name })
                   .IsUnique();
        }
    }
}