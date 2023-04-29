using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TZ2V.Repositories.IRepositories
{
    internal interface IRepository<T> : IRepositoryConfig 
        where T : class
    {
        IEnumerable<T> Get(int id);
        Task<int> Create(T entity);

    }
}
