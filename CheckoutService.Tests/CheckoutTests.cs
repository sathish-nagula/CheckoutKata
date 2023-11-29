namespace CheckoutService.Tests
{
    using CheckoutService.DomainModels;
    using Moq;
    using System;
    using Xunit;

    namespace CheckoutKata.Tests
    {
        public class CheckoutServiceTests
        {
            [Fact]
            public void Scan_ValidSKU_ShouldAddItemToScannedItems()
            {
                var mockItemRepository = new Mock<IItemRepository>();
                var mockDiscountRuleRepository = new Mock<IDiscountRuleRepository>();
                var checkoutService = new Checkout(mockItemRepository.Object, mockDiscountRuleRepository.Object);

                mockItemRepository.Setup(r => r.GetItemBySKU("A")).Returns(new Item { SKU = "A", Price = 50 });

                checkoutService.Scan("A");

                Assert.Equal(1, checkoutService.ScannedItems["A"]);
            }

            [Fact]
            public void Scan_InvalidSKU_ShouldThrowException()
            {
                var mockItemRepository = new Mock<IItemRepository>();
                var mockDiscountRuleRepository = new Mock<IDiscountRuleRepository>();
                var checkoutService = new Checkout(mockItemRepository.Object, mockDiscountRuleRepository.Object);

                Assert.Throws<ArgumentException>(() => checkoutService.Scan(null));
                Assert.Throws<ArgumentException>(() => checkoutService.Scan(""));
                Assert.Throws<InvalidOperationException>(() => checkoutService.Scan("X"));
            }

            [Fact]
            public void GetTotalPrice_NoItemsScanned_ShouldReturnZero()
            {
                var mockItemRepository = new Mock<IItemRepository>();
                var mockDiscountRuleRepository = new Mock<IDiscountRuleRepository>();
                var checkoutService = new Checkout(mockItemRepository.Object, mockDiscountRuleRepository.Object);

                var totalPrice = checkoutService.GetTotalPrice();

                Assert.Equal(0, totalPrice);
            }

            [Theory]
            [InlineData("A", 50)]
            [InlineData("B", 30)]
            [InlineData("C", 20)]
            [InlineData("D", 15)]
            public void GetTotalPrice_OneItemScanned_ShouldReturnItemPrice(string sku, decimal expectedPrice)
            {
                var mockItemRepository = new Mock<IItemRepository>();
                var mockDiscountRuleRepository = new Mock<IDiscountRuleRepository>();
                var checkoutService = new Checkout(mockItemRepository.Object, mockDiscountRuleRepository.Object);

                mockItemRepository.Setup(r => r.GetItemBySKU(sku)).Returns(new Item { SKU = sku, Price = expectedPrice });

                checkoutService.Scan(sku);
                var totalPrice = checkoutService.GetTotalPrice();

                Assert.Equal(expectedPrice, totalPrice);
            }

            [Theory]
            [InlineData(new string[] { "A", "B", "C", "D" }, 115)]
            [InlineData(new string[] { "A", "A", "B", "C" }, 150)]
            [InlineData(new string[] { "B", "B", "B", "D" }, 105)]
            [InlineData(new string[] { "C", "C", "C", "C" }, 80)]
            public void GetTotalPrice_MultipleItemsScanned_ShouldReturnSumOfItemPrices(string[] skus, decimal expectedPrice)
            {
                var mockItemRepository = new Mock<IItemRepository>();
                var mockDiscountRuleRepository = new Mock<IDiscountRuleRepository>();
                var checkoutService = new Checkout(mockItemRepository.Object, mockDiscountRuleRepository.Object);

                mockItemRepository.Setup(r => r.GetItemBySKU("A")).Returns(new Item { SKU = "A", Price = 50 });
                mockItemRepository.Setup(r => r.GetItemBySKU("B")).Returns(new Item { SKU = "B", Price = 30 });
                mockItemRepository.Setup(r => r.GetItemBySKU("C")).Returns(new Item { SKU = "C", Price = 20 });
                mockItemRepository.Setup(r => r.GetItemBySKU("D")).Returns(new Item { SKU = "D", Price = 15 });

                foreach (var sku in skus)
                {
                    checkoutService.Scan(sku);
                }
                var totalPrice = checkoutService.GetTotalPrice();

                Assert.Equal(expectedPrice, totalPrice);
            }

            [Theory]
            [InlineData(new string[] { "A", "A", "A" }, 130)]
            [InlineData(new string[] { "B", "B" }, 45)]
            [InlineData(new string[] { "A", "A", "A", "B", "B" }, 175)]
            [InlineData(new string[] { "A", "A", "A", "A", "B", "B", "B", "C", "D" }, 290)]
            public void GetTotalPrice_MultipleItemsScannedWithDiscounts_ShouldReturnSumOfItemPricesWithDiscounts(string[] skus, decimal expectedPrice)
            {
                var mockItemRepository = new Mock<IItemRepository>();
                var mockDiscountRuleRepository = new Mock<IDiscountRuleRepository>();
                var checkoutService = new Checkout(mockItemRepository.Object, mockDiscountRuleRepository.Object);

                mockItemRepository.Setup(r => r.GetItemBySKU("A")).Returns(new Item { SKU = "A", Price = 50 });
                mockItemRepository.Setup(r => r.GetItemBySKU("B")).Returns(new Item { SKU = "B", Price = 30 });
                mockItemRepository.Setup(r => r.GetItemBySKU("C")).Returns(new Item { SKU = "C", Price = 20 });
                mockItemRepository.Setup(r => r.GetItemBySKU("D")).Returns(new Item { SKU = "D", Price = 15 });

                mockDiscountRuleRepository.Setup(r => r.GetDiscountRuleBySKU("A")).Returns(new DiscountRule { SKU = "A", Quantity = 3, Discount = 20 });
                mockDiscountRuleRepository.Setup(r => r.GetDiscountRuleBySKU("B")).Returns(new DiscountRule { SKU = "B", Quantity = 2, Discount = 15 });

                foreach (var sku in skus)
                {
                    checkoutService.Scan(sku);
                }
                var totalPrice = checkoutService.GetTotalPrice();

                Assert.Equal(expectedPrice, totalPrice);
            }
        }
    }
}