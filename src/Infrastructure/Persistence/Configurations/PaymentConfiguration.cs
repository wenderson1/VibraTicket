using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            // Define a chave primária (já que é Guid, não precisa configurar geração)
            builder.HasKey(p => p.Id);

            // Configura a propriedade Amount
            builder.Property(p => p.Amount)
                .IsRequired() // Implícito para decimal não anulável
                .HasPrecision(10, 2); // Ajuste a precisão conforme necessário (ex: 10, 2 para até 99.999.999,99)

            // Configura a propriedade Method (Enum)
            builder.Property(p => p.Method)
                .IsRequired()
                .HasConversion<string>() // Armazena como string no banco
                .HasMaxLength(30);       // Tamanho suficiente para os nomes dos métodos

            // Configura a propriedade Status (Enum)
            builder.Property(p => p.Status)
                .IsRequired()
                .HasConversion<string>() // Armazena como string no banco
                .HasMaxLength(20);       // Tamanho suficiente para os nomes dos status

            // Configura a propriedade TransactionId (opcional)
            builder.Property(p => p.TransactionId)
                .HasMaxLength(100); // Ajuste o tamanho conforme o ID do gateway

            // Configura a propriedade GatewayResponse (opcional)
            // Pode precisar de um tamanho maior ou tipo diferente (ex: nvarchar(max)) se a resposta for longa
            builder.Property(p => p.GatewayResponse)
                .HasMaxLength(2000); // Ou .HasColumnType("nvarchar(max)");

            // Configura a propriedade PaymentDate
            // O valor padrão já é definido na entidade
            builder.Property(p => p.PaymentDate)
                .IsRequired();

            builder.Property(o => o.IsActive)
                   .IsRequired()
                   .HasDefaultValue(true);
            // Configura a propriedade ProcessedDate (opcional)
            builder.Property(p => p.ProcessedDate); // Já é nullable DateTime?

            // Configura o relacionamento com Order (1 Pagamento pertence a 1 Order)
            builder.HasOne(p => p.Order)
                   .WithMany(o => o.Payments) // 1 Order tem muitos Payments
                   .HasForeignKey(p => p.OrderId) // Chave estrangeira em Payment
                   .IsRequired() // Garante que OrderId não pode ser nulo
                   .OnDelete(DeleteBehavior.Restrict); // Impede deletar Order se tiver Payments associados (mais seguro)
                                                       // Evitar Cascade aqui para não perder histórico de pagamento

            // Adicionar índices se necessário, por exemplo, no TransactionId para buscas
            builder.HasIndex(p => p.TransactionId); // Não precisa ser único, talvez?
        }
    }
}