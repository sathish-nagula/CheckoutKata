using CheckoutService.DomainModels;

namespace CheckoutService
{
    public class ItemRepository : IItemRepository
    {
        private readonly Dictionary<string, Item> items;

        public ItemRepository()
        {
            items = new Dictionary<string, Item>()
            {
                { "A", new Item { SKU = "A", Price = 50 } },
                { "B", new Item { SKU = "B", Price = 30 } },
                { "C", new Item { SKU = "C", Price = 20 } },
                { "D", new Item { SKU = "D", Price = 15 } }
            };
        }

        public Item GetItemBySKU(string sku)
        {
            return items.TryGetValue(sku, out var item) ? item : null;
        }
    }

}
