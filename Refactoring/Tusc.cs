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
        private readonly IConsole console;

        public Tusc(IConsole console, User user, Store store)
        {
            this.console = console;
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

                int productIndex = ReadProductIndex(store.NumberOfProducts());

                if (productIndex == store.NumberOfProducts() + 1) 
                {
                    done = true;
                }
                else
                {
                    Product product = store.GetProductByIndex(productIndex-1);
                                
                    WriteProductToPurchaseMessage(product);

                    int purchaseQuantity = GetPurchaseQuantity(product);

                    try
                    {
                        if (purchaseQuantity > 0)
                        {
                            store.Purchase(product, purchaseQuantity);
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

        private void WritePurchaseCancelledMessage()
        {
            // Quantity is less than zero
            console.Clear();
            console.ForegroundColor = ConsoleColor.Yellow;
            console.WriteLine();
            console.WriteLine("Purchase cancelled");
            console.ResetColor();
        }

        private void WriteSuccessfulPurchaseMessage(Product product, int purchaseQuantity)
        {
            console.Clear();
            console.ForegroundColor = ConsoleColor.Green;
            console.WriteLine("You bought " + purchaseQuantity + " " + product.Name);
            console.WriteLine("Your new balance is " + user.Balance.ToString("C"));
            console.ResetColor();
        }

        private void WriteOutOfStockMessage(Product product)
        {
            console.Clear();
            console.ForegroundColor = ConsoleColor.Red;
            console.WriteLine();
            console.WriteLine("Sorry, " + product.Name + " is out of stock");
            console.ResetColor();
        }

        private void WriteInsufficientFundsMessage()
        {
            console.Clear();
            console.ForegroundColor = ConsoleColor.Red;
            console.WriteLine();
            console.WriteLine("You do not have enough money to buy that.");
            console.ResetColor();
        }

        private int GetPurchaseQuantity(Product product)
        {
            int purchaseQuantity;
            bool validIntegerEntered = Int32.TryParse(ReadText("Enter amount to purchase: "), out purchaseQuantity);

            while (!validIntegerEntered)
            {
                console.WriteLine("You have entered an invalid purchase quantity.");
            }

            return purchaseQuantity;            
        }

        private void WriteProductToPurchaseMessage(Product product)
        {
            console.WriteLine();
            console.WriteLine("You want to buy: " + product.Name);
            console.WriteLine("Your balance is " + user.Balance.ToString("C"));
        }

        private void WaitForConsoleClose()
        {
            // Prevent console from closing
            console.WriteLine();
            console.WriteLine("Press Enter key to exit");
            console.ReadLine();
        }

        private string ReadText(string message)
        {
            console.WriteLine(message);
            return console.ReadLine();
        }

        private int ReadProductIndex(int numProducts) 
        {
            int productIndex;
            bool validIntegerEntered = Int32.TryParse(ReadText("Enter a number: "), out productIndex);

            while (!validIntegerEntered || !IsValidProductSelected(numProducts, productIndex)) 
            {
                console.WriteLine("Invalid number entered, pleas enter a valid number");
            }

            return productIndex;
        }

        private bool IsExitProductSelected(List<Product> products, int enteredProductIndex)
        {
            return enteredProductIndex == products.Count + 1;
        }

        private bool IsValidProductSelected(int numProducts, int enteredProductIndex)
        {
            return enteredProductIndex > 0 || enteredProductIndex <= numProducts;
        }

        private void WriteCurrentBalanceMessage(User loggedInUser)
        {
            // Show balance 
            console.WriteLine();
            console.WriteLine("Your balance is " + loggedInUser.Balance.ToString("C"));
        }
    }
}
