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
    public class ProjectRepository : Repository<Project>, IProjectRepository
    {
        private readonly ApplicationDbContext _db;

        public ProjectRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Update(Project obj)
        {
            var objFromDb = _db.Projects.FirstOrDefault(u => u.ProjectId == obj.ProjectId);
            if (objFromDb != null)
            {
                objFromDb.ProjectName = obj.ProjectName;
                objFromDb.ProjectShortDesc = obj.ProjectShortDesc;
                objFromDb.ProjectLongDesc = obj.ProjectLongDesc;               
                objFromDb.ProjectManager = obj.ProjectManager;
                objFromDb.ModifiedDate = obj.ModifiedDate;
                objFromDb.ProjectImages = obj.ProjectImages;                
            }
        }
    }
}
