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
            //Arrange
            users.Add(createTestUser("Test User", "", 99.99));
            products.Add(createTestProduct(TEST_PRODUCT_ID, "Product", 9.99, 10));
            SetUpStore();  

            //Act
            store.Purchase(TEST_PRODUCT_ID, 10);

            //Assert
            Assert.Pass("No assertion really necessary here");
        }

        [Test]
        public void Test_PurchaseRemovesProductFromStore()
        {
            //Arrange
            users.Add(createTestUser("Test User", "", 99.99));
            products.Add(createTestProduct(TEST_PRODUCT_ID, "Product", 9.99, 10));
            SetUpStore();  

            //Act
            store.Purchase(TEST_PRODUCT_ID, 9);

            //Assert 
            Assert.AreEqual(1, products[0].Quantity);
        }

        [Test]
        public void Test_PurchaseThrowsExceptionWhenBalanceIsTooLow()
        {
            //Arrange
            users.Add(createTestUser("Test User", "", 1.00));
            products.Add(createTestProduct(TEST_PRODUCT_ID, "Product", 1.01, 10));
            SetUpStore(); 

            //Assert
            Assert.Throws(typeof(InsufficientFundsException),
                delegate { store.Purchase(TEST_PRODUCT_ID, 1); });
        }

        [Test]
        [ExpectedException(typeof(InsufficientFundsException))]
        public void Test_PurchaseThrowsExceptionWhenBalanceIsTooLowVersion2()
        {
            //Arrange
            users.Add(createTestUser("Test User", "", 1.00));
            products.Add(createTestProduct(TEST_PRODUCT_ID, "Product", 1.01, 10));
            SetUpStore();  

            //Act
            store.Purchase(TEST_PRODUCT_ID, 1);
        }

        [Test]
        [ExpectedException(typeof(OutOfStockException))]
        public void Test_PurchaseThrowsExceptionWhenOutOfStock()
        {
            //Arrange
            users.Add(createTestUser("Test User", "", 99.99));         
            products.Add(createTestProduct(TEST_PRODUCT_ID, "Product", 9.99, 1));
            SetUpStore();           

            //Act
            store.Purchase(TEST_PRODUCT_ID, 2);           
        }

        private List<User> users;
        private List<Product> products;
        private DataManager dataManager;
        private Store store;
        private const string TEST_PRODUCT_ID = "1";

        [SetUp]
        public void Test_Initialize()
        {
            users = new List<User>();
            products = new List<Product>();         
        }

        public void SetUpStore()
        {
            dataManager = new DataManager(users, products);
            store = new Store(users[0], dataManager);
        }

        [TearDown]
        public void Test_Cleanup()
        {

        }
    }
}
