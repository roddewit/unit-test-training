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

        private DataManager PreArrange(string userName, string userPassword, double userBalance, string productName, double productPrice, int productQty )
        {
            var users = new List<User>();
            users.Add(createTestUser(userName, userPassword, userBalance));

            var products = new List<Product>();
            products.Add(createTestProduct(TEST_PRODUCT_ID, productName, productPrice, productQty));
            var dataManager = new DataManager(users, products);
            return dataManager;
        }


        [Test]
        public void Test_PurchaseThrowsNoErrorForValidFunds()
        {
            //Arrange
            var dataManager = PreArrange("Test User", "", 99.99, "Product", 9.99, 10);
            var store = new Store(dataManager.Users[0], dataManager);
            //Act
            store.Purchase(TEST_PRODUCT_ID, 10);

            //Assert
            Assert.Pass("No assertion really necessary here");
        }

        [Test]
        public void Test_PurchaseRemovesProductFromStore()
        {
            //Arrange
            var dataManager = PreArrange("Test User", "", 99.99, "Product", 9.99, 10);
            var store = new Store(dataManager.Users[0], dataManager);
            //Act
            store.Purchase(TEST_PRODUCT_ID, 9);
            //Assert 
            //(choose the appropriate statement(s))
            Assert.AreEqual(1, store.GetProductById(TEST_PRODUCT_ID).Quantity);
            //Assert.AreSame(1, products[0].Quantity);
            //Assert.IsTrue(products[0].Quantity == 1);
        }

        [Test]
        public void Test_PurchaseThrowsExceptionWhenBalanceIsTooLow()
        {
            //Arrange
            var dataManager = PreArrange("Test User", "", 1.00, "Product", 1.01, 10);
            var store = new Store(dataManager.Users[0], dataManager);
            //Act
            try
            {
                store.Purchase(TEST_PRODUCT_ID, 1);
            }
            catch(Exception e)
            {
                Assert.AreEqual("Exception of type 'Refactoring.InsufficientFundsException' was thrown.", e.Message);
            }
            //Assert
        }

        [Test]
        public void Test_PurchaseThrowsExceptionWhenBalanceIsTooLowVersion2()
        {
            //Arrange
            var dataManager = PreArrange("Test User", "", 1.00, "Product", 1.01, 10);
            var store = new Store(dataManager.Users[0], dataManager);
            //Act
            double dBal = dataManager.Users[0].Balance - store.GetProductById(TEST_PRODUCT_ID).Price;
            //Assert
            Assert.IsFalse(dBal > 0);
        }
        [Test]
        public void Test_StoreHasStockForPurchase()
        {
            //Arrange
            var dataManager = PreArrange("Test User", "", 10.00, "Product", 0.55, 10);
            var store = new Store(dataManager.Users[0], dataManager);
            //Act
            try
            {
                store.Purchase(TEST_PRODUCT_ID, 12);
            }
            catch (Exception e)
            {
                if (e.GetType().Name == typeof(OutOfStockException).Name)
                    Assert.IsFalse((store.GetProductById(TEST_PRODUCT_ID).Quantity - 12) > 0);
                if ( e.GetType().Name ==typeof(InsufficientFundsException).Name)
                    Assert.IsFalse(dataManager.Users[0].Balance - store.GetProductById(TEST_PRODUCT_ID).Price * 12 > 0);

            }
            //Assert
            

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
