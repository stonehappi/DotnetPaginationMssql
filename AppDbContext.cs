using DotnetPaginationMssql.Models;
using Microsoft.EntityFrameworkCore;
namespace DotnetPaginationMssql;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) :
        base(options)
    {
    }

    public DbSet<User> Users { get; set; } = null!;
}

