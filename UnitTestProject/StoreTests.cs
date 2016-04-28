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
        private const string TEST_PRODUCT_ID = "1";

        private Product createTestProduct(string id, string name, double price, int quantity)
        {
            Product testProduct = new Product();
            testProduct.Id = id;
            testProduct.Name = name;
            testProduct.Price = price;
            testProduct.Quantity = quantity;

            return testProduct;
        }

        private Store setUpTestStore(double userBalance, List<Product> products)
        {
            var users = new List<User>();
            users.Add(CommonTestSetup.createTestUser("Test User", "", userBalance));

            var dataManager = new DataManager(users, products);
            
            return new Store(users[0], dataManager);
        }

        [Test]
        public void Test_PurchaseThrowsNoErrorForValidFunds()
        {
            //Arrange
            var products = new List<Product>();
            products.Add(createTestProduct(TEST_PRODUCT_ID, "Product", 9.99, 10));

            var store = setUpTestStore(99.99, products);

            //Act
            store.Purchase(TEST_PRODUCT_ID, 10);

            //Assert
            Assert.Pass("No assertion really necessary here");
        }

        [Test]
        public void Test_PurchaseRemovesProductFromStore()
        {
            //Arrange
            var products = new List<Product>();
            products.Add(createTestProduct(TEST_PRODUCT_ID, "Product", 9.99, 10));

            var store = setUpTestStore(99.99, products);

            //Act
            store.Purchase(TEST_PRODUCT_ID, 9);

            //Assert 
            Assert.AreEqual(1, products[0].Quantity);
        }

        [Test]
        public void Test_PurchaseThrowsExceptionWhenBalanceIsTooLow()
        {
            //Arrange
            var products = new List<Product>();
            products.Add(createTestProduct(TEST_PRODUCT_ID, "Product", 1.01, 10));

            var store = setUpTestStore(1.00, products);

            //Act/Assert
            Assert.Throws(typeof(InsufficientFundsException), delegate { store.Purchase(TEST_PRODUCT_ID, 1); });
        }

        [Test]
        public void Test_PurchaseThrowsExceptionWhenBalanceIsTooLowVersion2()
        {
            //Arrange
            var products = new List<Product>();
            products.Add(createTestProduct(TEST_PRODUCT_ID, "Product", 1.01, 10));

            var store = setUpTestStore(1.00, products);

            //Act
            try
            {
                store.Purchase(TEST_PRODUCT_ID, 1);
                Assert.Fail();
            }
            //Assert
            catch (InsufficientFundsException actualException)
            {
                Assert.AreEqual(typeof(InsufficientFundsException), actualException.GetType());
            }
        }

        [Test]
        public void Test_PurchaseThrowsExceptionWhenOutOfStock()
        {
            //Arrange
            var products = new List<Product>();
            products.Add(createTestProduct(TEST_PRODUCT_ID, "Product", 1.00, 0));

            var store = setUpTestStore(99.99, products);

            //Act/Assert
            Assert.Throws(typeof(OutOfStockException), delegate { store.Purchase(TEST_PRODUCT_ID, 1); });
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
