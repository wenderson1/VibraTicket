using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities;

namespace Infrastructure.Persistence.Configurations
{
    public class VenueConfiguration : IEntityTypeConfiguration<Venue>
    {
        public void Configure(EntityTypeBuilder<Venue> builder)
        {
            // Define a chave primária
            builder.HasKey(v => v.Id);

            // Configura a propriedade Name
            builder.Property(v => v.Name)
                .IsRequired()
                .HasMaxLength(150); // Ajuste o tamanho conforme necessário

            // Configura a propriedade Address
            builder.Property(v => v.Address)
                .IsRequired()
                .HasMaxLength(250);

            // Configura a propriedade City
            builder.Property(v => v.City)
                .IsRequired()
                .HasMaxLength(100);

            // Configura a propriedade State
            builder.Property(v => v.State)
                .IsRequired()
                .HasMaxLength(2); // Ou 2 se for usar sigla

            // Configura a propriedade ZipCode (opcional)
            builder.Property(v => v.ZipCode)
                .HasMaxLength(10); // Ajuste para o formato do CEP (ex: 00000-000)

            // Configura a propriedade Capacity
            builder.Property(v => v.Capacity)
                .IsRequired();

            // Configura as propriedades Latitude e Longitude (opcionais)
            // EF Core mapeia 'double' para 'float' no SQL Server por padrão, o que é adequado.
            builder.Property(v => v.Latitude);
            builder.Property(v => v.Longitude);

            // Configura o relacionamento com Event (1 Venue tem muitos Events)
            builder.HasMany(v => v.Events)
                   .WithOne(e => e.Venue) // 1 Event pertence a 1 Venue
                   .HasForeignKey(e => e.VenueId) // Chave estrangeira está em Event
                   .IsRequired() // Garante que VenueId não pode ser nulo em Event
                   .OnDelete(DeleteBehavior.Restrict); // Impede deletar Venue se tiver Events associados

            // Adicionar índices se necessário, por exemplo, no Nome ou Cidade/Estado
            builder.HasIndex(v => v.Name);
            builder.HasIndex(v => new { v.State, v.City }); // Índice composto para buscar locais por cidade/estado
        }
    }
}