using SolairxExample.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolairxExample.DataAccess.Repository.IRepository
{
    public interface ISpamTblRepository : IRepository<SpamTbl>
    {
        void Update(SpamTbl obj);
    }
}
