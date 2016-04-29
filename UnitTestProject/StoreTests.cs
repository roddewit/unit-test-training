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

        private const string TEST_PRODUCT_ID = "1";
        private List<User> users;
        private List<Product> products;
        private DataManager dataManager;
        private Store store;

        [SetUp]
        public void Initialize_Data()
        {
            users = new List<User>();
            products = new List<Product>();
        }

        private void CreateStore()
        {
            dataManager = new DataManager(users, products);
            store = new Store(users[0], dataManager);
        }

        [Test]
        public void Test_PurchaseThrowsNoErrorForValidFunds()
        {
            //Arrange
            users.Add(createTestUser("Test User", "", 99.99));

            products.Add(createTestProduct(TEST_PRODUCT_ID, "Product", 9.99, 10));

            CreateStore();

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

            CreateStore();

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

            CreateStore();

            InsufficientFundsException expectedException = new InsufficientFundsException();

            //Act
            try
            {
                store.Purchase(TEST_PRODUCT_ID, 1);
                Assert.Fail();
            }
            catch(Exception e)
            {
                Assert.AreEqual(expectedException.Message, e.Message);
            }
           
        }

        [Test]
        [ExpectedException(typeof(InsufficientFundsException))]
        public void Test_PurchaseThrowsExceptionWhenBalanceIsTooLowVersion2()
        {
            //Arrange
            users.Add(createTestUser("Test User", "", 1.00));

            products.Add(createTestProduct(TEST_PRODUCT_ID, "Product", 1.01, 10));

            CreateStore();

            //Act
            store.Purchase(TEST_PRODUCT_ID, 1);

        }

        [Test]
        [ExpectedException(typeof(OutOfStockException))]
        public void Test_PurchaseThrowsExceptionWhenOutOfStock()
        {
            //Arrange
            users.Add(createTestUser("Test User", "", 10.00));

            products.Add(createTestProduct(TEST_PRODUCT_ID, "Product", 1.00, 0));

            CreateStore();

            //Act
            store.Purchase(TEST_PRODUCT_ID, 1);

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
