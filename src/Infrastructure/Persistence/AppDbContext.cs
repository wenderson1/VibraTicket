using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Infrastructure.Persistence
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // DbSet para cada entidade que será mapeada para uma tabela
        public DbSet<Affiliate> Affiliates { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Venue> Venues { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Sector> Sectors { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Payment> Payments { get; set; }
        // Adicione DbSets para outras entidades como EventCategory, Discount, Review quando criá-las
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // Chama a implementação base no final

            // Aplica todas as configurações de entidade definidas em classes separadas
            // que implementam IEntityTypeConfiguration<TEntity> neste assembly.
            // Isso mantém este método limpo.
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            // Você pode adicionar configurações específicas ou seeding aqui se preferir,
            // mas a abordagem com classes de configuração separadas é mais organizada.
            // Exemplo: SeedData(modelBuilder);

            modelBuilder.Entity<Customer>().HasQueryFilter(c => c.IsActive);
            modelBuilder.Entity<Order>().HasQueryFilter(o => o.IsActive);
            modelBuilder.Entity<Payment>().HasQueryFilter(p => p.IsActive);

        }
    }
}