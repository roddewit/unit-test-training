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
        const string TEST_PRODUCT_ID = "Test Product ID";
        const string TEST_PRODUCT_NAME = "Test Product Name";
        const string TEST_USER_NAME = "Test User";
        const string TEST_USER_PASSWORD = "";

        class SingleProductSingleUserStoreTestHelper
        {
            private List<User> users;
            private List<Product> products;
            private Store store;

            public SingleProductSingleUserStoreTestHelper(double userBalance, double productPrice, int productQuantity)
            {
                users = new List<User>();
                users.Add(new User(TEST_USER_NAME, TEST_USER_PASSWORD, userBalance));

                products = new List<Product>();
                products.Add(new Product(TEST_PRODUCT_ID, TEST_PRODUCT_NAME, productPrice, productQuantity));

                store = new Store(users[0], new DataManager(users, products));
            }

            public void purchase(int quantity)
            {
                store.Purchase(TEST_PRODUCT_ID, quantity);
            }

            public int getProductQuantity()
            {
                return products[0].Quantity;
            }
        }

        [Test]
        public void Test_PurchaseThrowsNoErrorForValidFunds()
        {
            //Arrange
            SingleProductSingleUserStoreTestHelper helper = new SingleProductSingleUserStoreTestHelper(99.9, 9.99, 10);

            //Act
            helper.purchase(10);

            //Assert
            Assert.Pass("No assertion really necessary here");
        }

        [Test]
        public void Test_PurchaseRemovesProductFromStore()
        {
            //Arrange
            SingleProductSingleUserStoreTestHelper helper = new SingleProductSingleUserStoreTestHelper(99.9, 9.99, 10);

            //Act
            helper.purchase(9);

            //Assert 
            Assert.AreEqual(1, helper.getProductQuantity());
        }

        [Test]
        [ExpectedException(typeof(Refactoring.InsufficientFundsException))]
        public void Test_PurchaseThrowsExceptionWhenBalanceIsTooLow()
        {
            //Arrange
            SingleProductSingleUserStoreTestHelper helper = new SingleProductSingleUserStoreTestHelper(1.00, 1.01, 1);

            //Act
            helper.purchase(1);
        }

        [Test]
        public void Test_PurchaseThrowsExceptionWhenBalanceIsTooLowVersion2()
        {
            //Arrange
            SingleProductSingleUserStoreTestHelper helper = new SingleProductSingleUserStoreTestHelper(1.00, 1.01, 1);

            //Assert
            Assert.Throws<Refactoring.InsufficientFundsException>(
                delegate { helper.purchase(1); });
        }

        [Test]
        public void Test_PurchaseThrowsExceptionWhenProductIsOutOfStock()
        {
            //Arrange
            SingleProductSingleUserStoreTestHelper helper = new SingleProductSingleUserStoreTestHelper(100.00, 1.00, 1);

            //Assert
            Assert.Throws<Refactoring.OutOfStockException>(
                delegate { helper.purchase(2); });
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
