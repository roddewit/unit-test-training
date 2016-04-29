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
        private List<User> users;
        private Authenticator authenticator;

        [TestFixtureSetUp]
        public void Initialize_Data()
        {
            users = new List<User>();
            User testUser = new User();
            testUser.Name = "Test User";
            testUser.Password = "password";
            testUser.Balance = 10.00;
            users.Add(testUser);

            authenticator = new Authenticator(users);
        }

        [Test]
        public void Test_AuthenticateReturnsUserWithSuccessfulAuthenticate()
        {
            User returnUser;
            //Act
            returnUser = authenticator.Authenticate("Test User", "password");
            //Assert
            Assert.AreEqual(users[0], returnUser);
        }

        [Test]
        public void Test_AuthenticateReturnsNullWithIncorrectName()
        {
            User returnUser;
            //Act
            returnUser = authenticator.Authenticate("User Test", "password");
            //Assert
            Assert.Null(returnUser);
        }

        [Test]
        public void Test_AuthenticateReturnsNullWithWrongPassword()
        {
            User returnUser;
            //Act
            returnUser = authenticator.Authenticate("Test User", "");
            //Assert
            Assert.Null(returnUser);
        }

        [Test]
        public void Test_AuthenticateReturnsNullWithNullName()
        {
            User returnUser;
            //Act
            returnUser = authenticator.Authenticate(null, "password");
            //Assert
            Assert.Null(returnUser);
        }

        [Test]
        public void Test_AuthenticateReturnsUserWithNullPassword()
        {
            User returnUser;
            //Act
            returnUser = authenticator.Authenticate("Test User", null);
            //Assert
            Assert.AreEqual(users[0], returnUser);
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
