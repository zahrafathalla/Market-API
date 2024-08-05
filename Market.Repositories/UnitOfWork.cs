using Market.Core;
using Market.Core.Entities;
using Market.Core.RepositoriesContracts;
using Market.Repository.Data;
using Market.Repository.GenericRepository;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private Hashtable _reposetories;
        private readonly MarketDBContext _dBContext;

        public UnitOfWork(MarketDBContext dBContext)
        {
            _reposetories = new Hashtable();
            _dBContext = dBContext;
        }
        public IGenericRepository<T> Repository<T>() where T : BaseEntity
        {
            var key = typeof(T).Name;


            if(! _reposetories.ContainsKey(key) )
            {
                var repository = new GenericRepository<T>(_dBContext);

                _reposetories.Add(key, repository);
            }
            return _reposetories[key] as IGenericRepository<T> ;
        }

        public async Task<int> CompleteAsync()
            => await _dBContext.SaveChangesAsync();

        public ValueTask DisposeAsync()
            => _dBContext.DisposeAsync();
    }
}
