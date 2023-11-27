using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckoutService
{
    public class Checkout : ICheckout
    {
        public decimal GetTotalPrice()
        {
            throw new NotImplementedException();
        }

        public void Scan(string sku)
        {
            throw new NotImplementedException();
        }
    }
}
