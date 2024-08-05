using Market.Core.Entities.Order_Aggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Core.Specification.Order_Specs
{
    public class OrderSpecification :BaseSpecification<Order>
    {
        public OrderSpecification(string buyerEmail)
            :base(o => o.BuyerEmail == buyerEmail)
        {
            Include.Add(o => o.DeliveryMethod);
            Include.Add(o => o.Items);

            AddOrderByDec(o => o.OrderDate);
        }

        public OrderSpecification(string buyerEmail, int id)
            :base(o => o.BuyerEmail== buyerEmail && o.Id==id)
        {
            Include.Add(o => o.DeliveryMethod);
            Include.Add(o => o.Items);
        }
    }
}
