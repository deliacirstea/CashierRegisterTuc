namespace CashierRegisterTuc
{
    public class Receipt
    {
        public Receipt()
        {
            InitiateReceiptCounter();
            ReceiptNumber = ++idCounter;
            ReceiptItemList = new List<ReceiptItem>();
            Date = DateTime.Now;
        }
        public DateTime Date { get; set; }
        public int ReceiptNumber
        {
            get; set;
        }

        private static int idCounter = 0;
        public List<ReceiptItem> ReceiptItemList { get; set; }
        public void InitiateReceiptCounter()
        {
            if (idCounter == 0)
            {
                var savePath = Directory.GetCurrentDirectory() + "\\RECEIPT_ID.txt";
                if (File.Exists(savePath))
                {
                    string fileContents = File.ReadAllText(savePath);

                    idCounter = Convert.ToInt32(fileContents);
                }

            }

        }
        public void SaveReceiptNumber()
        {
            var savePath = Directory.GetCurrentDirectory() + "\\RECEIPT_ID.txt";
            try
            {
                File.WriteAllText(savePath, idCounter.ToString());
                Console.WriteLine("Receipt ID is stored in " + savePath);

            }
            catch (Exception ex)
            {
                Console.WriteLine("An error has occurred: " + ex.Message);
            }
        }
        public decimal TotalToPay()
        {
            bool campaign = false;
            int promotionId = 0;
            decimal total = 0;
            foreach (var item in ReceiptItemList)

                total += item.OrderTotal(Date, ref campaign, ref promotionId);

            return total;
        }
        public void UpdateReceipt(ReceiptItem item, int _n)
        {
            var receiptItem = ReceiptItemList.FirstOrDefault(x => x.Product.ProductId == item.Product.ProductId);
            if (receiptItem != null)
            {
                receiptItem.UpdateReceiptItem(_n);
            }

        }
        public void ShowReceipt()
        {
            bool campaign = false;
            string result = "\nReceipt nr: " + this.ReceiptNumber.ToString() + " ";
            result += Date.ToString();
            string result2 = string.Empty;
            decimal FinalFinalPrice = 0;
            foreach (var item in ReceiptItemList)
            {
                int promotiondId = 0;
                var finalPrice = item.OrderTotal(Date, ref campaign, ref promotiondId);
                FinalFinalPrice += finalPrice;
                var promotion = item.Product.PromotionList.FirstOrDefault(p => p.PromotionId == promotiondId);
                if (campaign == true && promotion != null)
                {
                    result2 = result2 + "\n" + item.Product.ProductName + " : " + item.Quantity + " * " + (item.Product.ProductPrice - promotion.DiscountPrice) +"/"+item.Product.PriceType + " = " + finalPrice.ToString();
                }
                else
                {
                    result2 = result2 + "\n" + item.Product.ProductName + " : " + item.Quantity + " * " + item.Product.ProductPrice.ToString()  +"/" + item.Product.PriceType + " = " + finalPrice.ToString();
                }

            }
            result = result + "\n" + result2 + "\nTotal to pay = " + FinalFinalPrice.ToString();

            Console.WriteLine(result);
        }
        public void ClearReceiptItem()
        {
            ReceiptItemList.Clear();
        }
    }

    public class ReceiptItem
    {
        public ReceiptItem(Product product, int quantity)
        {
            Product = product;
            Quantity = quantity;
        }
        public decimal OrderTotal(DateTime d, ref bool usedPromotion, ref int usedPromotionId)
        {
            decimal result = -1;
            usedPromotionId = -1;
            if (Product.PromotionList.Count > 0)
            {
                foreach (var promotion in Product.PromotionList)
                {
                    if (d.Date >= promotion.StartDate.Date && d.Date <= promotion.EndDate.Date)
                    {
                        result = (Product.ProductPrice - promotion.DiscountPrice);
                        Console.WriteLine("\nCampaign discount for" + Product.ProductName + " is found and used! New price applied : " + result.ToString());
                        Console.WriteLine(" Original price : " + (Product.ProductPrice).ToString());
                        usedPromotion = true;
                        usedPromotionId = promotion.PromotionId;
                        return result * Quantity;

                    }
                }
            }

            if (result == -1)
                
                   result = Product.ProductPrice * Quantity;

            return result;
        }
        public Product Product { get; set; }
        public int Quantity { get; set; }
        public void UpdateReceiptItem(int newQuantity)
        {
            this.Quantity = Quantity + newQuantity;
        }
    }
}

