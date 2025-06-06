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
    public class EmployeeRepository : Repository<Employee>, IEmployeeRepository
    {
        private readonly ApplicationDbContext _db;

        public EmployeeRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Update(Employee obj)
        {
            var objFromDb = _db.Employees.FirstOrDefault(u => u.EmplyeeId == obj.EmplyeeId);
            if (objFromDb != null)
            {
                objFromDb.FirstName = obj.FirstName;
                objFromDb.LastName = obj.LastName;
                objFromDb.Phone = obj.Phone;
                objFromDb.Email = obj.Email;
                objFromDb.CreateDate = obj.CreateDate;
                objFromDb.Projects = obj.Projects;
                objFromDb.PositionId  = obj.PositionId;
                objFromDb.ModifiedDate = DateTime.Now;               
            }
        }
    }
}
