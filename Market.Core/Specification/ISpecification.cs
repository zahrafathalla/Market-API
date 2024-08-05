using Market.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Market.Core.Specification
{
    public interface ISpecification<T>  where T : BaseEntity
    {
       
        public Expression<Func<T,bool>> Criteria { get; set; } //_dbcontext.set<product>().where(p => p.id == id).include(p=>p.brandname).include(p=>p.category) 
        public List<Expression<Func<T,object>>> Include { get; set;}
        public Expression<Func<T,object>> OrderBy { get; set; }
        public Expression<Func<T, object>> OrderByDec { get; set; }

        public int Skip { get; set; }
        public int Take { get; set; }
        public bool IsPaginated { get; set; }



    }
}
