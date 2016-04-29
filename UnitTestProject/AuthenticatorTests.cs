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
    /// <summary>
    /// Summary description for AuthenticatorTests
    /// </summary>
    [TestFixture]
    public class AuthenticatorTests
    {

        private User createTestUser(string name, string password, double balance)
        {
            User testUser = new User();
            testUser.Name = name;
            testUser.Password = password;
            testUser.Balance = balance;

            return testUser;
        }

        private Authenticator SetUpAuthenticatorAndTestUserList()
        {
            var users = new List<User>();
            users.Add(createTestUser("user1", "abcd", 99.99));
            users.Add(createTestUser("user2", "1234", 9.99));
            return new Authenticator(users);
        }

        [Test]
        public void Test_AuthenticateReturnsUserOnSuccess()
        {
            //Arrange
            var authenticator = SetUpAuthenticatorAndTestUserList();
            //Act
            var returnedValue = authenticator.Authenticate("user1", "abcd");
            //Assert
            Assert.IsInstanceOf<User>(returnedValue);
        }

        [Test]
        public void Test_AuthenticateReturnsNULLOnBadPassword()
        {
            //Arrange
            var authenticator = SetUpAuthenticatorAndTestUserList();
            //Act
            var returnedValue = authenticator.Authenticate("user1", "ab34");
            //Assert
            Assert.IsNull(returnedValue);
        }

        [Test]
        public void Test_AuthenticateReturnsNULLOnBadUsername()
        {
            //Arrange
            var authenticator = SetUpAuthenticatorAndTestUserList();
            //Act
            var returnedValue = authenticator.Authenticate("user3", "abcd");
            //Assert
            Assert.IsNull(returnedValue);
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
