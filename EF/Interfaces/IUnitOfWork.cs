using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF.Interfaces
{
    interface IUnitOfWork
    {
        Task BeginTransaction(); 
        Task Commit(); 
        Task Rollback();
    }
}
