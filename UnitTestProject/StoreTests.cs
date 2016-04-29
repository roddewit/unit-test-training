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
        const string TEST_PRODUCT_ID = "1";
        List<User> users;
        List<Product> products;
        DataManager dataManager;
        Store store;

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

        private Store createStore(List<User> users, List<Product> products)
        {
            DataManager dataManager = new DataManager(users, products);
            Store store = new Store(users[0], dataManager);
            return store;
        }

        [SetUp]
        public void Test_CodeSetup()
        {
            // Set up variables here for use when testing.  Subsequent tests will alter the specfics of this basic configuration for their individual tests
            users = new List<User>();
            users.Add(createTestUser("Test User", "", 99.99));

            products = new List<Product>();
            products.Add(createTestProduct(TEST_PRODUCT_ID, "Product", 9.99, 10));
        }

        [Test]
        public void Test_PurchaseThrowsNoErrorForValidFunds()
        {
            store = createStore(users, products);
            store.Purchase(TEST_PRODUCT_ID, 10);
            Assert.Pass("No assertion really necessary here");
        }

        [Test]
        public void Test_PurchaseRemovesProductFromStore()
        {
            store = createStore(users, products);
            store.Purchase(TEST_PRODUCT_ID, 9);
            Assert.AreEqual(1, products[0].Quantity);
            //Assert.IsTrue(products[0].Quantity == 1);   Functional and correct, but the first assert is more appropriate
        }

        [Test]
        [ExpectedException(typeof(InsufficientFundsException))]
        public void Test_PurchaseThrowsExceptionWhenBalanceIsTooLow()
        {
            users[0].Balance = 1.00;
            products[0].Price = 1.01;

            store = createStore(users, products);

            store.Purchase(TEST_PRODUCT_ID, 1);
            Assert.Fail("InsufficientFundsException was not thrown");   // Should not reach this point due to the [ExpectedException] attribute on this test
        }

        [Test]
        public void Test_PurchaseThrowsExceptionWhenBalanceIsTooLowVersion2()
        {
            users[0].Balance = 1.00;
            products[0].Price = 1.01;

            store = createStore(users, products);

            try
            {
                store.Purchase(TEST_PRODUCT_ID, 1);
                Assert.Fail("InsufficientFundsException was not thrown");   // Should not reach this point due to an invalid balance
            }
            catch (InsufficientFundsException ex)
            {
                Assert.True(ex is InsufficientFundsException);
            }
        }


        [Test]
        [ExpectedException(typeof(OutOfStockException))]
        public void Test_ProductOutOfStock()
        {
            users[0].Balance = 100.00;
            products[0].Price = 5;

            store = createStore(users, products);

            store.Purchase(TEST_PRODUCT_ID, 12);
            Assert.Fail("OutOfStockException was not thrown");   // Should not reach this point due to the [ExpectedException] attribute on this test
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
