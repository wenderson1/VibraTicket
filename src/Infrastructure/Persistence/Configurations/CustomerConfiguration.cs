using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(a => a.FullName)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(a => a.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(a => a.Email)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(a => a.Document)
                .IsRequired()
                .HasMaxLength(15);

            builder.HasIndex(a => a.Document)
                .IsUnique();

            builder.Property(a => a.BirthDate)
                .IsRequired();

            builder.Property(a => a.Address)
                .HasMaxLength(250);

            builder.Property(a => a.City)
                .HasMaxLength(250);

            builder.Property(a => a.State)
                .HasMaxLength(250);

            builder.Property(a => a.ZipCode)
                .HasMaxLength(250);

            builder.Property(a => a.CreatedAt)
                .IsRequired();

            builder.Property(e => e.IsActive)
                .IsRequired()
                .HasDefaultValue(true);

            builder.HasMany(x => x.Orders)
                   .WithOne(x => x.Customer)
                   .HasForeignKey(x => x.CustomerId)
                   .OnDelete(DeleteBehavior.Restrict); // Impede deletar Customer se ele tiver Orders        

            // Opcional, mas recomendado para performance: Índice Filtrado
            // (Exemplo para Customer.Email único apenas para ativos)
            builder.HasIndex(e => e.Email)
                   .IsUnique()
                   .HasFilter("[IsActive] = 1"); // Sintaxe SQL Server

            builder.HasIndex(c => c.Document)
                   .IsUnique()
                   .HasFilter("[IsActive] = 1");
        }
    }
}
