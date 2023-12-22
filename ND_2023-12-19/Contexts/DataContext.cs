using Microsoft.EntityFrameworkCore;
using ND_2023_12_19.Entities;

namespace ND_2023_12_19.Contexts;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options)
    : base(options)
    {
    }

    public DbSet<ItemEntity> Items { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<ItemEntity>().ToTable("items");
    }
}
