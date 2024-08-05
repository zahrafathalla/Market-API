using Market.Core.Entities.Order_Aggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Core.Specification.Order_Specs
{
    public class OrderSpecificationWithPaymentIntent :BaseSpecification<Order>
    {
        public OrderSpecificationWithPaymentIntent(string paymentIntentId)
            :base(o => o.PaymentIntentId == paymentIntentId)
        {
            
        }
    }
}
