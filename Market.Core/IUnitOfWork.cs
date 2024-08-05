using Market.Core.Entities;
using Market.Core.RepositoriesContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Core
{
    public interface IUnitOfWork : IAsyncDisposable 
    {
        IGenericRepository<T> Repository<T>() where T : BaseEntity;
        Task<int> CompleteAsync();
    }
}
