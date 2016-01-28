using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Refactoring
{
    public class Tusc
    {
        private readonly User user;
        private readonly Store store;

        public Tusc(User user, Store store)
        {
            this.user = user;
            this.store = store;
        }

        public void Run()
        {
            WriteCurrentBalanceMessage(user);

            bool done = false;
            while (!done)
            {
                store.WriteProductList();

                Product product = SelectProduct(products);

                if (product == null)
                {
                    SaveData();
                    WaitForConsoleClose();
                    done = true;
                }
                else
                {
                    WriteProductToPurchaseMessage(product);

                    int purchaseQuantity = GetPurchaseQuantity(product);

                    bool insufficientFunds = user.Balance - product.Price * purchaseQuantity < 0;
                    if (insufficientFunds)
                    {
                        WriteNotEnoughMoneyMessage();
                        continue;
                    }

                    bool insufficientStock = product.Quantity <= purchaseQuantity;
                    if (insufficientStock)
                    {
                        WriteOutOfStockMessage(product);
                        continue;
                    }

                    // Check if quantity is greater than zero
                    if (purchaseQuantity > 0)
                    {
                        PurchaseProduct(product, purchaseQuantity);
                    }
                    else
                    {
                        WritePurchaseCancelledMessage();
                    }
                }
            }

            WaitForConsoleClose();
        }

        private void PurchaseProduct(Product product, int purchaseQuantity)
        {
            user.Balance = user.Balance - product.Price * purchaseQuantity;
            product.Quantity = product.Quantity - purchaseQuantity;

            WriteSuccessfulPurchaseMessage(product, purchaseQuantity);
        }

        private void SaveData()
        {
            SaveUsers();
            SaveProducts();
        }

        private void SaveUsers()
        {
            // Write out new balance
            string json = JsonConvert.SerializeObject(users, Formatting.Indented);
            File.WriteAllText(@"Data\Users.json", json);
        }

        private void SaveProducts()
        {
            // Write out new quantities
            string json = JsonConvert.SerializeObject(products, Formatting.Indented);
            File.WriteAllText(@"Data\Products.json", json);
        }

        private static void WritePurchaseCancelledMessage()
        {
            // Quantity is less than zero
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine();
            Console.WriteLine("Purchase cancelled");
            Console.ResetColor();
        }

        private void WriteSuccessfulPurchaseMessage(Product product, int purchaseQuantity)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("You bought " + purchaseQuantity + " " + product.Name);
            Console.WriteLine("Your new balance is " + user.Balance.ToString("C"));
            Console.ResetColor();
        }

        private static void WriteOutOfStockMessage(Product product)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine();
            Console.WriteLine("Sorry, " + product.Name + " is out of stock");
            Console.ResetColor();
        }

        private static void WriteNotEnoughMoneyMessage()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine();
            Console.WriteLine("You do not have enough money to buy that.");
            Console.ResetColor();
        }

        private static int GetPurchaseQuantity(Product product)
        {
            while (true)
            {
                try
                {
                    Console.WriteLine("Enter amount to purchase:");
                    string answer = Console.ReadLine();
                    return Convert.ToInt32(answer);
                }
                catch
                {
                    Console.WriteLine("You have entered an invalid purchase quantity.");
                }
            }
            
        }

        private void WriteProductToPurchaseMessage(Product product)
        {
            Console.WriteLine();
            Console.WriteLine("You want to buy: " + product.Name);
            Console.WriteLine("Your balance is " + user.Balance.ToString("C"));
        }

        private static void WaitForConsoleClose()
        {
            // Prevent console from closing
            Console.WriteLine();
            Console.WriteLine("Press Enter key to exit");
            Console.ReadLine();
        }

        private static Product SelectProduct(List<Product> products) 
        {
            while (true)
            {
                try
                {
                    Console.WriteLine("Enter a number:");
                    string enteredText = Console.ReadLine();
                    int enteredProductIndex = Convert.ToInt32(enteredText);

                    if (IsExitProductSelected(products, enteredProductIndex))
                    {
                        return null;
                    }

                    if (!IsValidProductSelected(products, enteredProductIndex))
                    {
                        throw new Exception("Invalid product number entered.");
                    }

                    return  products[enteredProductIndex];
                }
                catch
                {
                    Console.WriteLine("Invalid number entered, please enter a valid number.");
                }
            }
        }

        private static bool IsExitProductSelected(List<Product> products, int enteredProductIndex)
        {
            return enteredProductIndex == products.Count + 1;
        }

        private static bool IsValidProductSelected(List<Product> products, int enteredProductIndex)
        {
            return enteredProductIndex > 0 || enteredProductIndex <= products.Count;
        }

        private static void WriteCurrentBalanceMessage(User loggedInUser)
        {
            // Show balance 
            Console.WriteLine();
            Console.WriteLine("Your balance is " + loggedInUser.Balance.ToString("C"));
        }
    }
}
