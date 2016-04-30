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
        private const int NUMBER_OF_PRODUCTS = 10;
        private List<Product> products = new List<Product>();
        private List<User> users = new List<User>();

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

        private Store setupStore(int numberOfProducts, double priceOfProduct, double userBalance)
        {
            users.Add(createTestUser("Test User", "", userBalance));
            products.Add(createTestProduct(TEST_PRODUCT_ID, "Product", priceOfProduct, numberOfProducts));

            var dataManager = new DataManager(users, products);
            return new Store(users[0], dataManager);
        }

        [Test]
        public void Test_PurchaseThrowsNoErrorForValidFunds()
        {
            //Arrange
            var store = setupStore(NUMBER_OF_PRODUCTS, 9.99, 99.99);

            //Act
            store.Purchase(TEST_PRODUCT_ID, 10);

            //Assert
            Assert.Pass("No assertion really necessary here");
        }

        [Test]
        public void Test_PurchaseRemovesProductFromStore()
        {
            //Arrange
            var store = setupStore(NUMBER_OF_PRODUCTS, 9.99, 99.99);

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
            var store = setupStore(NUMBER_OF_PRODUCTS, 1.01, 1.00);

            //Act
            store.Purchase(TEST_PRODUCT_ID, 1);
        }

        [Test]
        public void Test_PurchaseThrowsExceptionWhenBalanceIsTooLowVersion2()
        {
            //Arrange
            var store = setupStore(NUMBER_OF_PRODUCTS, 1.01, 1.00);
            //Act
            bool exception = false;
            //Assert
            try
            {
                store.Purchase(TEST_PRODUCT_ID, 1);
                Assert.Fail();
            }
            catch (InsufficientFundsException)
            {
                exception = true;
            }
            catch (Exception)
            {
                Assert.Fail();
            }
            Assert.IsTrue(exception);
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
