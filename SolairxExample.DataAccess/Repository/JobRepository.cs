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
    //public class ProductRepository : Repository<Product>, IProductRepository
    public class JobRepository : Repository<Job>, IJobRepository
    {
        private readonly ApplicationDbContext _db;

        public JobRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Update(Job obj)
        {
            var objFromDb = _db.Jobs.FirstOrDefault(u => u.JobId == obj.JobId);
            if (objFromDb != null)
            {
                objFromDb.JobName = obj.JobName;
                objFromDb.JobDescription = obj.JobDescription;
                objFromDb.WebClientIntJobs = obj.WebClientIntJobs;
                objFromDb.DateModified = DateTime.Now; 
            }
        }
    }
}
