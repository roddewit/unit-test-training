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
        private List<User> users;
        private List<Product> products;

        private void addTestUser(double balance)
        {
            User testUser = new User();
            testUser.Name = "Test User";
            testUser.Password = "";
            testUser.Balance = balance;

            users.Add(testUser);
        }

        private void addTestProduct(double price, int quantity)
        {
            Product testProduct = new Product();
            testProduct.Id = TEST_PRODUCT_ID;
            testProduct.Name = "Product";
            testProduct.Price = price;
            testProduct.Quantity = quantity;

            products.Add(testProduct);
        }

        private Store buildStore()
        {
            var dataManager = new DataManager(users, products);
            return new Store(users[0], dataManager);
        }

        [SetUp]
        public void beforeEachTest()
        {
            users = new List<User>();
            products = new List<Product>();
        }

        [Test]
        public void Test_PurchaseThrowsNoErrorForValidFunds()
        {
            //Arrange
            addTestUser(99.99);
            addTestProduct(9.99, 10);

            var store = buildStore();

            //Act
            store.Purchase(TEST_PRODUCT_ID, 10);

            //Assert
            Assert.Pass("No assertion really necessary here");
        }

        [Test]
        public void Test_PurchaseRemovesProductFromStore()
        {
            //Arrange
            addTestUser(99.99);
            addTestProduct(9.99, 10);

            var store = buildStore();

            //Act
            store.Purchase(TEST_PRODUCT_ID, 9);

            //Assert 
            //(choose the appropriate statement(s))
            Assert.AreEqual(1, products[0].Quantity);
            //Assert.AreSame(1, products[0].Quantity);
            //Assert.IsTrue(products[0].Quantity == 1);
        }

        [Test]
        [ExpectedException(typeof(InsufficientFundsException))]
        public void Test_PurchaseThrowsExceptionWhenBalanceIsTooLow()
        {
            //Arrange
            addTestUser(1.00);
            addTestProduct(1.01, 10);

            var store = buildStore();

            //Act
            store.Purchase(TEST_PRODUCT_ID, 1);

            //Assert
        }

        [Test]
        public void Test_PurchaseThrowsExceptionWhenBalanceIsTooLowVersion2()
        {
            //Arrange
            addTestUser(1.00);
            addTestProduct(1.01, 10);

            var store = buildStore();

            //Act/Assert
            try
            {
                store.Purchase(TEST_PRODUCT_ID, 1);
            }
            catch (InsufficientFundsException ex)
            {
                Assert.Pass();
            }
            Assert.Fail();
        }

        [Test]
        [ExpectedException(typeof(OutOfStockException))]
        public void Test_PurchaseThrowsExceptionWhenSupplyIsTooLow()
        {
            //Arrange
            addTestUser(100.00);
            addTestProduct(1.00, 10);

            var store = buildStore();

            //Act
            store.Purchase(TEST_PRODUCT_ID, 20);

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
