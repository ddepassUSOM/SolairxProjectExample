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
    public class PositionRepository : Repository<Position>, IPositionRepository
    {
        private readonly ApplicationDbContext _db;

        public PositionRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Update(Position obj)
        {
            var objFromDb = _db.Positions.FirstOrDefault(u => u.PositionId == obj.PositionId);
            if (objFromDb != null)
            {
                objFromDb.PositionName = obj.PositionName;
                objFromDb.Description  = obj.Description;
                objFromDb.CreateDate    = obj.CreateDate;
            }
        }
    }
}
