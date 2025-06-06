using SolairxExample.DataAccess.Data;
using SolairxExample.DataAccess.Repository.IRepository;
using SolairxExample.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolairxExample.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _db;

        public IJobRepository Job { get; private set; }
        public IEmployeeRepository Employee { get; private set; }
        public IPositionRepository Position { get; private set; }
        public IProjectRepository Project { get; private set; }
        public ICompanyRepository Company { get; private set; }
        public IApplicationUserRepository ApplicationUser { get; private set; }
        public ISpamTblRepository SpamTbl { get; private set; }
        public IWebClientRepository WebClient { get; private set; }

        public IProjectImageRepository ProjectImage { get; private set; }

        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            Job = new JobRepository(_db);
            Employee = new EmployeeRepository(_db);
            Position = new PositionRepository(_db);
            Project = new ProjectRepository(_db);
            Company = new CompanyRepository(_db);
            ApplicationUser = new ApplicationUserRepository(_db);
            SpamTbl = new SpamTblRepository(_db);
            WebClient = new WebClientRepository(_db);  
            ProjectImage = new ProjectImageRepository(_db);
        }
        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
