﻿using Newtonsoft.Json;
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
            var testUser = new User {Name = name, Password = password, Balance = balance};

            return testUser;
        }

        private Product createTestProduct(string id, string name, double price, int quantity)
        {
            var testProduct = new Product {Id = id, Name = name, Price = price, Quantity = quantity};

            return testProduct;
        }

        [Test]
        public void Test_PurchaseThrowsNoErrorForValidFunds()
        {
            //Arrange
            const string TEST_PRODUCT_ID = "1";

            var users = new List<User>();
            users.Add(createTestUser("Test User", "", 99.99));

            var products = new List<Product>();
            products.Add(createTestProduct(TEST_PRODUCT_ID, "Product", 9.99, 10));

            var dataManager = new DataManager(users, products);
            var store = new Store(users[0], dataManager);

            //Act
            store.Purchase(TEST_PRODUCT_ID, 10);

            //Assert
            Assert.Pass("No assertion really necessary here");
        }

        [Test]
        public void Test_PurchaseRemovesProductFromStore()
        {
            //Arrange
            const string TEST_PRODUCT_ID = "2";

            var users = new List<User> {createTestUser("Unit Test", "", 100.00)};
            var products = new List<Product> {createTestProduct(TEST_PRODUCT_ID, "MyProduct", 5.00, 10)};
            var dataManager = new DataManager(users, products);
            var store = new Store(users.ToList().First(), dataManager);
            
            //Act
            store.Purchase(TEST_PRODUCT_ID,9);

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
            const string TEST_PRODUCT_ID = "2";
            const string INSUFFICIENT_FUND_EXCEPTION =
                "Exception of type 'Refactoring.InsufficientFundsException' was thrown.";

            var users = new List<User> { createTestUser("Unit Test", "", 1.00) };
            var products = new List<Product> { createTestProduct(TEST_PRODUCT_ID, "MyProduct", 5.00, 10) };
            var dataManager = new DataManager(users, products);
            var store = new Store(users.ToList().First(), dataManager);

            try
            {
                //Act
                store.Purchase(TEST_PRODUCT_ID, 9);
            }
            catch (Exception e)
            {
                Assert.AreEqual(INSUFFICIENT_FUND_EXCEPTION, e.Message);
            }

        }

        [Test]
        public void Test_PurchaseThrowsExceptionWhenBalanceIsTooLowVersion2()
        {
            //Arrange
            const string TEST_PRODUCT_ID = "2";

            var users = new List<User> { createTestUser("Unit Test", "", 1.00) };
            var products = new List<Product> { createTestProduct(TEST_PRODUCT_ID, "MyProduct", 5.00, 1) };
            var dataManager = new DataManager(users, products);
            var store = new Store(users.ToList().First(), dataManager);

            //Act
            Assert.Throws<InsufficientFundsException>(() => store.Purchase(TEST_PRODUCT_ID, 9));
        }


        [Test]
        public void Test_ShouldThrowOutOfStockException()
        {
            //Arrange
            Store store;
            var TEST_PRODUCT_ID = InitialSetup(out store);            
            //Act
            Assert.Throws<OutOfStockException>(() => store.Purchase(TEST_PRODUCT_ID, 3));

        }

        [Test]
        public void Test_ShouldReturnProductList()
        {
            //Arrange
            Store store;
            var TEST_PRODUCT_ID = InitialSetup(out store);            
            //Act
            Assert.IsNotNullOrEmpty(store.GetProductList());
        }

        [Test]
        public void Test_ShouldReturnNumberOfProducts()
        {
            //Arrange
            Store store;
            var TEST_PRODUCT_ID = InitialSetup(out store);
            //Act
            Assert.IsNotNullOrEmpty(store.NumberOfProducts().ToString());
        }

        [Test]
        public void Test_ShouldContainsProduct()
        {
            Store store;
            var TEST_PRODUCT_ID = InitialSetup(out store);

            //Act
            Assert.IsTrue(store.ContainsProduct(TEST_PRODUCT_ID));
        }

        private string InitialSetup(out Store store)
        {
            //Arrange
            const string TEST_PRODUCT_ID = "2";

            var users = new List<User> {createTestUser("Unit Test", "", 100.00)};
            var products = new List<Product> {createTestProduct(TEST_PRODUCT_ID, "MyProduct", 5.00, 1)};
            var dataManager = new DataManager(users, products);
            store = new Store(users.ToList().First(), dataManager);
            return TEST_PRODUCT_ID;
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
