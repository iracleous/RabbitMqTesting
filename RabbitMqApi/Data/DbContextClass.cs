using Microsoft.EntityFrameworkCore;
using RabbitMqApi.Models;

namespace RabbitMqApi.Data;

public class DbContextClass : DbContext
{
    protected readonly IConfiguration Configuration;
    public DbSet<Product> Products   { get; set;  }

    public DbContextClass(IConfiguration configuration)
    {
        Configuration = configuration;
    }
    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
    }
}