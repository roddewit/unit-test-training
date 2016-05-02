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
        #region Variables
        private const string FirstTestPrdId = "1";
        private const string FirstTestPrdName = "Coke";
        private const double FirstTestPrdPrice = 1.01;
        private const int FirstTestPrdQty = 10;

        private const string FirstUserName = "John";
        private const string FirstUserPassword = "";
        private const double FirstUserBalance = 99.99;


        private const string SecondTestPrdID = "2";
        private const string SecondTestPrdName = "Ice Tea";
        private const double SecondTestPrdPrice = 1.50;
        private const int SecondTestPrdQty = 5;

        User _firstUser = new User();
        Product _firstProduct = new Product();
        Product _secondProduct = new Product();
        #endregion Variables


        #region TestSetUp
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

        private Store setUpFirstUserFirstProductPurchase()
        {   
           
            var users = new List<User>();
            _firstUser = createTestUser(FirstUserName, FirstUserPassword, FirstUserBalance);
            users.Add(_firstUser);

            var products = new List<Product>();
            _firstProduct = createTestProduct(FirstTestPrdId, FirstTestPrdName, FirstTestPrdPrice, FirstTestPrdQty);
            products.Add(_firstProduct);

            var dataManager = new DataManager(users, products);
            var store = new Store(users[0], dataManager);
            return store;

        }

        private Store setUpTwoProducts()
        {
            var users = new List<User>();
            _firstUser = createTestUser(FirstUserName, FirstUserPassword, FirstUserBalance);
            users.Add(_firstUser);

            var products = new List<Product>();
            _firstProduct = createTestProduct(FirstTestPrdId, FirstTestPrdName, FirstTestPrdPrice, FirstTestPrdQty);
            products.Add(_firstProduct);

            _secondProduct = createTestProduct(SecondTestPrdID, SecondTestPrdName, SecondTestPrdPrice, SecondTestPrdQty);
            products.Add(_secondProduct);
            
            var dataManager = new DataManager(users, products);
            var store = new Store(users[0], dataManager);
            return store;
        }

        #endregion TestSetUp


        #region Test
        [Test]
        public void Test_PurchaseThrowsNoErrorForValidFunds()
        {
            //Arrange
            int purchaseValidFunds = 1;
            var store = setUpFirstUserFirstProductPurchase();

            //Act
            store.Purchase(FirstTestPrdId, purchaseValidFunds);

            //Assert
            Assert.Pass("No assertion really necessary here");
        }

        [Test]
        public void Test_PurchaseRemovesProductFromStore()
        {
            //Arrange
            int removeQty = 4;
            int equalCount = 6;
            var store = setUpFirstUserFirstProductPurchase();

            //Act
            store.Purchase(FirstTestPrdId, removeQty);
            
            //Assert 
            //(choose the appropriate statement(s))
            Assert.AreEqual(equalCount, _firstProduct.Quantity);
            //Assert.AreSame(1, products[0].Quantity);
            //Assert.IsTrue(products[0].Quantity == 1);
        }

        [Test]
        [ExpectedException(typeof(InsufficientFundsException))]
        public void Test_PurchaseThrowsExceptionWhenBalanceIsTooLow()
        {
            //Arrange
            int purchaseInsufficientQty = 100;
            var store = setUpFirstUserFirstProductPurchase();
            
            //Act
            store.Purchase(FirstTestPrdId, purchaseInsufficientQty);
          
            //Assert   
        }

        [Test]
        public void Test_PurchaseThrowsExceptionWhenBalanceIsTooLowVersion2()
        {
            //Arrange
            int purchaseInsufficientQty = 100;
            var store = setUpFirstUserFirstProductPurchase();
          
            try
            {
                //Act
                store.Purchase(FirstTestPrdId, purchaseInsufficientQty);

                //Assert
                Assert.Fail();
            }
            catch (Exception e)
            {
                Assert.IsTrue(e is InsufficientFundsException );
            }
        }

        [Test]
        public void Test_PurchaseThrowsExceptionWhenOutOfStock()
        {
            //Arrange
            int purchaseInsufficientQty = 11;
            var store = setUpFirstUserFirstProductPurchase();
          
            try
            {
                //Act
                store.Purchase(FirstTestPrdId, purchaseInsufficientQty);

                //Assert
                Assert.Fail();
            }
            catch (Exception e)
            {
                Assert.IsTrue(e is OutOfStockException);
            }
        }

        [Test]
        public void Test_NullWhenGetProductList()
        {
            //Arrange
            var store = setUpFirstUserFirstProductPurchase();
            
            Assert.NotNull(store.GetProductList());
            
        }

        [Test]
        public void Test_ValidProductCount()
        {
            //Arrange
            int equalCount = 2;

            var store = setUpTwoProducts();

            Assert.AreEqual(equalCount, store.NumberOfProducts());

        }

        [Test]
        public void Test_ContainsProductByProductID()
        {
            //Arrange
            var store = setUpTwoProducts();

            //Act

            //Assert
            Assert.IsTrue(store.ContainsProduct(_firstProduct.Id));

        }

        #endregion Test

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
