using Microsoft.EntityFrameworkCore;
using PetSearch2.Models;
using Microsoft.EntityFrameworkCore.SqlServer;

namespace PetSearch2.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }


        public DbSet<Animals> Animals { get; set; }
        public DbSet<Clinics> Clinics { get; set; }
        public DbSet<Locations> Locations { get; set; }
        public DbSet<Vets> Vets { get; set; }
        public DbSet<Persons> Persons { get; set; }

        public DbSet<Specialties> Specialties { get; set; }

        public DbSet <Tasks> Tasks { get; set; }

    }
}
