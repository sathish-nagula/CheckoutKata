using CheckoutService.DomainModels;

namespace CheckoutService
{
    public interface IItemRepository
    {
        Item GetItemBySKU(string sku);
    }
}
