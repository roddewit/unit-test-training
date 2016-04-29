using Newtonsoft.Json;
using NUnit.Framework;
using Refactoring;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace UnitTestProject
{
    [TestFixture]
    class StoreTests
    {
        private Store _store;

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

        private Store createTestStore(User user, params Product[] products)
        {
            var users = new List<User>();
            users.Add(user);

            var productList = products.ToList();

            var dataManager = new DataManager(users, productList);
            return new Store(users[0], dataManager);
        }

        [Test]
        public void Test_PurchaseThrowsNoErrorForValidFunds()
        {
            //Arrange
            const string TEST_PRODUCT_ID = "1";

            _store = 
                createTestStore(
                    createTestUser("Test User", "", 99.99), 
                    createTestProduct(TEST_PRODUCT_ID, "Product", 9.99, 10)
                );

            //Act
            _store.Purchase(TEST_PRODUCT_ID, 10);

            //Assert
            Assert.Pass("No assertion really necessary here");
        }

        [Test]
        public void Test_PurchaseRemovesProductFromStore()
        {
            //Arrange
            const string TEST_PRODUCT_ID = "2";


            _store =
                createTestStore(
                    createTestUser("Test Guy", "123", 50),
                    createTestProduct(TEST_PRODUCT_ID, "Product", 1.20, 10)
                );
          
            //Act
            _store.Purchase(TEST_PRODUCT_ID, 9);

            //Assert 
            //(choose the appropriate statement(s))
            Assert.AreEqual(1, _store.GetProductById(TEST_PRODUCT_ID).Quantity);
            //Assert.AreSame(1, products[0].Quantity);
            //Assert.IsTrue(products[0].Quantity == 1);
        }

        [Test]
        [ExpectedException(typeof(InsufficientFundsException))]
        public void Test_PurchaseThrowsExceptionWhenBalanceIsTooLow()
        {
            //Arrange
            const string TEST_PRODUCT_ID = "3";

            _store =
                createTestStore(
                    createTestUser("Tester Testington", "abc123", 1.00),
                    createTestProduct(TEST_PRODUCT_ID, "Product", 1.01, 10)
                );

            //Act
            _store.Purchase(TEST_PRODUCT_ID, 1);

        }

        [Test]
        public void Test_PurchaseThrowsExceptionWhenBalanceIsTooLowVersion2()
        {
            //Arrange
            const string TEST_PRODUCT_ID = "4";

            _store =
                createTestStore(
                    createTestUser("Tommy Test", "password", 1.00),
                    createTestProduct(TEST_PRODUCT_ID, "Product", 1.01, 10)
                );

            //Act
            try
            {
                _store.Purchase(TEST_PRODUCT_ID, 1);
                Assert.Fail("InsufficientFundsException is not thrown");
            }

            //Assert
            catch(InsufficientFundsException e)
            {
                Assert.Pass("{0} exception is thrown", e.GetType().Name);
            }                
        }

        [Test]
        [ExpectedException(typeof(OutOfStockException))]
        public void Test_PurchaseThrowsExceptionWhenStoreIsOutOfStock()
        {
            //Arrange
            const string TEST_PRODUCT_ID = "5";

            _store =
                createTestStore(
                    createTestUser("Keith Tremorin", "Test", 5.00),
                    createTestProduct(TEST_PRODUCT_ID, "Product", 1.00, 1)
                );

            //Act
            _store.Purchase(TEST_PRODUCT_ID, 2);
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
