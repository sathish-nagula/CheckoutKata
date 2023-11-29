namespace CheckoutService
{
    public class Checkout : ICheckout
    {
        private readonly IItemRepository _itemRepository; // The item repository dependency
        private readonly IDiscountRuleRepository _discountRuleRepository; // The discount rule repository dependency
        private readonly Dictionary<string, int> _scannedItems; // The dictionary of items and their quantities scanned
        public Dictionary<string, int> ScannedItems
        {
            get { return _scannedItems; }
        }

        public Checkout(IItemRepository itemRepository, IDiscountRuleRepository discountRuleRepository)
        {
            _itemRepository = itemRepository;
            _discountRuleRepository = discountRuleRepository;
            _scannedItems = new Dictionary<string, int>();
        }

        public void Scan(string sku)
        {
            if (string.IsNullOrEmpty(sku))
            {
                throw new ArgumentException("SKU cannot be null or empty.", nameof(sku));
            }

            var item = _itemRepository.GetItemBySKU(sku);
            if (item == null)
            {
                throw new InvalidOperationException($"Item with SKU {sku} does not exist.");
            }

            if (_scannedItems.ContainsKey(sku))
            {
                _scannedItems[sku]++;
            }
            else
            {
                _scannedItems[sku] = 1;
            }
        }

        public decimal GetTotalPrice()
        {
            decimal totalPrice = 0;

            foreach (var scannedItem in _scannedItems)
            {
                var item = _itemRepository.GetItemBySKU(scannedItem.Key);
                var discountRule = _discountRuleRepository.GetDiscountRuleBySKU(scannedItem.Key);

                if (discountRule != null && scannedItem.Value >= discountRule.Quantity)
                {
                    int quotient = scannedItem.Value / discountRule.Quantity;
                    int remainder = scannedItem.Value % discountRule.Quantity;
                    totalPrice += quotient * (item.Price * discountRule.Quantity - discountRule.Discount) + remainder * item.Price;
                }
                else
                {
                    totalPrice += scannedItem.Value * item.Price;
                }
            }

            return totalPrice;
        }
    }

}
