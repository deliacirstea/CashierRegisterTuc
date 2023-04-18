using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashierRegisterTuc
{
    public class Product
    {
        public Product(int productId, string productName, int productPrice)
        {
            ProductId = productId;
            ProductName = productName;
            ProductPrice = productPrice;
            PromotionList = new List<Promotion>();
        }
        public List<Promotion> PromotionList { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int ProductPrice { get; set; }

    }
}
