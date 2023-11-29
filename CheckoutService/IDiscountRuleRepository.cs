using CheckoutService.DomainModels;

namespace CheckoutService
{
    public interface IDiscountRuleRepository
    {
        DiscountRule GetDiscountRuleBySKU(string sku);
    }

}
