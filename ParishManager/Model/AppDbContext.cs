using Microsoft.EntityFrameworkCore;

namespace ParishManager.Model;

public class AppDbContext:DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options){}

    public DbSet<Parish> Parishes { get; set; }


}