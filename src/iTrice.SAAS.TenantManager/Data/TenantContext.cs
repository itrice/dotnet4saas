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
            optionsBuilder.UseSqlServer(@"Data Source=.\SQLEXPRESS01;Integrated Security=True;Initial Catalog=Tenant");

            return new TenantContext(optionsBuilder.Options);
        }
    }

    public class TenantContext : DbContext//, IDesignTimeDbContextFactory<TenantContext>
    {
        public TenantContext(DbContextOptions<TenantContext> options) : base(options)
        {
        }

        public DbSet<Tenant> Tenants { get; set; }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=.\SQLEXPRESS01;Integrated Security=True;Initial Catalog=Tenant");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Tenant>().ToTable("Tenant");
            modelBuilder.Entity<SystemLog>().ToTable("SystemLog");
        }
    }
}