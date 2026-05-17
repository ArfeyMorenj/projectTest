using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace LoginApp.API.Data;

// Design-time factory ini dipakai oleh `dotnet ef` saat membuat migration.
// Tujuannya agar EF Core tidak perlu mencoba konek ke database dulu hanya
// untuk membaca versi server.
public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false)
            .AddJsonFile("appsettings.Development.json", optional: true)
            .Build();

        var connectionString = configuration.GetConnectionString("DefaultConnection");

        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionsBuilder.UseMySql(
            connectionString,
            ServerVersion.Parse("8.0.36-mysql")
        );

        return new AppDbContext(optionsBuilder.Options);
    }
}
