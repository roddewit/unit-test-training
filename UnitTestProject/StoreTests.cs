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
            const string TEST_PRODUCT_ID = "1";

            List<Product> products;
            Store store;
            ArrangeStoreForProductTest(TEST_PRODUCT_ID, out products, out store);

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

            List<Product> products;
            Store store;
            ArrangeStoreForProductTest(TEST_PRODUCT_ID, out products, out store);

            //Act
            store.Purchase(TEST_PRODUCT_ID, 9);
       
            //Assert 
            //(choose the appropriate statement(s))
            Assert.AreEqual(1, products[0].Quantity);
            //Assert.AreSame(1, products[0].Quantity);
            Assert.IsTrue(products[0].Quantity == 1);
        }

        private void ArrangeStoreForProductTest(string TEST_PRODUCT_ID, out List<Product> products, out Store store)
        {
            var users = new List<User>();
            users.Add(createTestUser("Test User", "", 99.99));

            products = new List<Product>();
            products.Add(createTestProduct(TEST_PRODUCT_ID, "Product", 9.99, 10));

            var dataManager = new DataManager(users, products);
            store = new Store(users[0], dataManager);
        }

        [Test]
        public void Test_PurchaseThrowsExceptionWhenBalanceIsTooLow()
        {         
           
            //Arrange
            const string TEST_PRODUCT_ID = "1";

            var store = ArrangeStoreForBalanceTest(TEST_PRODUCT_ID);


            try
            {     
            //Act
            store.Purchase(TEST_PRODUCT_ID, 1);
            Assert.Fail();
            }
            catch (Exception e)
            {
                Assert.AreEqual("Exception of type 'Refactoring.InsufficientFundsException' was thrown.", e.Message);
            }

        }

        private Store ArrangeStoreForBalanceTest(string TEST_PRODUCT_ID)
        {
            double userBalance = 1.00;
            double productPrice = 1.01;
            int amountofProducts = 1;

            var store = setupStore(TEST_PRODUCT_ID, userBalance, productPrice, amountofProducts);
            return store;
        }

        private Store setupStore(string TEST_PRODUCT_ID, double userBalance, double productPrice, int amountofProducts)
        {
            var users = new List<User>();
            users.Add(createTestUser("Test User", "", userBalance));

            var products = new List<Product>();
            products.Add(createTestProduct(TEST_PRODUCT_ID, "Product", productPrice, amountofProducts));

            var dataManager = new DataManager(users, products);
            var store = new Store(users[0], dataManager);
            return store;
        }


        [Test]
        public void Test_PurchaseThrowsExceptionWhenBalanceIsTooLowVersion2()
        {
            //Arrange
            const string TEST_PRODUCT_ID = "1";

            var store = ArrangeStoreForBalanceTest(TEST_PRODUCT_ID);
            
            //Act
            Assert.Throws<InsufficientFundsException>(() => store.Purchase(TEST_PRODUCT_ID, 1));
           
        }


        [Test]
        public void Test_CoveragePurchase()
        {
            //Arrange
            const string TEST_PRODUCT_ID = "1";

            List<Product> products;
            Store store;
            ArrangeStoreForProductTest(TEST_PRODUCT_ID, out products, out store);

            //Act
            store.Purchase(TEST_PRODUCT_ID, 9);

            //Assert 
            //(choose the appropriate statement(s))
            Assert.AreEqual(1, store.NumberOfProducts());
            //Assert.AreSame(1, products[0].Quantity);
            Assert.IsTrue(store.NumberOfProducts() == 1);
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
