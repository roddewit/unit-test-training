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
        private const int QUANTITY_OF_PRODUCT = 10;
        private List<Product> products;
        private List<User> users;

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
            users = new List<User>();
            users.Add(createTestUser("Test User", "", userBalance));
            products = new List<Product>();
            products.Add(createTestProduct(TEST_PRODUCT_ID, "Product", priceOfProduct, numberOfProducts));

            var dataManager = new DataManager(users, products);
            return new Store(users[0], dataManager);
        }

        [Test]
        public void Test_PurchaseThrowsNoErrorForValidFunds()
        {
            //Arrange
            var store = setupStore(QUANTITY_OF_PRODUCT, 9.99, 99.99);

            //Act
            store.Purchase(TEST_PRODUCT_ID, 10);

            //Assert
            Assert.Pass("No assertion really necessary here");
        }

        [Test]
        public void Test_PurchaseRemovesProductFromStore()
        {
            //Arrange
            var store = setupStore(QUANTITY_OF_PRODUCT, 9.99, 99.99);

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
            var store = setupStore(QUANTITY_OF_PRODUCT, 1.01, 1.00);

            //Act
            store.Purchase(TEST_PRODUCT_ID, 1);
        }

        [Test]
        public void Test_PurchaseThrowsExceptionWhenBalanceIsTooLowVersion2()
        {
            //Arrange
            var store = setupStore(QUANTITY_OF_PRODUCT, 1.01, 1.00);
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

        [Test]
        [ExpectedException(typeof(OutOfStockException))]
        public void Test_PurchaseThrowsExceptionWhenYouTryToPurcahseToMuch()
        {
            //Arrange
            var store = setupStore(1, 1.00, 5.00);

            //Act
            store.Purchase(TEST_PRODUCT_ID, 2);
        }

        [Test]
        public void Test_GetProductList()
        {
            //Arrange
            var store = setupStore(QUANTITY_OF_PRODUCT, 9.99, 99.99);
            string expectedProductList = "\nWhat would you like to buy?\n1: Product ($9.99)\nType quit to exit the application\n";

            //Act
            string actualProductList = store.GetProductList();

            //Assert
            Assert.AreEqual(expectedProductList, actualProductList);
        }


        [Test]
        public void Test_NumberOfProducts()
        {
            //Arrange
            var store = setupStore(QUANTITY_OF_PRODUCT, 9.99, 99.99);
            int expectedNumberOfProducts = 1;
            //Act
            int actualNumberOfProducts = store.NumberOfProducts();

            //Assert
            Assert.AreEqual(expectedNumberOfProducts, actualNumberOfProducts);
        }


        [Test]
        public void Test_GetNumberOfProducts()
        {
            //Arrange
            var store = setupStore(QUANTITY_OF_PRODUCT, 9.99, 99.99);
            int expectedNumberOfProducts = 1;
            //Act
            int actualNumberOfProducts = store.NumberOfProducts();

            //Assert
            Assert.AreEqual(expectedNumberOfProducts, actualNumberOfProducts);
        }


        [Test]
        public void Test_ContainsProduct_ReturnWithExactMatch()
        {
            //Arrange
            var store = setupStore(QUANTITY_OF_PRODUCT, 9.99, 99.99);
            //Act
            bool expectedProduct = store.ContainsProduct(TEST_PRODUCT_ID);

            //Assert
            Assert.IsTrue(expectedProduct);
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
