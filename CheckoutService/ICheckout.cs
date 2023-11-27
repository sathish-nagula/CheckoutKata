using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckoutService
{
    public interface ICheckout
    {
        void Scan(string sku);

        decimal GetTotalPrice();
    }
}
