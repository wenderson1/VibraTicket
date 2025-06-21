using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Persistence
{
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {

            var basePath = Directory.GetCurrentDirectory();
            var apiProjectPath = Path.GetFullPath(Path.Combine(basePath, "../Api"));

            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(apiProjectPath)
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development"}.json", optional: true)
                .Build();

            var builder = new DbContextOptionsBuilder<AppDbContext>();
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            builder.UseSqlServer(connectionString);

            return new AppDbContext(builder.Options);
        }
    }
}