using CashierRegisterTuc;

public class App
{
    List<Receipt> todayReceiptList = new();
    List<Product> productList = new();
    public void Run()
    {
        loadStock();
        DisplayHeader();
        DisplayMenu();
        DisplayFooter();
    }
    private void DisplayHeader()
    {
        Console.BackgroundColor = ConsoleColor.DarkRed;
        Console.WriteLine("=========================");
        Console.WriteLine("===== Delia's store =====");
        Console.WriteLine("=========================");
        Console.BackgroundColor = ConsoleColor.Black;
        Console.WriteLine("\n");
    }
    private void DisplayFooter()
    {
        Console.BackgroundColor = ConsoleColor.DarkRed;
        Console.WriteLine("\n");
        Console.WriteLine("=====================================");
        Console.WriteLine("= Press key 0 to exit the program =");
        Console.WriteLine("=====================================");
        Console.ReadKey();
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine("\n");
    }
    private void DisplayMenu()
    {
        Console.WriteLine("Select one of the following:");
        Console.WriteLine("1. New customer");
        Console.WriteLine("2. Admin");
        Console.WriteLine("0. Close the program");

        int menuSelect;
        while (true)
        {
            if (int.TryParse(Console.ReadLine(), out menuSelect))
            {
                if (menuSelect == 1)
                {
                    NewPurchase();
                }
                if (menuSelect == 2)
                {
                    AdminMenuTools();
                }
                if (menuSelect == 0)
                {
                    DisplayFooter();
                    Environment.Exit(0);
                }
                else
                {
                    Console.WriteLine("You have not made a valid choice, try again.\n");
                }
            }
            else
            {
                Console.WriteLine("In order to make a selection of the menu, you must select an integer(a number between 0 and 2).Try again!");
            }
        }

    }
    private void SecondMenu()
    {
        Console.WriteLine(" Select one of the following options:");
        Console.WriteLine("--------------------------------------");
        Console.WriteLine("1. Go back to the main menu");
        Console.WriteLine("2. Check Admin Tools");
        Console.WriteLine("3. Check the stock list");
        Console.WriteLine("0. Exit");

        int menuSelect;
        while (true)
        {
            if (int.TryParse(Console.ReadLine(), out menuSelect))
            {
                if (menuSelect == 1)
                {
                    DisplayMenu();
                }
                if (menuSelect == 2)
                {
                    AdminMenuTools();
                }
                if (menuSelect == 3)
                {
                    ReadFromFile();
                }
                if (menuSelect == 0)
                {
                    Environment.Exit(0);
                }
                else
                {
                    Console.WriteLine("You have not made a valid choice. Try again.");
                }
            }
            else
            {
                Console.WriteLine("In order to make a selection of the menu, you must select an integer(a number between 0 and 3).Try again!");
            }
        }
    }
    public void AdminMenuTools()
    {
        Console.WriteLine("---------------------------------");
        Console.WriteLine("Welcome to Admin Tools, select one of the following:\n");
        Console.WriteLine("1. Add a new product");
        Console.WriteLine("2. Edit price for a product");
        Console.WriteLine("3. Edit name for a product");
        Console.WriteLine("4. Check the product list");
        Console.WriteLine("5. Create a campaign");
        Console.WriteLine("6. Check the campaign list (both comming and existing)");
        Console.WriteLine("7. Remove a campaign");
        Console.WriteLine("8. Return to main menu");
        Console.WriteLine("0. Exit the program");

        int menuSelect;
        while (true)
        {
            if (int.TryParse(Console.ReadLine(), out menuSelect))
            {
                if (menuSelect == 1)
                {
                    AddNewStock();
                }
                if (menuSelect == 2)
                {
                    EditPriceStock();
                }
                if (menuSelect == 3)
                {
                    EditNameProduct();
                }
                if (menuSelect == 4)
                {
                    ReadFromFile();
                }
                if (menuSelect == 5)
                {
                    CreatePromotion();
                }
                if (menuSelect == 6)
                {
                    CheckPromotion();
                }
                if (menuSelect == 7)
                {
                    RemovePromotion();
                }
                if (menuSelect == 8)
                {
                    DisplayMenu();
                }
                if (menuSelect == 0)
                {
                    Environment.Exit(0);
                }
                else
                {
                    Console.WriteLine("You have not made a valid choice");
                }
            }
            else
            {
                Console.WriteLine("In order to make a selection of the menu, you must select an integer(a number between 0 and 8).Try again!");
            }
        }

    }
    private void MoreInfo()
    {
        Console.WriteLine("-----------------------------------------");
        Console.WriteLine("If you are ready to pay please write: PAY");
        Console.WriteLine("If you wish to go back to the menu press 1.");
        Console.WriteLine("If you wish to continue shopping");
        Console.WriteLine("<product PLU> <quantity> \n");
        Console.WriteLine("-----------------------------------------");
        int answer = Convert.ToInt32(Console.ReadLine());
        if (answer == 1)
        {
            DisplayMenu();
        }
        else
        {
            Console.WriteLine("Not a valid choice.Try again please.");
            DisplayMenu();
        }
    }
    private void ReadFromFile()
    {
        string filePath = "stock.txt";

        string stockCheck = File.ReadAllText(filePath);
        Console.WriteLine(stockCheck);
        SecondMenu();
    }
    public void loadStock()
    {
        List<string> lines = new List<string>();

        string filePath = "stock.txt";

        lines = File.ReadAllLines(filePath).ToList();

        foreach (string line in lines)
        {
            string[] items = line.Split('!');

            Product product = new Product(Convert.ToInt32(items[0]), items[1], Convert.ToInt32(items[2]), (PriceType)Enum.Parse(typeof(PriceType), items[3]));
            productList.Add(product);
        }
        if (File.Exists("promotion.txt"))
        {
            LoadPromotion();
        }
    }
    private void NewPurchase()
    {
        Console.Clear();
        Receipt receipt = new Receipt();

        while (true)
        {
            Console.WriteLine("Write in the following: ");
            Console.WriteLine("***********************************************");
            Console.WriteLine("Product PLU (price look-up number or product ID) followed by the quantity desired");
            Console.WriteLine("<product PLU> <quantity> \n");

            var feedNumbers = Console.ReadLine();

            if (feedNumbers.ToUpper() == "PAY")
            {
                todayReceiptList.Add(receipt);
                SaveTheReceipt(todayReceiptList);
                todayReceiptList.Clear();
                Run();
            }
            if (feedNumbers.Length == 0)
            {
                Console.WriteLine("Wrong product ID. Please try again.");
                continue;
            }
            var numbers = feedNumbers.Split(' ');

            if (receipt.ReceiptItemList != null)
            {
                var receiptItem = receipt.ReceiptItemList.FirstOrDefault(x => x.Product.ProductId == Convert.ToInt32(numbers[0]));

                if (receiptItem != null)
                {
                    receipt.UpdateReceipt(receiptItem, Convert.ToInt32(numbers[1]));
                    receipt.ShowReceipt();
                    continue;
                }
            }

            var product2 = productList.FirstOrDefault(x => x.ProductId == Convert.ToInt32(numbers[0]));
            if (product2 == null)
            {
                Console.WriteLine("Invalid Product ID. Please try again.\n");
                DisplayMenu();
                continue;
            }

            ReceiptItem newReceipt = new ReceiptItem(product2, Convert.ToInt32(numbers[1]));
            receipt.ReceiptItemList.Add(newReceipt);

            receipt.ShowReceipt();
            MoreInfo();
        }

    }
    private void SaveStock(List<Product> products)
    {
        List<string> lines = new List<string>();
        foreach (Product product in products)
        {
            string line = $"{product.ProductId}!{product.ProductName}!{product.ProductPrice}!{product.PriceType}";
            lines.Add(line);
        }

        string filePath = "stock.txt";

        File.WriteAllLines(filePath, lines);
    }
    private void SaveTheReceipt(List<Receipt> list)
    {
        string writingTheReceipt = "";
        string ReceiptDate = "";

        foreach (Receipt receipt in list)
        {
            bool campaign = false;
            int promotionId = 0;
            writingTheReceipt += "\nReceipt nr: " + receipt.ReceiptNumber.ToString() + " ";
            writingTheReceipt += receipt.Date.ToString("MM/dd/yyyy HH:mm:ss") + "\n";
            ReceiptDate = receipt.Date.ToString("yyyyMMdd");

            foreach (ReceiptItem item in receipt.ReceiptItemList)
            {
                writingTheReceipt += item.Product.ProductName
                    + " :  "
                    + item.Product.ProductPrice.ToString()
                    + "/"
                    + item.Product.PriceType
                    + " * "
                    + item.Quantity.ToString()
                    + " = "
                    + item.OrderTotal(receipt.Date, ref campaign, ref promotionId).ToString()
                    + " kr "
                    + "\n";
            }
            writingTheReceipt += "Total to pay:" + receipt.TotalToPay().ToString() + " kr \n";
            writingTheReceipt += "-----------------------------------";

            receipt.SaveReceiptNumber();
            receipt.ClearReceiptItem();
        }

        var outPutPath = Directory.GetCurrentDirectory() + "\\RECEIPT_" + ReceiptDate + ".txt";
        Console.WriteLine("Receipt is stored in " + outPutPath);
        try
        {
            File.AppendAllText(outPutPath, writingTheReceipt);
        }
        catch (Exception ex)
        {
            Console.WriteLine("An error has occurred: " + ex.Message);
        }
    }
    private void AddNewStock()
    {
        Console.WriteLine("Enter new product details");
        Console.Write("Enter unique Product ID -> ");
        int productId = Convert.ToInt32(Console.ReadLine());
        Console.Write("Product name -> ");
        string name = Console.ReadLine();
        Console.Write("Product price -> ");
        int price = Convert.ToInt32(Console.ReadLine());

        Console.WriteLine("Enter price type:");
        Console.WriteLine("1. Price per kg");
        Console.WriteLine("2. Price per piece");
        int priceTypeInput = Convert.ToInt32(Console.ReadLine());
        PriceType priceType = (PriceType)(priceTypeInput - 1);

        productList.Add(new Product(productId, name, price, priceType));
        Console.WriteLine($"New product added: {productId} {name} {price} {priceType}");

        SaveStock(productList);

        SecondMenu();
    }
    private void EditPriceStock()
    {
        Console.WriteLine("Enter the product ID you wish to edit:");
        int productId = Convert.ToInt32(Console.ReadLine());

        var product = productList.FirstOrDefault(p => p.ProductId == productId);
        if (product == null)
        {
            Console.WriteLine("Product not found. Try again!");
            return;
        }

        Console.WriteLine($"Current price for {product.ProductName} is {product.ProductPrice}");
        Console.Write("Enter new price -> ");
        int price = Convert.ToInt32(Console.ReadLine());
        product.ProductPrice = price;
        SaveStock(productList);

        SecondMenu();
    }
    private void EditNameProduct()
    {
        Console.WriteLine("Enter the product ID you wish to edit:");
        int productId = Convert.ToInt32(Console.ReadLine());

        var product = productList.FirstOrDefault(p => p.ProductId == productId);
        if (product == null)
        {
            Console.WriteLine("Product not found.");
            return;
        }

        Console.WriteLine($"Current name for product id {productId} is {product.ProductName}");
        Console.Write("Enter new name -> ");
        string newName = Console.ReadLine();
        product.ProductName = newName;
        SaveStock(productList);

        SecondMenu();
    }
    public void CreatePromotion()
    {
        Console.WriteLine("Enter the product Id selected for campaign");
        int productId = Convert.ToInt32(Console.ReadLine());

        if (productList.Count > 0)
        {
            Product product = productList.Find(p => p.ProductId == productId);

            if (product == null)
            {
                Console.WriteLine($"Product with Id {productId} not found in stock database");
                return;
            }

            Console.WriteLine($"Current price for {product.ProductName} = {product.ProductPrice}");
            Console.Write("Enter new discount (ex: 2) -> ");

            int discountPrice = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Enter start date (yyyy-mm-dd)");
            DateTime startDate = DateTime.Parse(Console.ReadLine());

            Console.WriteLine("Enter end date (yyyy-mm-dd)");
            DateTime endDate = DateTime.Parse(Console.ReadLine());

            Promotion promotion = new Promotion(product.ProductId, discountPrice, startDate, endDate);

            product.PromotionList.Add(promotion);

            string writingThePromotion = "";
            writingThePromotion = promotion.PromotionId + "!" + promotion.ProductId + "!" + promotion.StartDate.ToString() + "!" + promotion.EndDate.ToString() + "!" + promotion.DiscountPrice.ToString() + "\n";


            try
            {
                File.AppendAllText("promotion.txt", writingThePromotion);
            }
            catch (Exception ex)
            {

                Console.WriteLine("An error has occurred: " + ex.Message);
            }

            Console.WriteLine($"\n Promotion with id no: {promotion.PromotionId} is created and stored. ");

        }

        AdminMenuTools();
    }
    public void LoadPromotion()
    {
        string filename = @"promotion.txt";

        List<string> lines = new List<string>();

        if (!File.Exists(filename))
        {
            return;
        }
        try
        {
            lines = File.ReadAllLines("promotion.txt").ToList();
            foreach (string line in lines)
            {
                string[] items = line.Split('!');

                Promotion promotion = new Promotion(Convert.ToInt32(items[0]), Convert.ToInt32(items[1]), Convert.ToDateTime(items[2]), Convert.ToDateTime(items[3]), Convert.ToDecimal(items[4]));

                foreach (Product p in productList)
                {
                    if (p.ProductId == promotion.ProductId)
                    {
                        p.PromotionList.Add(promotion);
                    }
                }

            }
        }
        catch (IOException ex)
        {
            Console.WriteLine("\n---------------------------------------------------------");
            Console.WriteLine("An error has occurred while reading the file: " + ex.Message);
        }
        catch (FormatException ex)
        {
            Console.WriteLine("\n---------------------------------------------------------");
            Console.WriteLine("An error has occurred while parsing the file: " + ex.Message);
        }
        catch (Exception ex)
        {
            Console.WriteLine("\n---------------------------------------------------------");
            Console.WriteLine("An unexpected error has occurred: " + ex.Message);
        }
        return;
    }
    public void CheckPromotion()
    {
        string filename = @"promotion.txt";

        if (!File.Exists(filename))
        {
            Console.WriteLine("There are no active campaigns at the moment.");
            Console.WriteLine("---------------------------------------------");
            return;
        }
        string promotionCheck = File.ReadAllText("promotion.txt");
        String header = "<PromotionId >!<ProductId>!<StartDate>!<EndDate>!<DiscountPrice>";
        Console.WriteLine(header);
        Console.WriteLine(promotionCheck);
        Console.WriteLine("\n");

        DisplayMenu();
    }
    private void SavePromotionsToFile()
    {
        string filename = @"promotion.txt";
        List<string> lines = new List<string>();

        foreach (Product product in productList)
        {
            foreach (Promotion promotion in product.PromotionList)
            {
                string line = $"{promotion.PromotionId}!{promotion.ProductId}!{promotion.StartDate}!{promotion.EndDate}!{promotion.DiscountPrice}";
                lines.Add(line);
            }
            File.WriteAllLines(filename, lines);
        }
    }
    private void RemovePromotion()
    {
        Console.WriteLine("In order to remove a campain you must enter the product ID first to identify the campaign:");
        int clientProductId = Convert.ToInt32(Console.ReadLine());

        Product product = productList.Find(p => p.ProductId == clientProductId);
        if (product == null)
        {
            Console.WriteLine($"Product with ID {clientProductId} not found.");
            return;
        }

        Console.WriteLine("Enter the promotion ID you wish to remove:");
        int promotionId = Convert.ToInt32(Console.ReadLine());

        Promotion promotion = product.PromotionList.Find(p => p.PromotionId == promotionId);
        if (promotion == null)
        {
            Console.WriteLine($"Promotion with ID {promotionId} not found for product with ID {clientProductId}.");
            return;
        }

        Console.WriteLine($"Do you want to remove the promotion with ID {promotionId}? (Y/N)");
        string input = Console.ReadLine();
        List<Promotion> promotionsOnly = new();
        if (input.ToLower() == "y")
        {
            product.PromotionList.Remove(promotion);

            Console.WriteLine($"Promotion with ID {promotionId} has been removed.");
            SavePromotionsToFile();
        }
        else
        {
            Console.WriteLine($"Promotion with ID {promotionId} has not been removed.");
        }
        SecondMenu();
    }
}