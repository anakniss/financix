using backend.Entities;
using Microsoft.EntityFrameworkCore;

namespace backend.Data;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
        
    }
    
    public DbSet<User> Users { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        

        base.OnModelCreating(modelBuilder);
    }
    
    // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    // {
    //     optionsBuilder.ConfigureWarnings(warnings =>
    //         warnings.Ignore(RelationalEventId.PendingModelChangesWarning));
    // }
}