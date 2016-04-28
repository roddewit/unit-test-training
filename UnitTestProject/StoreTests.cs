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
            const string TEST_PRODUCT_ID = "1";

            var products = new List<Product>();
            products.Add(createTestProduct(TEST_PRODUCT_ID, "Product", 9.99, 10));

            var users = new List<User>();
            users.Add(createTestUser("Test User", "", 100));

            var dataManager = new DataManager(originalUsers, products);
            var store = new Store(users[0], dataManager);

            //Act
            store.Purchase(TEST_PRODUCT_ID, 9);

            //Assert 
            //(choose the appropriate statement(s))
            Assert.AreEqual(1, products[0].Quantity);
        }

        [Test]
        public void Test_PurchaseThrowsExceptionWhenBalanceIsTooLow()
        {
            const string TEST_PRODUCT_ID = "1";

            var products = new List<Product>();
            products.Add(createTestProduct(TEST_PRODUCT_ID, "Product", 1.01, 10));

            var users = new List<User>();
            users.Add(createTestUser("Test User", "", 1.00));

            var dataManager = new DataManager(originalUsers, products);
            var store = new Store(users[0], dataManager);

            try
            {
                store.Purchase(TEST_PRODUCT_ID, 1);
                Assert.Fail();
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex is InsufficientFundsException);
            }

        }

        [Test]
        [ExpectedException(typeof(InsufficientFundsException))]
        public void Test_PurchaseThrowsExceptionWhenBalanceIsTooLowVersion2()
        {
            const string TEST_PRODUCT_ID = "1";

            var products = new List<Product>();
            products.Add(createTestProduct(TEST_PRODUCT_ID, "Product", 1.01, 10));

            var users = new List<User>();
            users.Add(createTestUser("Test User", "", 1.00));

            var dataManager = new DataManager(originalUsers, products);
            var store = new Store(users[0], dataManager);

            store.Purchase(TEST_PRODUCT_ID, 1);
        }

        [Test]
        public void Test_PurchaseMoreProductsThanStock()
        {
            const string TEST_PRODUCT_ID = "1";

            var products = new List<Product>();
            products.Add(createTestProduct(TEST_PRODUCT_ID, "Product", 1.00, 10));

            var users = new List<User>();
            users.Add(createTestUser("Test User", "", 100));

            var dataManager = new DataManager(originalUsers, products);
            var store = new Store(users[0], dataManager);

            try
            {
                store.Purchase(TEST_PRODUCT_ID, 11);
                Assert.Fail();
            }
            catch (Exception e)
            {
                Assert.IsTrue(e is OutOfStockException);
            }
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
