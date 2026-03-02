using CRUD_Operation.Entities;
using Microsoft.EntityFrameworkCore;

namespace CRUD_Operation.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<SuperHero> SuperHeroes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<SuperHero>(entity =>
            {
                entity.ToTable("SuperHeroes");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).UseIdentityColumn();
            });
        }
    }
}
