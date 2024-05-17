using Microsoft.EntityFrameworkCore;
using DetailingCenterApp.Models;


namespace DetailingCenterApp.Models
{
    public class DetailingCenterDBContext : DbContext
    {
        public DetailingCenterDBContext(DbContextOptions<DetailingCenterDBContext> options) : base(options)
        {
        }        
        public DbSet<Automobile> Automobiles { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Employer> Employers { get; set; }
        public DbSet<ProvidedService> ProvidedServices { get; set; }
        public DbSet<Service> Services { get; set; }
    }
}
