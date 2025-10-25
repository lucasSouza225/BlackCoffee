using BlackCoffee.API.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace BlackCoffee.API.Data
{
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionsBuilder.UseMySql(
                "Server=localhost;Database=BlackCoffeeDB;User=root;Password=Ls@246813;",
                ServerVersion.AutoDetect("Server=localhost;Database=BlackCoffeeDB;User=root;Password=Ls@246813;")
            );

            return new AppDbContext(optionsBuilder.Options);
        }
    }
}
