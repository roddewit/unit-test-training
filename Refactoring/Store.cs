using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Refactoring
{
    public class Store
    {
        private readonly User user;
        private readonly List<Product> products;
        private readonly DataManager dataManager;
        
        public Store(User user, DataManager dataManager)
        {
            this.user = user;
            this.dataManager = dataManager;
        }

        public void Purchase(string productId, int quantityPurchased)
        {
            Product product = this.GetProductById(productId);

            if (!UserHasFundsForPurchase(product, quantityPurchased))
            {
                throw new InsufficientFundsException();
            }

            if (!StoreHasStockForPurchase(product, quantityPurchased))
            {
                throw new OutOfStockException();
            }

            product.Quantity -= quantityPurchased;
            user.Balance = user.Balance - product.Price * quantityPurchased;

            dataManager.SaveUser(user);
            dataManager.SaveProduct(product);
        }

        private bool UserHasFundsForPurchase(Product product, int quantity)
        {
            double totalPurchasePrice = product.Price * quantity;
            return user.Balance >= totalPurchasePrice;
        }

        private bool StoreHasStockForPurchase(Product product, int quantity)
        {
            return product.Quantity >= quantity;
        }

        public string GetProductList()
        {
            string output = "\n";
            output += "What would you like to buy?\n";

            foreach (var product in dataManager.Products.Where(p => p.Quantity > 0))
            {
                string productDisplay = GetFormattedProductText(product);
                output += productDisplay + "\n";
            }

            output += "Type quit to exit the application\n";

            return output;
        }

        public int NumberOfProducts()
        {
            return dataManager.Products.Count;
        }

        public Product GetProductById(string productId)
        {
            return dataManager.Products.FirstOrDefault(p => p.Id.Equals(productId));
        }

        public bool ContainsProduct(string productId)
        {
            return dataManager.Products.Count(p => p.Id.Equals(productId)) > 0;
        }

        private static string GetFormattedProductText(Product product)
        {
            return String.Format("{0}: {1} ({2:C})", product.Id, product.Name, product.Price);
        }
    }
}
