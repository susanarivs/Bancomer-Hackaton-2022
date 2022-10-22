using Microsoft.EntityFrameworkCore;

namespace OData.SmallVille.Models
{
    public class SourceStoreContext : DbContext
    {
        public SourceStoreContext(DbContextOptions<SourceStoreContext> options)
        : base(options)
        {
        }

        public DbSet<Prospecto> Prospectos { get; set; }
        public DbSet<Canal> Canales { get; set; }
        public DbSet<CanalProspecto> CanalesProspectos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CanalProspecto>()
                .HasKey(c => new { c.CanalId, c.ProspectoId });
        }
    }
}
