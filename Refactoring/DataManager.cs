using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Refactoring
{
    public class DataManager
    {
        private readonly List<User> users;
        private readonly List<Product> products;

        public List<User> Users
        {
            get { return users; }
        }

        public List<Product> Products
        {
            get { return products; }
        }

        public DataManager(List<User> users, List<Product> products)
        {
            this.users = users;
            this.products = products;
        }

        public void SaveUser(User user)
        {
            SetUser(user);
            SaveUsers();
        }

        private void SetUser(User newUser)
        {
            User currentUser = users.FirstOrDefault(u => u.Name.Equals(newUser.Name) && u.Password.Equals(newUser.Password));
            if (currentUser != null)
            {
                currentUser.Balance = newUser.Balance;
            }
        }

        private void SaveUsers()
        {
            // Write out new balance
            string json = JsonConvert.SerializeObject(users, Formatting.Indented);
            File.WriteAllText(@"Data\Users.json", json);
        }

        public void SaveProduct(Product product)
        {
            SetProduct(product);
            SaveProducts();
        }

        private void SetProduct(Product newProduct)
        {
            Product currentProduct = products.FirstOrDefault(p => p.Name.Equals(newProduct.Name));
            if (currentProduct != null)
            {
                currentProduct.Quantity = newProduct.Quantity;
            }
        }

        private void SaveProducts()
        {
            // Write out new quantities
            string json = JsonConvert.SerializeObject(products, Formatting.Indented);
            File.WriteAllText(@"Data\Products.json", json);
        }
    }
}
