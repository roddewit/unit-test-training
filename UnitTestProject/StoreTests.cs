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

        private Store purchaseMethodSetupSingleUserSingleProduct(double balance, string id, double price, int quantity)
        {
            var users = new List<User>();
            users.Add(createTestUser("Test User", "", balance));

            var products = new List<Product>();
            products.Add(createTestProduct(id, "Product", price, quantity));

            var dataManager = new DataManager(users, products);
            return new Store(users[0], dataManager);
            
        }



        [Test]
        public void Test_PurchaseThrowsNoErrorForValidFunds()
        {
            //Arrange
            
            const string TEST_PRODUCT_ID = "1";
            var store = purchaseMethodSetupSingleUserSingleProduct(99.99,TEST_PRODUCT_ID, 9.99, 10);


            //Act
            store.Purchase(TEST_PRODUCT_ID, 10);

            //Assert
            Assert.Pass("No assertion really necessary here");
        }

        [Test]
        public void Test_PurchaseRemovesProductFromStore()
        {
            //Arrange
            const string TEST_PRODUCT_ID = "1";
            var store = purchaseMethodSetupSingleUserSingleProduct(99.99, TEST_PRODUCT_ID, 9.99, 10);

            //Act
            store.Purchase(TEST_PRODUCT_ID, 9);

            //Assert 
            //(choose the appropriate statement(s))
            Assert.AreEqual(1, store.GetProductById(TEST_PRODUCT_ID).Quantity);
            //Assert.AreSame(1, store.GetProductById(TEST_PRODUCT_ID).Quantity);
            Assert.IsTrue(store.GetProductById(TEST_PRODUCT_ID).Quantity == 1);
        }

        [Test]
        public void Test_PurchaseThrowsExceptionWhenBalanceIsTooLow()
        {
            //Arrange
            const string TEST_PRODUCT_ID = "1";
            var store = purchaseMethodSetupSingleUserSingleProduct(1.00, TEST_PRODUCT_ID, 1.01, 10);
            //Act
            try
            {
                store.Purchase(TEST_PRODUCT_ID, 1);
            }
            //Assert
            catch(InsufficientFundsException ife)
            {
                Assert.IsTrue(true);
            }
            
        }

        [Test]
        public void Test_PurchaseThrowsExceptionWhenBalanceIsTooLowVersion2()
        {
            //Arrange
            const string TEST_PRODUCT_ID = "1";
            var store = purchaseMethodSetupSingleUserSingleProduct(1.00, TEST_PRODUCT_ID, 1.01, 10);
            //Act

            //Assert
            Assert.That(() => store.Purchase(TEST_PRODUCT_ID, 1), 
                Throws.Exception
                .TypeOf<InsufficientFundsException>());
        }

        [Test]
        public void Test_PurchaseThrowsExceptionWhenQuantityTooLow()
        {
            //Arrange
            const string TEST_PRODUCT_ID = "1";
            var store = purchaseMethodSetupSingleUserSingleProduct(99.99, TEST_PRODUCT_ID, 9.99, 1);
            //Act

            //Assert
            Assert.That(() => store.Purchase(TEST_PRODUCT_ID, 2),
                Throws.Exception
                .TypeOf<OutOfStockException>());
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
