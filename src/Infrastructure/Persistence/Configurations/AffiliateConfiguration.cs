using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities;

namespace Infrastructure.Persistence.Configurations
{
    public class AffiliateConfiguration : IEntityTypeConfiguration<Affiliate>
    {
        public void Configure(EntityTypeBuilder<Affiliate> builder)
        {
            // Define a chave primária
            // Nota: O comentário na entidade sugere 'long', mas a propriedade está como 'int'.
            // Se mudar para long na entidade, mude aqui também.
            builder.HasKey(a => a.Id);

            // Configura a propriedade Name
            builder.Property(a => a.Name)
                .IsRequired()       // Campo obrigatório no banco
                .HasMaxLength(100); // Define um tamanho máximo

            // Configura a propriedade FullName
            builder.Property(a => a.FullName)
                .IsRequired()
                .HasMaxLength(200);

            // Configura a propriedade Document (CPF/CNPJ)
            builder.Property(a => a.Document)
                .IsRequired()
                .HasMaxLength(20); // Ajuste o tamanho conforme necessário (14 para CNPJ, 11 para CPF)

            // Configura a propriedade Email (opcional)
            builder.Property(a => a.Email)
                .HasMaxLength(150);

            // Configura a propriedade Phone (opcional)
            builder.Property(a => a.Phone)
                .HasMaxLength(20);

            // Configura as propriedades bancárias (opcionais)
            builder.Property(a => a.BankName)
                .HasMaxLength(100);

            builder.Property(a => a.BankAccount)
                .HasMaxLength(30);

            builder.Property(a => a.BankBranch)
                .HasMaxLength(20);

            // Configura a propriedade DefaultCommissionRate (decimal)
            builder.Property(a => a.DefaultCommissionRate)
                .HasPrecision(5, 2); // Ex: 5 dígitos no total, 2 após a vírgula (até 999.99)

            // Configura a propriedade CreatedAt
            // O valor padrão já é definido na entidade, mas podemos garantir que seja gerado pelo banco se necessário
            // builder.Property(a => a.CreatedAt)
            //    .ValueGeneratedOnAdd() // Informa ao EF que o valor é gerado ao adicionar
            //    .HasDefaultValueSql("GETUTCDATE()"); // Exemplo para SQL Server para usar a data do banco

            // Configuração do relacionamento com Event (lado "um" do "um-para-muitos")
            // A chave estrangeira (AffiliateId) e o OnDelete são configurados em EventConfiguration.
            // Aqui apenas definimos a coleção de navegação inversa.
            builder.HasMany(a => a.Events)          // Um Affiliate tem muitos Events
                   .WithOne(e => e.Affiliate)       // Um Event pertence a um Affiliate
                   .HasForeignKey(e => e.AffiliateId); // A chave estrangeira está em Event

            // Adicionar índices se necessário, por exemplo, no Documento para buscas rápidas
            builder.HasIndex(a => a.Document).IsUnique(); // Garante que o documento seja único
        }
    }
}