using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities;

namespace Infrastructure.Persistence.Configurations
{
    public class EventConfiguration : IEntityTypeConfiguration<Event>
    {
        public void Configure(EntityTypeBuilder<Event> builder)
        {
            builder.HasKey(e => e.Id); // Define a chave primária

            builder.Property(e => e.Name)
                .IsRequired() // Campo obrigatório
                .HasMaxLength(150); // Tamanho máximo

            builder.Property(e => e.Description)
                .HasMaxLength(2000); // Tamanho máximo

            builder.Property(e => e.Status)
                .IsRequired()
                .HasConversion<string>() // Armazena o Enum como string no banco
                .HasMaxLength(20);

            builder.Property(e => e.BannerImageUrl)
                .HasMaxLength(500);

            builder.Property(e => e.StartDate)
                   .IsRequired(); // Implícito para DateTime não anulável

            builder.Property(e => e.EndDate)
                   .IsRequired(); // Implícito para DateTime não anulável

            builder.Property(e => e.MinimumAge)
                   .IsRequired()
                   .HasDefaultValue(0); // Define o valor padrão no banco também

            builder.Property(e => e.CreatedAt)
                   .IsRequired(); // Valor padrão já definido na entidade

            builder.Property(e => e.UpdatedAt); // Já é nullable DateTime?
                                                // Se quiser que o banco atualize automaticamente:
                                                // .ValueGeneratedOnUpdate();

            // Exemplo para Venue:
            builder.HasOne(e => e.Venue)
                   .WithMany(v => v.Events)
                   .HasForeignKey(e => e.VenueId)
                   .IsRequired() // Adicionado para clareza
                   .OnDelete(DeleteBehavior.Restrict);

            // Exemplo para Affiliate:
            builder.HasOne(e => e.Affiliate)
                   .WithMany(a => a.Events)
                   .HasForeignKey(e => e.AffiliateId)
                   .IsRequired() // Adicionado para clareza
                   .OnDelete(DeleteBehavior.Restrict);

            // Relacionamento com Sector (1 Evento tem N Sectors)
            builder.HasMany(e => e.Sectors)
                .WithOne(s => s.Event) // 1 Sector pertence a 1 Evento
                .HasForeignKey(s => s.EventId)
                .OnDelete(DeleteBehavior.Cascade); // Deletar Sectors se o Evento for deletado

            // Relacionamento com Ticket (1 Evento tem N Tickets)
            builder.HasMany(e => e.Tickets)
               .WithOne(t => t.Event)
               .HasForeignKey(t => t.EventId)
               .OnDelete(DeleteBehavior.Restrict); // Não deletar Evento se tiver Tickets (geralmente)

            // Configurar índice se necessário, ex:
            // builder.HasIndex(e => e.StartDate);

            // Adicionar dentro do método Configure:
            builder.HasIndex(e => e.StartDate);

            // Outros índices úteis podem ser:
            builder.HasIndex(e => e.Status);
            builder.HasIndex(e => e.VenueId);
        }
    }
}