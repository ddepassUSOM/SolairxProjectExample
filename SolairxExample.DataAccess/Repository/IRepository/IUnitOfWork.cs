using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolairxExample.DataAccess.Repository.IRepository
{
    public interface IUnitOfWork
    {
        IJobRepository Job { get; }

        IEmployeeRepository Employee { get; }

        IPositionRepository Position { get; }
        IProjectRepository Project { get; }

        ICompanyRepository Company { get; }

        IApplicationUserRepository ApplicationUser { get; }

        ISpamTblRepository SpamTbl { get; }

        IWebClientRepository WebClient { get; }

        IProjectImageRepository ProjectImage { get; }
        void Save();
    }
}
