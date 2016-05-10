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
        private Authenticator authenticator;

        private User createTestUser(string name, string password, double balance)
        {
            User testUser = new User();
            testUser.Name = name;
            testUser.Password = password;
            testUser.Balance = balance;

            return testUser;
        }

        private List<User> CreateTestUserList()
        {
            var users = new List<User>();
            users.Add(createTestUser("Test User", "TestPassWord", 5.01));
            users.Add(createTestUser("Test User2", "TestPassWord2", 0.00));
            return users;
        }

        private Authenticator CreateAuthenticator(List<User> users)
        {  
            return new Authenticator(users);
        }

        [Test]
        public void Test_AuthenticateValidUser()
        {
            string userName = "Test User";
            string userPassword = "TestPassWord";

            User user = authenticator.Authenticate(userName, userPassword);
            Assert.IsNotNull(user);
            Assert.AreEqual(userName, user.Name);
            Assert.AreEqual(userPassword, user.Password);
        }

        [Test]
        public void Test_RejectInvalidUsername()
        {
            string userName = "Test User3";
            string userPassword = "TestPassWord";

            User user = authenticator.Authenticate(userName, userPassword);
            Assert.IsNull(user);
        }

        [Test]
        public void Test_RejectInvalidPassword()
        {
            string userName = "Test User2";
            string userPassword = "TestPassWord";

            User user = authenticator.Authenticate(userName, userPassword);
            Assert.IsNull(user);
        }

        [Test]
        public void Test_HandleNullInputs()
        {
            string userName = "Test User";
            string userPassword = null;

            User user = authenticator.Authenticate(userName, userPassword);
            Assert.IsNull(user);

            userName = null;
            userPassword = null;

            user = authenticator.Authenticate(userName, userPassword);
            Assert.IsNull(user);
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

            var users = CreateTestUserList();
            authenticator = CreateAuthenticator(users);
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
