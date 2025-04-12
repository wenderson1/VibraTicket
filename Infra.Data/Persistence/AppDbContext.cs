using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infra.Data.Persistence
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<Affiliate> Affiliates { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<EventCategory> EventCategories { get; set; }
        public DbSet<Venue> Venues { get; set; }
        public DbSet<Sector> Sectors { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<Discount> Discounts { get; set; }
        public DbSet<Review> Reviews { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Affiliate configuration
            modelBuilder.Entity<Affiliate>(entity =>
            {
                entity.HasKey(a => a.Id);
                entity.Property(a => a.FullName).IsRequired().HasMaxLength(100);
                entity.Property(a => a.Name).IsRequired().HasMaxLength(50);
                entity.Property(a => a.Document).IsRequired().HasMaxLength(20);
                entity.Property(a => a.Email).HasMaxLength(100);
                entity.Property(a => a.Phone).HasMaxLength(20);
                entity.Property(a => a.DefaultCommissionRate).HasPrecision(5, 2);
            });

            // Customer configuration
            modelBuilder.Entity<Customer>(entity =>
            {
                entity.HasKey(c => c.Id);
                entity.Property(c => c.FullName).IsRequired().HasMaxLength(100);
                entity.Property(c => c.Gender).IsRequired().HasMaxLength(20);
                entity.Property(c => c.Document).IsRequired().HasMaxLength(20);
                entity.Property(c => c.Email).IsRequired().HasMaxLength(100);
                entity.Property(c => c.Phone).HasMaxLength(20);
                entity.Property(c => c.Address).HasMaxLength(200);
                entity.Property(c => c.City).HasMaxLength(100);
                entity.Property(c => c.State).HasMaxLength(50);
                entity.Property(c => c.ZipCode).HasMaxLength(20);
            });

            // EventCategory configuration
            modelBuilder.Entity<EventCategory>(entity =>
            {
                entity.HasKey(ec => ec.Id);
                entity.Property(ec => ec.Name).IsRequired().HasMaxLength(50);
                entity.Property(ec => ec.Description).HasMaxLength(200);
            });

            // Venue configuration
            modelBuilder.Entity<Venue>(entity =>
            {
                entity.HasKey(v => v.Id);
                entity.Property(v => v.Name).IsRequired().HasMaxLength(100);
                entity.Property(v => v.Address).IsRequired().HasMaxLength(200);
                entity.Property(v => v.City).HasMaxLength(100);
                entity.Property(v => v.State).HasMaxLength(50);
                entity.Property(v => v.ZipCode).HasMaxLength(20);
            });

            // Event configuration
            modelBuilder.Entity<Event>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Description).HasMaxLength(2000);
                entity.Property(e => e.Status).IsRequired().HasMaxLength(20);
                entity.Property(e => e.BannerImageUrl).HasMaxLength(500);
                entity.Property(e => e.CancellationPolicy).HasMaxLength(1000);

                entity.HasOne(e => e.Venue)
                    .WithMany(v => v.Events)
                    .HasForeignKey(e => e.VenueId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Affiliate)
                    .WithMany(a => a.Events)
                    .HasForeignKey(e => e.AffiliateId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Category)
                    .WithMany(c => c.Events)
                    .HasForeignKey(e => e.EventCategoryId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(e => e.InterestedCustomers)
                    .WithMany(c => c.FavoriteEvents)
                    .UsingEntity<Dictionary<string, object>>("CustomerFavorites",
                        j => j.HasOne<Customer>().WithMany().HasForeignKey("CustomerId").OnDelete(DeleteBehavior.Cascade),
                        j => j.HasOne<Event>().WithMany().HasForeignKey("EventId").OnDelete(DeleteBehavior.Cascade));
            });

            // Sector configuration
            modelBuilder.Entity<Sector>(entity =>
            {
                entity.HasKey(s => s.Id);
                entity.Property(s => s.Name).IsRequired().HasMaxLength(50);
                entity.Property(s => s.Description).HasMaxLength(200);
                entity.Property(s => s.Price).HasPrecision(10, 2);
                entity.Property(s => s.Tax).HasPrecision(10, 2);
                entity.Property(s => s.Commission).HasPrecision(10, 2);

                entity.HasOne(s => s.Event)
                    .WithMany(e => e.Sectors)
                    .HasForeignKey(s => s.EventId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Discount configuration
            modelBuilder.Entity<Discount>(entity =>
            {
                entity.HasKey(d => d.Id);
                entity.Property(d => d.Code).IsRequired().HasMaxLength(20);
                entity.Property(d => d.Description).HasMaxLength(200);
                entity.Property(d => d.Amount).HasPrecision(10, 2);

                entity.HasOne(d => d.Event)
                    .WithMany()
                    .HasForeignKey(d => d.EventId)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            // Order configuration
            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(o => o.Id);
                entity.Property(o => o.OrderNumber).IsRequired().HasMaxLength(20);
                entity.Property(o => o.Status).IsRequired().HasMaxLength(20);
                entity.Property(o => o.Subtotal).HasPrecision(10, 2);
                entity.Property(o => o.TaxAmount).HasPrecision(10, 2);
                entity.Property(o => o.DiscountAmount).HasPrecision(10, 2);
                entity.Property(o => o.TotalAmount).HasPrecision(10, 2);

                entity.HasOne(o => o.Customer)
                    .WithMany(c => c.Orders)
                    .HasForeignKey(o => o.CustomerId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(o => o.Discount)
                    .WithMany(d => d.Orders)
                    .HasForeignKey(o => o.DiscountId)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            // Payment configuration
            modelBuilder.Entity<Payment>(entity =>
            {
                entity.HasKey(p => p.Id);
                entity.Property(p => p.Amount).HasPrecision(10, 2);
                entity.Property(p => p.Method).IsRequired().HasMaxLength(50);
                entity.Property(p => p.Status).IsRequired().HasMaxLength(50);
                entity.Property(p => p.TransactionId).IsRequired().HasMaxLength(100);
                entity.Property(p => p.GatewayResponse).HasMaxLength(1000);
                entity.Property(p => p.RefundAmount).HasPrecision(10, 2);
                entity.Property(p => p.RefundReason).HasMaxLength(200);

                entity.HasOne(p => p.Order)
                    .WithMany(o => o.Payments)
                    .HasForeignKey(p => p.OrderId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(p => p.Customer)
                    .WithMany()
                    .HasForeignKey(p => p.CustomerId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Ticket configuration
            modelBuilder.Entity<Ticket>(entity =>
            {
                entity.HasKey(t => t.Id);
                entity.Property(t => t.TicketNumber).IsRequired().HasMaxLength(20);
                entity.Property(t => t.Price).HasPrecision(10, 2);
                entity.Property(t => t.Tax).HasPrecision(10, 2);
                entity.Property(t => t.Commission).HasPrecision(10, 2);
                entity.Property(t => t.Status).IsRequired().HasMaxLength(20);
                entity.Property(t => t.QrCode).IsRequired().HasMaxLength(200);
                entity.Property(t => t.Type).IsRequired().HasMaxLength(50);

                entity.HasOne(t => t.Event)
                    .WithMany(e => e.Tickets)
                    .HasForeignKey(t => t.EventId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(t => t.Sector)
                    .WithMany(s => s.Tickets)
                    .HasForeignKey(t => t.SectorId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(t => t.Customer)
                    .WithMany(c => c.Tickets)
                    .HasForeignKey(t => t.CustomerId)
                    .OnDelete(DeleteBehavior.SetNull);

                entity.HasOne(t => t.Order)
                    .WithMany(o => o.Tickets)
                    .HasForeignKey(t => t.OrderId)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            // Review configuration
            modelBuilder.Entity<Review>(entity =>
            {
                entity.HasKey(r => r.Id);
                entity.Property(r => r.Comment).HasMaxLength(1000);

                entity.HasOne(r => r.Event)
                    .WithMany()
                    .HasForeignKey(r => r.EventId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(r => r.Customer)
                    .WithMany()
                    .HasForeignKey(r => r.CustomerId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
            SeedData(modelBuilder);
        }
        private void SeedData(ModelBuilder modelBuilder)
        {
            // Categorias de eventos
            modelBuilder.Entity<EventCategory>().HasData(
                new EventCategory { Id = 1, Name = "Show", Description = "Eventos musicais e apresentações ao vivo" },
                new EventCategory { Id = 2, Name = "Teatro", Description = "Peças teatrais e performances" },
                new EventCategory { Id = 3, Name = "Esporte", Description = "Eventos esportivos e competições" },
                new EventCategory { Id = 4, Name = "Conferência", Description = "Palestras, workshops e eventos corporativos" }
            );// Afiliado inicial
            modelBuilder.Entity<Affiliate>().HasData(
                new Affiliate
                {
                    Id = 1,
                    FullName = "Administrador do Sistema",
                    Name = "Admin",
                    Document = "00000000000",
                    Email = "admin@vibraticket.com",
                    Phone = "11999999999",
                    DefaultCommissionRate = 5.0m
                }
            );

            // Local inicial
            modelBuilder.Entity<Venue>().HasData(
                new Venue
                {
                    Id = 1,
                    Name = "Centro de Convenções",
                    Address = "Av. Principal, 1000",
                    City = "São Paulo",
                    State = "SP",
                    ZipCode = "01000-000",
                    Capacity = 5000
                }
            );
        }
    }
}