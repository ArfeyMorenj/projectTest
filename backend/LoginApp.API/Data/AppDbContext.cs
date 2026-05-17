using LoginApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace LoginApp.API.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(entity =>
        {
            // Di Laravel ini mirip Schema::create('users', ...)
            entity.ToTable("users");

            // Username wajib unik, setara $table->unique('username')
            entity.HasIndex(u => u.Username).IsUnique();

            // Atur batasan kolom supaya konsisten dengan validasi input
            entity.Property(u => u.Username).HasMaxLength(50).IsRequired();
            entity.Property(u => u.PasswordHash).HasMaxLength(255).IsRequired();
            entity.Property(u => u.Email).HasMaxLength(100);

            // Default value di database, mirip default timestamp di migration Laravel
            entity.Property(u => u.CreatedAt)
                  .HasColumnType("timestamp")
                  .HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

        // Seed data awal, mirip DatabaseSeeder / Model::create() di Laravel.
        // Password di bawah sudah di-hash dengan BCrypt agar sesuai flow login asli.
        modelBuilder.Entity<User>().HasData(
            new User
            {
                Id = 1,
                Username = "admin",
                PasswordHash = "$2y$10$S0elRIMODvRzfEsQvBdhyuqQo30NpiUDo/TK6glOh.JNWfrYDBvsO",
                Email = "admin@gmail.com",
                CreatedAt = new DateTime(2026, 5, 16, 0, 0, 0, DateTimeKind.Utc)
            },
            new User
            {
                Id = 2,
                Username = "dzaki",
                PasswordHash = "$2y$10$1wMlnT0osstFTk9aCt0RvekXN4GEGECS8Xrlywc4.NsfMS5..hvuG",
                Email = "dzaki@gmail.com",
                CreatedAt = new DateTime(2026, 5, 16, 0, 0, 0, DateTimeKind.Utc)
            }
        );
    }
}
