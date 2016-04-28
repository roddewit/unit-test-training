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
    class AuthenticatorTests
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
        public void Test_ValidUserReturned()
        {
            var users = new List<User>();
            users.Add(createTestUser("Test User", "", 99.99));

            Authenticator a = new Authenticator(users);

            var authUser = a.Authenticate("Test User", "");

            Assert.NotNull(authUser);
        }

        [Test]
        public void Test_InvalidUserReturnsNull()
        {
            var users = new List<User>();

            Authenticator a = new Authenticator(users);

            var authUser = a.Authenticate("Test User", "");

            Assert.Null(authUser);
        }

        [Test]
        public void Test_BlankUsernameReturnsNull()
        {
            var users = new List<User>();
            users.Add(createTestUser("Test User", "", 99.99));

            Authenticator a = new Authenticator(users);

            var authUser = a.Authenticate("", "");

            Assert.Null(authUser);
        }

        [Test]
        public void Test_IncorrectPasswordReturnsNull()
        {
            var users = new List<User>();
            users.Add(createTestUser("Test User", "password", 99.99));

            Authenticator a = new Authenticator(users);

            var authUser = a.Authenticate("Test User", "something else");

            Assert.Null(authUser);
        }

        [Test]
        public void Test_PasswordBypassedWithNullAreYouSerious()
        {
            var users = new List<User>();
            users.Add(createTestUser("Test User", "M%A444葉a(*@é", 99.99));

            Authenticator a = new Authenticator(users);

            var authUser = a.Authenticate("Test User", null);

            Assert.NotNull(authUser);
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
