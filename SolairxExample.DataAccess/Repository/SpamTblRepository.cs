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
    public class SpamTblRepository : Repository<SpamTbl>, ISpamTblRepository
    {
        private readonly ApplicationDbContext _db;

        public SpamTblRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Update(SpamTbl obj)
        {
            var objFromDb = _db.SpamTbls.FirstOrDefault(u => u.SpamId == obj.SpamId);
            if (objFromDb != null)
            {
                objFromDb.FirstName = obj.FirstName;
                objFromDb.LastName  = obj.LastName;
                objFromDb.Phone = obj.Phone;
                objFromDb.Email = obj.Email;
                objFromDb.Residential = obj.Residential;
                objFromDb.Commercial = obj.Commercial;
                objFromDb.Message = obj.Message;
                objFromDb.DateModified = DateTime.Now;
            }
        }
    }
}
