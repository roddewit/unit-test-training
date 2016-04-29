using Newtonsoft.Json;
using NUnit.Framework;
using Refactoring;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestProject
{
    [TestFixture]
    class StoreTests
    {
        private const String TEST_PRODUCT_ID = "1";

        private Store createStore(double balance, double product_cost, int quantity)
        {
            var users = getUserInList("Test User", "", balance);
            var products = getProductInList(TEST_PRODUCT_ID, "Product", product_cost, quantity);

            var dataManager = new DataManager(users, products);
            return new Store(users[0], dataManager);
        }

        private Store createStoreWithProducts(double balance, List<Product> products)
        {
            var users = getUserInList("Test User", "", balance);
            var dataManager = new DataManager(users, products);
            return new Store(users[0], dataManager);
        }

        private List<User> getUserInList(string name, string password, double balance)
        {
            var users = new List<User>();
            users.Add(createTestUser("Test User", "", balance));
            return users;
        }

        private List<Product> getProductInList(string id, string name, double price, int quantity)
        {
            var products = new List<Product>();
            products.Add(createTestProduct(id, "Product", price, quantity));
            return products;
        }

        private User createTestUser(string name, string password, double balance)
        {
            User testUser = new User();
            testUser.Name = name;
            testUser.Password = password;
            testUser.Balance = balance;

            return testUser;
        }

        private Product createTestProduct(string id, string name, double price, int quantity)
        {
            Product testProduct = new Product();
            testProduct.Id = id;
            testProduct.Name = name;
            testProduct.Price = price;
            testProduct.Quantity = quantity;

            return testProduct;
        }

        [Test]
        public void Test_PurchaseThrowsNoErrorForValidFunds()
        {
            var store = createStore(99.9, 9, 10);

            store.Purchase(TEST_PRODUCT_ID, 10);

            Assert.Pass("No assertion really necessary here");
        }

        [Test]
        public void Test_PurchaseRemovesProductFromStore()
        {
            var products = getProductInList(TEST_PRODUCT_ID, "Product", 9.99, 10);
            Store store = createStoreWithProducts(99.99, products);

            store.Purchase(TEST_PRODUCT_ID, 9);

            Assert.AreEqual(1, products[0].Quantity);
        }

        [Test]
        [ExpectedException(typeof(InsufficientFundsException))]
        public void Test_PurchaseThrowsExceptionWhenBalanceIsTooLow()
        {
            var store = createStore(1.00, 1.01, 10);

            store.Purchase(TEST_PRODUCT_ID, 1);
        }

        [Test]
        [ExpectedException(typeof(OutOfStockException))]
        public void Test_PurchaseThrowsOutOfStockException()
        {
            var store = createStore(99.99, 9.99, 1);

            store.Purchase(TEST_PRODUCT_ID, 2);
        }

        // THE BELOW CODE IS REQUIRED TO PREVENT THE TESTS FROM MODIFYING THE USERS/PRODUCTS ON FILE
        //  This is not a good unit testing pattern - the unit test dependency on the file system should
        //  actually be broken ... training on how to do this will be coming.
        private List<User> originalUsers;
        private List<Product> originalProducts;

        [SetUp]
        public void Test_Initialize()
        {
            // Load users from data file
            originalUsers = JsonConvert.DeserializeObject<List<User>>(File.ReadAllText(@"Data/Users.json"));

            // Load products from data file
            originalProducts = JsonConvert.DeserializeObject<List<Product>>(File.ReadAllText(@"Data/Products.json"));


        }


        [TearDown]
        public void Test_Cleanup()
        {
            // Restore users
            string json = JsonConvert.SerializeObject(originalUsers, Formatting.Indented);
            File.WriteAllText(@"Data/Users.json", json);

            // Restore products
            string json2 = JsonConvert.SerializeObject(originalProducts, Formatting.Indented);
            File.WriteAllText(@"Data/Products.json", json2);
        }
    }
}
