using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SolairxExample.Model;
using System.Net;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using WebClient = SolairxExample.Model.WebClient;

namespace SolairxExample.DataAccess.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Job> Jobs { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<SpamTbl> SpamTbls { get; set; }
        public DbSet<WebClient> WebClients { get; set; }
        public DbSet<ProjectImage> ProjectImages { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<WebClientIntJob> WebClientIntJobs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<Employee>(entity =>
            {
                entity.Property(e => e.CreateDate).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Position)
                    .WithMany(p => p.Employees)
                    .HasForeignKey(d => d.PositionId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_Employee_Postion");
            });

            modelBuilder.Entity<Job>(entity =>
            {
                entity.Property(e => e.DateModified).HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<Position>(entity =>
            {
                entity.Property(e => e.CreateDate).HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<Project>(entity =>
            {
                entity.Property(e => e.CreateDate).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Employee)
                    .WithMany(p => p.Projects)
                    .HasForeignKey(d => d.ProjectManager)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_Project_Employee");
            });

            modelBuilder.Entity<SpamTbl>(entity =>
            {
                entity.Property(e => e.DateModified).HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<WebClient>(entity =>
            {
                entity.Property(e => e.DateModified).HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<WebClientIntJob>(entity =>
            {
                entity.HasKey(e => new { e.WcId, e.JobId });

                entity.HasOne(d => d.Job)
                    .WithMany(p => p.WebClientIntJobs)
                    .HasForeignKey(d => d.JobId)
                    .HasConstraintName("FK_Web_Client_INT_Jobs_Jobs");

                entity.HasOne(d => d.Wc)
                    .WithMany(p => p.WebClientIntJobs)
                    .HasForeignKey(d => d.WcId)
                    .HasConstraintName("FK_Web_Client_INT_Jobs_Web_Client");
            });
  
            modelBuilder.Entity<Job>().HasData(
              new Job
              {
                  JobId = 1,
                  JobName = "Residential",
                  JobDescription = "Private Home",
                  DateModified = DateTime.Now
              },
              new Job
              {
                  JobId = 2,
                  JobName = "Solar Energy",
                  JobDescription = "Solar Energy",
                  DateModified = DateTime.Now
              },
               new Job
               {
                   JobId = 3,
                   JobName = "Wind Energy",
                   JobDescription = "Wind Energy",
                   DateModified = DateTime.Now
               },
               new Job
               {
                   JobId = 4,
                   JobName = "Commercial",
                   JobDescription = "Place of Business",
                   DateModified = DateTime.Now
               },
               new Job
               {
                   JobId = 5,
                   JobName = "Installation",
                   JobDescription = "Installation",
                   DateModified = DateTime.Now
               },
               new Job
               {
                   JobId = 6,
                   JobName = "Maintenance",
                   JobDescription = "Maintenance",
                   DateModified = DateTime.Now
               },
               new Job
               {
                   JobId = 7,
                   JobName = "Repair",
                   JobDescription = "Repair",
                   DateModified = DateTime.Now
               },
               new Job
               {
                   JobId = 8,
                   JobName = "Request for quote",
                   JobDescription = "Request for quote",
                   DateModified = DateTime.Now
               },
               new Job
               {
                   JobId = 9,
                   JobName = "Financing",
                   JobDescription = "Financing",
                   DateModified = DateTime.Now
               },
               new Job
               {
                   JobId = 10,
                   JobName = "Inquiry",
                   JobDescription = "Inquiry",
                   DateModified = DateTime.Now
               });
            modelBuilder.Entity<Position>().HasData(
             new Position
             {
                 PositionId = 1,
                 PositionName = "Project Manager",
                 Description = "Manager of particular project",
                 CreateDate = DateTime.Now
             },
             new Position
             {
                 PositionId = 2,
                 PositionName = "Supervisor",
                 Description = "Administration Supervisor",
                 CreateDate = DateTime.Now
             },
             new Position
             {
                 PositionId = 3,
                 PositionName = "Manager",
                 Description = "Administration Manager",
                 CreateDate = DateTime.Now
             },
             new Position
             {
                 PositionId = 4,
                 PositionName = "Accountant",
                 Description = "Administration Accountant",
                 CreateDate = DateTime.Now
             },
             new Position
             {
                 PositionId = 5,
                 PositionName = "Technician",
                 Description = "Operation",
                 CreateDate = DateTime.Now
             });
            modelBuilder.Entity<Employee>().HasData(
            new Employee
            {
                EmplyeeId = 1,
                FirstName = "Dwight",
                LastName = "DePass",
                Phone = "3059895416",
                Email = "ddepass@gmail.com",
                PositionId = 1,
                CreateDate = DateTime.Now,
                ModifiedDate = null
            },
            new Employee
            {
                EmplyeeId = 2,
                FirstName = "Andrew",
                LastName = "Peart",
                Phone = null,
                Email = null,
                PositionId = 1,
                CreateDate = DateTime.Now,
                ModifiedDate = null
            });
            modelBuilder.Entity<Project>().HasData(
            new Project
            {
                ProjectId = 1,
                ProjectName = "Andrew First Project",
                ProjectShortDesc = "<p>Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.&nbsp;</p><p><br></p>",
                ProjectLongDesc = "<p>Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.</p><p><br></p><p><br></p>",
                ProjectManager = 2,
                CreateDate = DateTime.Now,
                ModifiedDate = null
            },
            new Project
            {

                ProjectId = 2,
                ProjectName = "Andrew Second Project",
                ProjectShortDesc = "<p>Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.&nbsp;<br></p>",
                ProjectLongDesc = "<p>Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.<br></p>",
                ProjectManager = 2,

                CreateDate = DateTime.Now,
                ModifiedDate = null
            });
            modelBuilder.Entity<WebClient>().HasData(
            new WebClient
            {
                WcId = 1,
                FirstName = "Dwight",
                LastName = "DePass",
                Email = "ddepass@gmail.com",
                Phone = "1-305-989-5416",
                Residential = true,
                Commercial = false,
                Message = "<p>Test</p>",
                DateModified = DateTime.Now
            },
            new WebClient
            {

                WcId = 2,
                FirstName = "Regine",
                LastName = "DePass",
                Email = "dwight@dreadscout.com",
                Phone = "1-305-989-5416",
                Residential = true,
                Commercial = false,
                Message = "<p>Test</p>",
                DateModified = DateTime.Now
            });
         
        }

       
    }
}
