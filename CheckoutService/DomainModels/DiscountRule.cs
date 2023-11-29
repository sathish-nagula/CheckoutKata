namespace CheckoutService.DomainModels
{
    public class DiscountRule
    {
        public string SKU { get; set; }
        public int Quantity { get; set; }
        public decimal Discount { get; set; }
    }

}
