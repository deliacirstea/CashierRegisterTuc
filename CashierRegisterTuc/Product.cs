using System.IO;

namespace CashierRegisterTuc
{
    public class Product
    {
        public Product(int productId, string productName, int productPrice, PriceType priceType)
        {
            ProductId = productId;
            ProductName = productName;
            ProductPrice = productPrice;
            PriceType = priceType;
            PromotionList = new List<Promotion>();
        }
        public List<Promotion> PromotionList { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int ProductPrice { get; set; }
        public PriceType PriceType { get; set; }
    }
}
