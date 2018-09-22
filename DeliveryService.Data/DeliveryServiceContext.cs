using DeliveryService.Data.Model;
using Microsoft.EntityFrameworkCore;

namespace DeliveryService.Data
{
    public class DeliveryServiceContext : DbContext
    {
        public DeliveryServiceContext(DbContextOptions<DeliveryServiceContext> options) : base(options) { }

        public DbSet<Path> Paths { get; set; }

        public DbSet<Point> Points { get; set; }

        public DbSet<Route> Routes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Point>().HasKey(p => p.PointId);
            modelBuilder.Entity<Point>().HasAlternateKey(p => p.Name);

            modelBuilder.Entity<Path>().HasKey(p => p.PathId);
            modelBuilder.Entity<Path>().HasAlternateKey(p => new { p.DestinyId, p.OriginId });

            modelBuilder.Entity<Route>().HasKey(r => r.RouteId);
            modelBuilder.Entity<Route>().HasAlternateKey(r => r.PathId);
        }
    }
}
