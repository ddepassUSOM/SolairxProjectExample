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
    public class WebClientRepository : Repository<WebClient>, IWebClientRepository
    {
        private readonly ApplicationDbContext _db;

        public WebClientRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Update(WebClient obj)
        {
            var objFromDb = _db.WebClients.FirstOrDefault(u => u.WcId == obj.WcId);
            if (objFromDb != null)
            {
                objFromDb.FirstName = obj.FirstName;
                objFromDb.LastName = obj.LastName;
                objFromDb.Phone = obj.Phone;
                objFromDb.Email = obj.Email;
                objFromDb.Message = obj.Message;
                objFromDb.Residential = obj.Residential;
                objFromDb.Commercial = obj.Commercial;
                objFromDb.WebClientIntJobs = obj.WebClientIntJobs; 
                objFromDb.DateModified = DateTime.Now;
            }
        }
    }
}
