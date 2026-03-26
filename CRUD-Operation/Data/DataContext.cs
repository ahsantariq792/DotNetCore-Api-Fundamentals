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
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<SuperHero>(entity =>
            {
                entity.ToTable("SuperHeroes");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).UseIdentityColumn();
            });
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("Users");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).UseIdentityColumn();
                entity.HasIndex(e => e.Email).IsUnique();
                entity.Property(e => e.Email).HasMaxLength(256);
                entity.Property(e => e.PasswordHash).HasMaxLength(500);
                entity.Property(e => e.FullName).HasMaxLength(200);
            });
        }
    }
}
