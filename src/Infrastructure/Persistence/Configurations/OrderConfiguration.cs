using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(x => x.Id); // Define a chave primária

            builder.Property(x => x.OrderNumber)
                .IsRequired()
                .HasMaxLength(36);

            builder.Property(x => x.TotalAmount)
                .IsRequired()
                .HasPrecision(10, 2);


            builder.Property(x => x.Status)
                .IsRequired()
                .HasConversion<string>();

            builder.Property(x => x.OrderDate)
                .IsRequired();

            builder.Property(o => o.IsActive)
                   .IsRequired()
                   .HasDefaultValue(true);

            builder.HasOne(x => x.Customer)
                .WithMany(e => e.Orders)
                .HasForeignKey(e => e.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(x => x.OrderNumber).IsUnique(); // Garante que o número do pedido seja único

            builder.HasMany(o => o.Tickets)
                .WithOne(t => t.Order) // 1 Ticket pertence a 1 Order (ou nenhum, pois OrderId é nullable em Ticket)
                .HasForeignKey(t => t.OrderId)
                .OnDelete(DeleteBehavior.Restrict); // Se a Order for deletada, definir OrderId em Ticket como NULL
                                          // Alternativa: Restrict, se um Ticket sempre DEVE ter uma Order após criado.

            // Relacionamento com Payment (1 Order tem N Payments)
            builder.HasMany(o => o.Payments)
                   .WithOne(p => p.Order) // 1 Payment pertence a 1 Order
                   .HasForeignKey(p => p.OrderId)
                   .OnDelete(DeleteBehavior.Restrict); // Impedir deletar Order se tiver Payments associados (mais seguro)
                                                       // Alternativa: Cascade, se deletar Order deve deletar Payments (menos comum)
        }
    }
}
