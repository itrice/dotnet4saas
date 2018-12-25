using iTrice.SAAS.TenantManager.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace iTrice.SAAS.TenantManager.Data
{
    public class TenantContextFactory : IDesignTimeDbContextFactory<TenantContext>
    {
        public TenantContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<TenantContext>();
            optionsBuilder.UseSqlServer(TenantContext.ConnectionString);
            ///按时
            return new TenantContext(optionsBuilder.Options);
        }
    }

    public class TenantContext : DbContext//, IDesignTimeDbContextFactory<TenantContext>
    {
        public static string ConnectionString { get; set; }

        public TenantContext(DbContextOptions<TenantContext> options) : base(options)
        {
        }

        public DbSet<Tenant> Tenants { get; set; }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(ConnectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Tenant>().ToTable("Tenant");
            modelBuilder.Entity<SystemLog>().ToTable("SystemLog");
        }
    }
}