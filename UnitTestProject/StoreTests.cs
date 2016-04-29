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

        private Store arrangeSingleProductSingleUserTest( 
                                  string productId, string testUserName="Test User", int fund=100, 
                                  string productName="Product", int price=1, int availableToPurchase=10,
                                  List<Product> productList = null )
        {
            var users = new List<User>();
            users.Add( createTestUser( testUserName, "", fund ) );
            var products = (productList==null)? new List<Product>():productList;
            products.Add( createTestProduct( productId, "Product", price, availableToPurchase ) );
            var dataManager = new DataManager(users, products);
            var store = new Store(users[0], dataManager);
            return store;
        }

        [Test]
        public void Test_PurchaseThrowsNoErrorForValidFunds()
        {
            //Arrange
            const string TEST_PRODUCT_ID = "1";
            var store = arrangeSingleProductSingleUserTest(TEST_PRODUCT_ID);
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
            var products = new List<Product>();
            var store = arrangeSingleProductSingleUserTest(
                TEST_PRODUCT_ID, 
                availableToPurchase:10, 
                productList:products);

            //Act
            store.Purchase( TEST_PRODUCT_ID, 9 );

            Assert.IsTrue(products[0].Quantity == 1);
        }

        [Test]
        [ExpectedException( typeof( InsufficientFundsException ) )]
        public void Test_PurchaseThrowsExceptionWhenBalanceIsTooLow()
        {
            //Arrange
            const string TEST_PRODUCT_ID = "1";
            //act
            var store = arrangeSingleProductSingleUserTest(
                TEST_PRODUCT_ID, 
                fund:1, 
                price:10);
            store.Purchase( TEST_PRODUCT_ID, 1 );
        }

        [Test]
        [ExpectedException( typeof( InsufficientFundsException ) )]
        public void Test_PurchaseMultipleItemThrowsExceptionWhenBalanceIsTooLow()
        {
            //Arrange
            const string TEST_PRODUCT_ID = "1";
            const int QUANTITY_TO_PURCHASE = 11;
            const int price = 1;

            var store = arrangeSingleProductSingleUserTest(
                TEST_PRODUCT_ID, price:price, 
                fund:(QUANTITY_TO_PURCHASE-1)*price, 
                availableToPurchase:QUANTITY_TO_PURCHASE );

            //Act
            store.Purchase( TEST_PRODUCT_ID, QUANTITY_TO_PURCHASE );
        }

        [Test]
        [ExpectedException( typeof( OutOfStockException ) )]
        public void Test_PurchaseThrowsExceptionWhenNotEnoughQuantity ()
        {
            //Arrange
            const string TEST_PRODUCT_ID = "1";
            const int QUANTITY_TO_PURCHASE = 11;

            var store = arrangeSingleProductSingleUserTest(
                TEST_PRODUCT_ID, 
                availableToPurchase:QUANTITY_TO_PURCHASE-1);

            //Act
            store.Purchase( TEST_PRODUCT_ID, QUANTITY_TO_PURCHASE );
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
