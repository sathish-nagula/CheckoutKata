using CheckoutService.DomainModels;

namespace CheckoutService
{
    public class DiscountRuleRepository : IDiscountRuleRepository
    {
        private readonly Dictionary<string, DiscountRule> discountRules;

        public DiscountRuleRepository()
        {
            discountRules = new Dictionary<string, DiscountRule>()
        {
            { "A", new DiscountRule { SKU = "A", Quantity = 3, Discount = 20 } },
            { "B", new DiscountRule { SKU = "B", Quantity = 2, Discount = 15 } }
        };
        }

        public DiscountRule GetDiscountRuleBySKU(string sku)
        {
            return discountRules.TryGetValue(sku, out var discountRule) ? discountRule : null;
        }
    }
}
