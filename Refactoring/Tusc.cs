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
                Console.Write(store.GetProductList());

                string productId = ReadProductId();

                if (productId.Equals("quit"))
                {
                    done = true;
                }
                else if (!productId.Equals(""))
                {
                    Product product = store.GetProductById(productId);
                                
                    WriteProductToPurchaseMessage(product);

                    int purchaseQuantity = GetPurchaseQuantity(product);

                    try
                    {
                        if (purchaseQuantity > 0)
                        {
                            store.Purchase(productId, purchaseQuantity);
                            WriteSuccessfulPurchaseMessage(product, purchaseQuantity);
                        }
                        else
                        {
                            WritePurchaseCancelledMessage();
                        }
                    }
                    catch (InsufficientFundsException)
                    {
                        WriteInsufficientFundsMessage();
                    }
                    catch (OutOfStockException)
                    {
                        WriteOutOfStockMessage(product);
                    }
                }
            }

            WaitForConsoleClose();
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

        private static void WriteInsufficientFundsMessage()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine();
            Console.WriteLine("You do not have enough money to buy that.");
            Console.ResetColor();
        }

        private static int GetPurchaseQuantity(Product product)
        {
            int purchaseQuantity;
            bool validIntegerEntered = Int32.TryParse(ReadText("Enter amount to purchase: "), out purchaseQuantity);

            while (!validIntegerEntered)
            {
                Console.WriteLine("You have entered an invalid purchase quantity.");
            }

            return purchaseQuantity;            
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

        private static string ReadText(string message)
        {
            Console.WriteLine(message);
            return Console.ReadLine();
        }

        private string ReadProductId() 
        {
            string productId = ReadText("Enter a product ID: ");
            if (!productId.Equals("quit") && !store.ContainsProduct(productId))
            {
                Console.WriteLine("Invalid product ID entered, please enter a valid ID");
                productId = "";
            }
            return productId;
        }
        

        private static void WriteCurrentBalanceMessage(User loggedInUser)
        {
            // Show balance 
            Console.WriteLine();
            Console.WriteLine("Your balance is " + loggedInUser.Balance.ToString("C"));
        }
    }
}
