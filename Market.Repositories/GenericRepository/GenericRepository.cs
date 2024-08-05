using Market.Core.Entities;
using Market.Core.RepositoriesContracts;
using Market.Core.Specification;
using Market.Repository.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Repository.GenericRepository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly MarketDBContext _dBContext;

        public GenericRepository(MarketDBContext dBContext)
        {
            _dBContext = dBContext;
        }


        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            //if(typeof(T)== typeof(Product))
            //    return (IEnumerable<T>) await _dBContext.Set<Product>().Include(p=> p.Brand).Include(p=> p.Category).ToListAsync();

            return await _dBContext.Set<T>().ToListAsync();
        }

        public async Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).ToListAsync();
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            //if (typeof(T) == typeof(Product))
            //    return await _dBContext.Set<Product>().Where(p=> p.Id == id).Include(p => p.Brand).Include(p => p.Category).FirstOrDefaultAsync() as T;

            return await _dBContext.Set<T>().FindAsync(id);
        }

        public async Task<T?> GetByIdWithSpecAsync(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).FirstOrDefaultAsync();
        }

        public async Task<int> GetCountAsync(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).CountAsync();
        }

        public async Task AddAsync(T entity)
            => await _dBContext.AddAsync(entity);
   
        public void Update(T entity)
            =>_dBContext.Update(entity);
       
        public void Delete(T entity)
            =>_dBContext.Remove(entity);

        private IQueryable<T> ApplySpecification(ISpecification<T> spec)
        {
            return SpecificationEvaluator<T>.GetQuery(_dBContext.Set<T>(), spec);
        }
    }
}
