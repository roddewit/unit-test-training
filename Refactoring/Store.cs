using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Refactoring
{
    class Store
    {
        private readonly User user;
        private readonly DataManager dataManager;
        
        public Store(User user, DataManager dataManager)
        {
            this.user = user;
            this.dataManager = dataManager;
        }

        public void Purchase(Product product, int quantity)
        {
            if (!UserHasFundsForPurchase(product, quantity))
            {
                throw new Exception("User does not have sufficient funds to purchase product");
            }

            if (!StoreHasStockForPurchase(product, quantity))
            {
                throw new Exception("Store does not enough stock to purchase product quantity.")
            }

            product.Quantity = product.Quantity - quantity;
            user.Balance = user.Balance - product.Price * quantity;
        }

        public bool UserHasFundsForPurchase(Product product, int quantity)
        {
            double totalPurchasePrice = product.Price * quantity;
            return user.Balance >= totalPurchasePrice;
        }

        public bool StoreHasStockForPurchase(Product product, int quantity)
        {
            return product.Quantity >= quantity;
        }

        public void WriteProductList()
        {
            // Prompt for user input
            Console.WriteLine();
            Console.WriteLine("What would you like to buy?");

            foreach (var item in dataManager.Products.Select((product, index) => new { index, product }))
            {
                string productDisplay = GetFormattedProductText(item.product, item.index + 1);
                Console.WriteLine(productDisplay);
            }

            Console.WriteLine(dataManager.Products.Count + 1 + ": Exit");
        }
    }
}
