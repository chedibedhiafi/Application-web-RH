using ApplicationCore.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class ExamenContext: IdentityDbContext<IdentityUser>
    {
        public ExamenContext(DbContextOptions<ExamenContext> options)
        : base(options)
        {
        }
        public DbSet<ApplicationUser> Users { get; set; }
        public DbSet<Employees> Employees { get; set; }
        public DbSet<Section> Sections { get; set; }
        public DbSet<Conges> Conges { get; set; }
        public DbSet<Situation> Situations { get; set; }
     

        public DbSet<TypeConges> TypeConges { get; set; }
        public DbSet<ContreVisite> ContreVisites { get; set; }

        public DbSet<Attestation> Attestations { get; set; }

        public DbSet<TypeConfirmation> typeConfirmations { get; set; }

        public DbSet<TypeJustificatif> TypeJustificatifs { get; set; }

        public DbSet<Mission> Missions { get; set; }

        public DbSet<Absence> Absences { get; set; }


        public DbSet<Genre> Genres { get; set; }
        public DbSet<FonctionEmployee> FonctionEmployees { get; set; }
       


        public DbSet<GestionDocument> GestionDocuments { get; set; }
        public DbSet<SoldeConges> SoldeConges { get; set; }
        public DbSet<TypeAttestation> TypeAttestation { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
           // optionsBuilder.UseSqlServer(@"Data Source=(localdb)\mssqllocaldb;
            //  Initial Catalog=ApplicationDB;Integrated Security=true;
            //  MultipleActiveResultSets=true");
           base.OnConfiguring(optionsBuilder);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Conges>()
       .HasOne(c => c.Employees)
       .WithMany(e => e.Conges)
       .HasForeignKey(c => c.IdEmployees)
       .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Conges>()
                .HasOne(c => c.ConfirmeParEmployee)
                .WithMany()
                .HasForeignKey(c => c.ConfirmePar)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Conges>()
                .HasOne(c => c.RemplasseParEmployee)
                .WithMany()
                .HasForeignKey(c => c.RemplassePar)
                .OnDelete(DeleteBehavior.Restrict);


            base.OnModelCreating(modelBuilder);

        }
        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            base.ConfigureConventions(configurationBuilder);
            //optionsBuilder.UseSqlServer(@"Data Source=(localdb)\mssqllocaldb;
            //  Initial Catalog=ApplicationDB;Integrated Security=true;
            //  MultipleActiveResultSets=true");
        }
    }
}
