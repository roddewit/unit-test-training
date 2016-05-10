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
        const string TEST_PRODUCT_ID = "1";

        private User createTestUser(string name, string password, double balance)
        {
            User testUser = new User();
            testUser.Name = name;
            testUser.Password = password;
            testUser.Balance = balance;

            return testUser;
        }

        private List<User> CreateTestUserList(double credit)
        {
            var users = new List<User>();
            users.Add(createTestUser("Test User", "", credit));
            return users;
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

        private List<Product> CreateTestProductList(string productId, double price, int quantity)
        {
            var products = new List<Product>();
            products.Add(createTestProduct(productId, "Product", price, quantity));
            return products;
        }

        private static Store CreateStore(List<User> users, List<Product> products)
        {
            var dataManager = new DataManager(users, products);
            var store = new Store(users[0], dataManager);
            return store;
        }

        [Test]
        public void Test_PurchaseThrowsNoErrorForValidFunds()
        {
            //Arrange
            var users = CreateTestUserList(99.99);
            var products = CreateTestProductList(TEST_PRODUCT_ID, 9.99, 10);
            var store = CreateStore(users, products);

            //Act
            store.Purchase(TEST_PRODUCT_ID, 10);

            //Assert
            Assert.Pass("No assertion really necessary here");
        }

        [Test]
        public void Test_PurchaseRemovesProductFromStore()
        {
            //Arrange
            var users = CreateTestUserList(99.99);
            var products = CreateTestProductList(TEST_PRODUCT_ID, 9.99, 10);
            var store = CreateStore(users, products);

            //Act
            store.Purchase(TEST_PRODUCT_ID, 9);

            //Assert 
            Assert.AreEqual(1, products[0].Quantity);
        }

        [Test]
        public void Test_PurchaseThrowsExceptionWhenBalanceIsTooLow()
        {
            //Arrange
            var users = CreateTestUserList(1.00);
            var products = CreateTestProductList(TEST_PRODUCT_ID, 1.01, 1);
            var store = CreateStore(users, products);

            //Act
            var ex = Assert.Throws<InsufficientFundsException>(() =>store.Purchase(TEST_PRODUCT_ID, 1));

            //Assert
            Assert.AreEqual(1, products[0].Quantity);
        }

        [Test]
        public void Test_PurchaseThrowsExceptionWhenBalanceIsTooLowVersion2()
        {
            //Arrange
            var users = CreateTestUserList(1.00);
            var products = CreateTestProductList(TEST_PRODUCT_ID, 1.01, 1);
            var store = CreateStore(users, products);

            //Act & Assert
            var ex = Assert.Throws<InsufficientFundsException>(() => store.Purchase(TEST_PRODUCT_ID, 1));        
        }

        [Test]
        public void Test_PurchaseThrowsExceptionWhenOutOfStock()
        {
            //Arrange
            const string TEST_PRODUCT_ID = "1";
            var users = CreateTestUserList(5.00);
            var products = CreateTestProductList(TEST_PRODUCT_ID, 1.01, 1);
            var store = CreateStore(users, products);

            //Act & Assert
            var ex = Assert.Throws<OutOfStockException>(() => store.Purchase(TEST_PRODUCT_ID, 2));
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
