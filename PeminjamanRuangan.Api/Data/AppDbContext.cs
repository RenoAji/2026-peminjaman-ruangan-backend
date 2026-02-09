using Microsoft.EntityFrameworkCore;
using PeminjamanRuangan.Api.Models;

namespace PeminjamanRuangan.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Ruangan> Ruangan => Set<Ruangan>();
    public DbSet<Peminjaman> Peminjaman => Set<Peminjaman>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Ruangan configuration
        modelBuilder.Entity<Ruangan>(entity =>
        {
            entity.HasIndex(r => r.NamaRuangan).IsUnique();
        });

        // Peminjaman configuration
        modelBuilder.Entity<Peminjaman>(entity =>
        {
            entity.HasOne(p => p.Ruangan)
                  .WithMany(r => r.Peminjaman)
                  .HasForeignKey(p => p.RuanganId)
                  .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
