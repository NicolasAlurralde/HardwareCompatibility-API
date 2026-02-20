using Microsoft.EntityFrameworkCore;
using PCBuilder.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Text;
namespace PCBuilder.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Processor> Processors { get; set; }
    public DbSet<Motherboard> Motherboards { get; set; }
    public DbSet<Ram> Rams { get; set; }
    public DbSet<PowerSupply> PowerSupplies { get; set; }
    public DbSet<VideoCard> VideoCards { get; set; }
    public DbSet<Storage> Storages { get; set; }
    public DbSet<PcCase> PcCases { get; set; }
    public DbSet<Cooler> Coolers { get; set; }

    public DbSet<PCBuild> PCBuilds { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Le decimos a EF Core que cree tablas separadas para cada componente 
        // en lugar de meter todo en una sola tabla gigante.
        modelBuilder.Entity<Processor>().ToTable("Processors");
        modelBuilder.Entity<Motherboard>().ToTable("Motherboards");
        modelBuilder.Entity<Ram>().ToTable("Rams");
        modelBuilder.Entity<PowerSupply>().ToTable("PowerSupplies");
        modelBuilder.Entity<VideoCard>().ToTable("VideoCards");
        modelBuilder.Entity<Storage>().ToTable("Storages");
        modelBuilder.Entity<PcCase>().ToTable("PcCases");
        modelBuilder.Entity<Cooler>().ToTable("Coolers");
    }
}
