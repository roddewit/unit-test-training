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
    class AuthenticatorTest
        {

        private User createTestUser(string name, string password, double balance)
            {
            User testUser = new User();
            testUser.Name = name;
            testUser.Password = password;
            testUser.Balance = balance;

            return testUser;
            }

        [Test]
        public void Test_CanAuthenicateWithValidUserAndPassword()
            {
            //Arrange
            var users = ArrangeThreeUsers();

            var authenticator = new Authenticator(users);

            //Act
            var user = authenticator.Authenticate("Larry", "nuck");

            //
            Assert.IsNotNull(user);

            }

        [Test]
        public void Test_CanAuthenicateWithValidUserAndPassword2()
            {
            //Arrange
            var users = ArrangeThreeUsers();

            var authenticator = new Authenticator(users);

            //Act
            var user = authenticator.Authenticate("Curly", "Flowbee");

            //
            Assert.IsNotNull(user);

            }

        [Test]
        public void Test_CanAuthenicateWithWrongCasingOnPassword()
            {
            //Arrange
            var users = ArrangeThreeUsers();

            var authenticator = new Authenticator(users);

            //Act
            var user = authenticator.Authenticate("Curly", "flowbee");

            //
            Assert.IsNull(user);

            }

        [Test]
        public void Test_CannotAuthenicateWithInvalidPassword()
            {
            //Arrange
            var users = ArrangeThreeUsers();

            var authenticator = new Authenticator(users);

            //Act
            var user = authenticator.Authenticate("Larry", "muck");

            //
            Assert.IsNull(user);

            }

        [Test]
        public void Test_CannotAuthenicateWithInvalidUser()
            {
            //Arrange
            var users = ArrangeThreeUsers();

            var authenticator = new Authenticator(users);

            //Act
            var user = authenticator.Authenticate("Garry", "nuck");

            //
            Assert.IsNull(user);

            }

        [Test]
        public void Test_CannotAuthenicateNullUser()
            {
            //Arrange
            var users = ArrangeThreeUsers();

            var authenticator = new Authenticator(users);

            //Act
            var user = authenticator.Authenticate(null, "nuck");

            //
            Assert.IsNull(user);

            }

        [Test]
        public void Test_CannotAuthenicateEmptyStringUser()
            {
            //Arrange
            var users = ArrangeThreeUsers();

            var authenticator = new Authenticator(users);

            //Act
            var user = authenticator.Authenticate("", "nuck");

            //
            Assert.IsNull(user);

            }

        private List<User> ArrangeThreeUsers()
            {
            var users = new List<User>();
            users.Add(createTestUser("Larry", "nuck", 10.00));
            users.Add(createTestUser("Curly", "Flowbee", 10.00));
            users.Add(createTestUser("Moe", "zoink", 10.00));
            return users;
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
