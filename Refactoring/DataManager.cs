using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Refactoring
{
    class DataManager
    {
        private readonly List<User> users;
        private readonly List<Product> products;

        public readonly List<Product> Products
        {
            get { return products; }
        }

        public DataManager(List<User> users, List<Product> products)
        {
            this.users = users;
            this.products = products;
        }

        public void Save()
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
    }
}
