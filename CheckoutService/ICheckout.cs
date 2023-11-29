namespace CheckoutService
{
    public interface ICheckout
    {
        void Scan(string sku);

        decimal GetTotalPrice();
    }
}
