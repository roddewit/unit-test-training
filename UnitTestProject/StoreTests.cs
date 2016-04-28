using System.Runtime.Remoting;
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
        private const string DEFAULT_PRODUCT_ID = "1";
        private const double DEFAULT_PRICE = 1.00;
        private const int DEFAULT_QUANTITY = 1;

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

        private List<User> CreateUserWithBalance(string userName, double balance)
        {
            var users = new List<User>();
            users.Add(createTestUser(userName, "", balance));

            return users;
        }

        private List<Product> CreateDefaultProduct()
        {
            return CreateDefaultProduct(DEFAULT_PRICE, DEFAULT_QUANTITY);
        }

        private List<Product> CreateDefaultProduct(double price, int quantity)
        {
            var products = new List<Product>();
            products.Add(createTestProduct(DEFAULT_PRODUCT_ID, "Product", price, quantity));

            return products;
        }

        [Test]
        public void Test_PurchaseThrowsNoErrorForValidFunds()
        {
            //Arrange
            var users = CreateUserWithBalance("Test User", 99.99);
            var products = CreateDefaultProduct(9.99, 10);

            var dataManager = new DataManager(users, products);
            var store = new Store(users[0], dataManager);

            //Act
            store.Purchase(DEFAULT_PRODUCT_ID, 10);

            //Assert
            Assert.Pass("No assertion really necessary here");
        }

        [Test]
        public void Test_PurchaseRemovesProductFromStore()
        {
            //Arrange
            var users = CreateUserWithBalance("Test User", 9999.99);
            var products = CreateDefaultProduct(1.00, 10);

            var dataManager = new DataManager(users, products);
            var store = new Store(users[0], dataManager);


            //Act
            store.Purchase(DEFAULT_PRODUCT_ID, 9);

            //Assert 
            //(choose the appropriate statement(s))
            Assert.AreEqual(1, products[0].Quantity);
            //Assert.AreSame(1, products[0].Quantity);
            Assert.IsTrue(products[0].Quantity == 1);
        }

        [Test]
        public void Test_PurchaseThrowsExceptionWhenBalanceIsTooLow()
        {
            //Arrange
            var users = CreateUserWithBalance("Test User", 1.00);
            var products = CreateDefaultProduct(1.01, 10);

            var dataManager = new DataManager(users, products);
            var store = new Store(users[0], dataManager);
            
            //Act & Assert
            try
            {
                store.Purchase(DEFAULT_PRODUCT_ID, 1);
                Assert.Fail();
            }
            catch (Exception e)
            {
                Assert.IsTrue(e is InsufficientFundsException);
            }
        }

        [Test]
        [ExpectedException(typeof(InsufficientFundsException))]
        public void Test_PurchaseThrowsExceptionWhenBalanceIsTooLowVersion2()
        {
            //Arrange
            var users = CreateUserWithBalance("Test User", 1.00);
            var products = CreateDefaultProduct(1.01, 10);

            var dataManager = new DataManager(users, products);
            var store = new Store(users[0], dataManager);

            //Act
            store.Purchase(DEFAULT_PRODUCT_ID, 1);

            //Assert
            Assert.Fail("Expected InsufficientFundsException");
        }

        [Test]
        [ExpectedException(typeof(OutOfStockException))]
        public void Test_PurchaseThrowsExceptionWhenItemOutOfStock()
        {
            //Arrange
            var users = CreateUserWithBalance("Test User", 100.00);
            var products = CreateDefaultProduct();

            var dataManager = new DataManager(users, products);
            var store = new Store(users[0], dataManager);

            //Act
            store.Purchase(DEFAULT_PRODUCT_ID, 2);

            //Assert
            Assert.Fail("Expected OutOfStockException");
        }

        [Test]
        public void Test_GetProductList()
        {
            //Arrange
            string actualResult = "";
            var users = CreateUserWithBalance("Test User", 100.00);
            var products = CreateDefaultProduct();

            var dataManager = new DataManager(users, products);
            var store = new Store(users[0], dataManager);

            //Act
            actualResult = store.GetProductList();

            //Assert
            Assert.IsTrue(actualResult.Contains(products[0].Name));
        }

        [Test]
        public void Test_GetProductList_NoProducts()
        {
            //Arrange
            var users = CreateUserWithBalance("Test User", 100.00);
            var products = new List<Product>();

            var dataManager = new DataManager(users, products);
            var store = new Store(users[0], dataManager);

            //Act
            store.GetProductList();

            //Assert
            Assert.Pass("GetProductList() successfully handled an empty list.");
        }

        [Test]
        public void Test_NumberOfProducts()
        {
            //Arrange
            const string TEST_PRODUCT_ID = "1";
            const int NUM_OF_PRODUCTS = 4;
            int actualNumberOfProducts;

            var users = CreateUserWithBalance("Test User", 100.00);

            var products = new List<Product>();
            for (int i=0; i<NUM_OF_PRODUCTS; i++)
                products.Add(createTestProduct(TEST_PRODUCT_ID+i, String.Format("Product({0})",i+1), 1.00, 1));

            var dataManager = new DataManager(users, products);
            var store = new Store(users[0], dataManager);

            //Act
            actualNumberOfProducts = store.NumberOfProducts();

            //Assert
            Assert.AreEqual(NUM_OF_PRODUCTS, actualNumberOfProducts);
        }

        [Test]
        public void Test_NumberOfProducts_NoProducts()
        {
            //Arrange
            int actualNumberOfProducts;
            var users = CreateUserWithBalance("Test User", 100.00);
            var products = new List<Product>();

            var dataManager = new DataManager(users, products);
            var store = new Store(users[0], dataManager);

            //Act
            actualNumberOfProducts = store.NumberOfProducts();

            //Assert
            Assert.AreEqual(0, actualNumberOfProducts);
        }

        [Test]
        public void Test_ContainsProduct_ContainsProduct()
        {
            //Arrange
            bool actualResult;
            bool expectedResult = true;
            var users = CreateUserWithBalance("Test User", 100.00);
            var products = CreateDefaultProduct();

            var dataManager = new DataManager(users, products);
            var store = new Store(users[0], dataManager);

            //Act
            actualResult = store.ContainsProduct(DEFAULT_PRODUCT_ID);

            //Assert
            Assert.AreEqual(expectedResult, actualResult);
        }

        [Test]
        public void Test_ContainsProduct_DoesNotContainsProduct()
        {
            //Arrange
            bool actualResult;
            bool expectedResult = false;
            const string NONEXISTENT_PRODUCT_ID = "2";
            var users = CreateUserWithBalance("Test User", 100.00);
            var products = CreateDefaultProduct();

            var dataManager = new DataManager(users, products);
            var store = new Store(users[0], dataManager);

            //Act
            actualResult = store.ContainsProduct(NONEXISTENT_PRODUCT_ID);

            //Assert
            Assert.AreEqual(expectedResult, actualResult);
        }

        [Test]
        public void Test_ContainsProduct_NoProductsInStore()
        {
            //Arrange
            bool actualResult;
            bool expectedResult = false;
            var users = CreateUserWithBalance("Test User", 100.00);
            var products = new List<Product>();

            var dataManager = new DataManager(users, products);
            var store = new Store(users[0], dataManager);

            //Act
            actualResult = store.ContainsProduct(DEFAULT_PRODUCT_ID);

            //Assert
            Assert.AreEqual(expectedResult, actualResult);
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
