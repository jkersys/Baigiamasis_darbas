using Microsoft.EntityFrameworkCore;
using UTP_Web_API.Database.InitialData;
using UTP_Web_API.Models;

namespace UTP_Web_API.Database
{
    public class UtpContext : DbContext
    {
        public UtpContext(DbContextOptions<UtpContext> options) : base(options)
        {
        }

        public DbSet<AdministrativeInspection> AdministrativeInspection { get; set; }
        public DbSet<Company> Company { get; set; }
        public DbSet<Complain> Complain { get; set; }
        public DbSet<Conclusion> Conclusion { get; set; }
        public DbSet<Investigation> Investigation { get; set; }
        public DbSet<InvestigationStage> InvestigationStage { get; set; }
        public DbSet<Investigator> Investigator { get; set; }
        public DbSet<LocalUser> LocalUser { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           modelBuilder.Entity<Conclusion>().HasData(ConclusionInitialData.DataSeed);
            //modelBuilder.Entity<LocalUser>().HasData(LocalUserInitialData.GetDataSeed());
            //modelBuilder.Entity<Complain>().HasOne(x => x.Investigator).WithOne


       //     modelBuilder
       //.Entity<Complain>()
       //.HasMany(e => e.Stages)
       //.WithOne(e => e.Complain)
       //.OnDelete(DeleteBehavior.ClientCascade);
        }
    }
}