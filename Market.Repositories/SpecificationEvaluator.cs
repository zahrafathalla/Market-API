using Market.Core.Entities;
using Market.Core.Specification;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Repository
{
    internal static class SpecificationEvaluator<T> where T : BaseEntity
    {
        public static IQueryable<T> GetQuery(IQueryable<T> inputQuery, ISpecification<T> spec)
        {
            //_dbcontext.set<product>().where(p => p.id == id).include(p=>p.brandname).include(p=>p.category) 

            var query = inputQuery; //_dbcontext.set<product>()


            if (spec.Criteria is not null)
                query = query.Where(spec.Criteria); //_dbcontext.set<product>().where(p => p.BrandId == 1 && true) or .where(p => p.CategoryId == 1 && true) or .where(p => p.BrandId == 1 && p.CategoryId == 1)

            if (spec.OrderBy is not null)
                query = query.OrderBy(spec.OrderBy);

            else if (spec.OrderByDec is not null)
                query = query.OrderByDescending(spec.OrderByDec);

            if (spec.IsPaginated)
                query = query.Skip(spec.Skip).Take(spec.Take);

            query = spec.Include.Aggregate(query, (currentQuery, includeExpression) => currentQuery.Include(includeExpression));

            return query;
        }
    }
}
