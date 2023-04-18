namespace CashierRegisterTuc
{
    public class Promotion
    {
        public Promotion(int productId, decimal discountPrice, DateTime startDate, DateTime endDate)
        {
            InitiatePromotionCounter();
            PromotionId = ++idPromotionCounter;
            ProductId = productId;
            DiscountPrice = discountPrice;
            StartDate = startDate;
            EndDate = endDate;
            SavePromotionNumber();
        }
        public Promotion(int promotionId, int productId, DateTime startDate, DateTime endDate, decimal discountPrice)
        {
            PromotionId = promotionId;
            ProductId = productId;
            StartDate = startDate;
            EndDate = endDate;
            DiscountPrice = discountPrice;
        }
        private static int idPromotionCounter = 0;
        public int PromotionId { get; set; }
        public int ProductId { get; set; }
        public decimal DiscountPrice { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public void SavePromotionNumber()
        {
            var savePath = Directory.GetCurrentDirectory() + "\\PROMOTON_ID.txt";
            try
            {
                File.WriteAllText(savePath, this.PromotionId.ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Cannot write the promotion with ID : {PromotionId}. The following error is given : " + ex.Message);
            }
        }
        public void InitiatePromotionCounter()
        {
            if (idPromotionCounter == 0)
            {

                var savePath = Directory.GetCurrentDirectory() + "\\PROMOTON_ID.txt";
                if (File.Exists(savePath))
                {
                    string fileContents = File.ReadAllText(savePath);
                    Console.WriteLine(fileContents);

                    idPromotionCounter = Convert.ToInt32(fileContents);
                }

            }

        }

    }
}

