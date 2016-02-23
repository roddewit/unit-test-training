using Newtonsoft.Json;
using NUnit.Framework;
using Refactoring;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using Moq;

namespace UnitTestProject
{
    [TestFixture]
    public class UnitTests
    {
        private List<User> users;
        private List<User> originalUsers;
        private List<Product> products;
        private List<Product> originalProducts;

        [SetUp]
        public void Test_Initialize()
        {
            // Load users from data file
            originalUsers = JsonConvert.DeserializeObject<List<User>>(File.ReadAllText(@"Data/Users.json"));
            users = DeepCopy<List<User>>(originalUsers);

            // Load products from data file
            originalProducts = JsonConvert.DeserializeObject<List<Product>>(File.ReadAllText(@"Data/Products.json"));
            products = DeepCopy<List<Product>>(originalProducts);
        }

        [TearDown]
        public void Test_Cleanup()
        {
            // Restore users
            string json = JsonConvert.SerializeObject(originalUsers, Formatting.Indented);
            File.WriteAllText(@"Data/Users.json", json);
            users = DeepCopy<List<User>>(originalUsers);

            // Restore products
            string json2 = JsonConvert.SerializeObject(originalProducts, Formatting.Indented);
            File.WriteAllText(@"Data/Products.json", json2);
            products = DeepCopy<List<Product>>(originalProducts);
        }

        [Test]
        public void Test_StartingTuscFromMainDoesNotThrowAnException()
        {
            var idx = 0;
            string[] returnValues = { "Jason", "sfa", "1", "1", "8", String.Empty };

            var mockConsole = new Mock<IConsole>();
            mockConsole.Setup(c => c.ReadLine())
                .Returns(() => returnValues[idx++]);

            using (var writer = new StringWriter())
            {
                Console.SetOut(writer);

                using (var reader = new StringReader("Jason\r\nsfa\r\n1\r\n1\r\n8\r\n\r\n"))
                {
                    Console.SetIn(reader);

                    Program.Main(new string[] { });
                }
            }
        }

        [Test]
        public void Test_TuscDoesNotThrowAnException()
        {
            var idx = 0;
            string[] returnValues = { "Jason", "sfa", "1", "1", "8", String.Empty };

            var mockConsole = new Mock<IConsole>();
            mockConsole.Setup(c => c.ReadLine())
                .Returns(() => returnValues[idx++]);

            DataManager dataManager = new DataManager(users, products);

            LoginManager loginManager = new LoginManager(mockConsole.Object);
            User loggedInUser = loginManager.LogIn(users);
            Store store = new Store(mockConsole.Object, loggedInUser, dataManager);

            Tusc tusc = new Tusc(mockConsole.Object, loggedInUser, store);
            tusc.Run();
        }
        
        [Test]
        public void Test_InvalidUserIsNotAccepted()
        {
            var idx = 0;
            string[] returnValues = { "invalid_user", "invalid_pass", "Jason", "sfa" };

            var mockConsole = new Mock<IConsole>();
            mockConsole.Setup(c => c.ReadLine())
                .Returns(() => returnValues[idx++]);

            DataManager dataManager = new DataManager(users, products);

            LoginManager loginManager = new LoginManager(mockConsole.Object);
            User loggedInUser = loginManager.LogIn(users);

            mockConsole.Verify(c => c.WriteLine("You entered an invalid username or password."));
        }

        [Test]
        public void Test_EmptyUserDoesNotThrowAnException()
        {
            using (var writer = new StringWriter())
            {
                Console.SetOut(writer);

                using (var reader = new StringReader("\r\n\r\n"))
                {
                    Console.SetIn(reader);

                    DataManager dataManager = new DataManager(users, products);

                    LoginManager loginManager = new LoginManager(new ConsoleImpl());
                    User loggedInUser = loginManager.LogIn(users);
                }
            }
        }
        
        [Test]
        public void Test_InvalidPasswordIsNotAccepted()
        {
            using (var writer = new StringWriter())
            {
                Console.SetOut(writer);

                using (var reader = new StringReader("Jason\r\nsfb\r\n"))
                {
                    Console.SetIn(reader);

                    DataManager dataManager = new DataManager(users, products);

                    LoginManager loginManager = new LoginManager(new ConsoleImpl());
                    User loggedInUser = loginManager.LogIn(users);
                }

                Assert.IsTrue(writer.ToString().Contains("You entered an invalid username or password."));
            }
        }

        [Test]
        public void Test_UserCanCancelPurchase()
        {
            var idx = 0;
            string[] returnValues = { "Jason", "sfa", "1", "0", "8", String.Empty };

            var mockConsole = new Mock<IConsole>();
            mockConsole.Setup(c => c.ReadLine())
                .Returns(() => returnValues[idx++]);

            DataManager dataManager = new DataManager(users, products);

            LoginManager loginManager = new LoginManager(mockConsole.Object);
            User loggedInUser = loginManager.LogIn(users);
            Store store = new Store(mockConsole.Object, loggedInUser, dataManager);

            Tusc tusc = new Tusc(mockConsole.Object, loggedInUser, store);
            tusc.Run();

            mockConsole.Verify(c => c.WriteLine("Purchase cancelled"));
        }

        [Test]
        public void Test_ErrorOccursWhenBalanceLessThanPrice()
        {
            var idx = 0;
            string[] returnValues = { "Jason", "sfa", "1", "1", "8", String.Empty };

            var mockConsole = new Mock<IConsole>();
            mockConsole.Setup(c => c.ReadLine())
                .Returns(() => returnValues[idx++]);

            // Update data file
            List<User> tempUsers = DeepCopy<List<User>>(originalUsers);
            tempUsers.Where(u => u.Name == "Jason").Single().Balance = 0.0;

            DataManager dataManager = new DataManager(tempUsers, products);

            LoginManager loginManager = new LoginManager(mockConsole.Object);
            User loggedInUser = loginManager.LogIn(tempUsers);
            Store store = new Store(mockConsole.Object, loggedInUser, dataManager);

            Tusc tusc = new Tusc(mockConsole.Object, loggedInUser, store);
            tusc.Run();

            mockConsole.Verify(c => c.WriteLine("You do not have enough money to buy that."));
        }

        [Test]
        public void Test_ErrorOccursWhenProductOutOfStock()
        {
            var mockConsole = new Mock<IConsole>();

            // TODO: Complete test to work

            mockConsole.Verify(c => c.WriteLine("Sorry, Chips is out of stock"));
        }

        private static T DeepCopy<T>(T obj)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, obj);
                stream.Position = 0;

                return (T)formatter.Deserialize(stream);
            }
        }
    }
}
