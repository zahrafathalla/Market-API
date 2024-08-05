using Market.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Market.Core.Specification
{
    public class BaseSpecification<T> : ISpecification<T> where T : BaseEntity
    {
        //_dbcontext.set<product>().where(p => p.id == id).include(p=>p.brandname).include(p=>p.category) 
        public Expression<Func<T, bool>> Criteria { get; set; } 
        public List<Expression<Func<T, object>>> Include { get ; set ; } = new List<Expression<Func<T, object>>>();
        public Expression<Func<T, object>> OrderBy { get; set; }
        public Expression<Func<T, object>> OrderByDec { get; set; }
        public int Skip { get; set; }
        public int Take { get; set; }
        public bool IsPaginated { get; set; }
        public BaseSpecification()
        {
            //Criteria = null;
        }
        public BaseSpecification(Expression<Func<T, bool>> criteriaExpressions)
        {
            Criteria = criteriaExpressions;
        }

        public void AddOrderBy(Expression<Func<T, object>> orderBy)
        {
            OrderBy = orderBy;
        }
        public void AddOrderByDec(Expression<Func<T, object>> orderByDec)
        {
            OrderByDec = orderByDec;
        }
        public void ApplyPagination (int skip , int take)
        {
            IsPaginated= true;
            Skip = skip;
            Take = take;
        }
    }
}








