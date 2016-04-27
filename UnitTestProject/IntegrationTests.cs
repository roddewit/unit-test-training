using Newtonsoft.Json;
using NUnit.Framework;
using Refactoring;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;

namespace UnitTestProject
{
    [TestFixture]
    //[Ignore("Disable integration tests")]
    public class IntegrationTests
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
            using (var writer = new StringWriter())
            {
                Console.SetOut(writer);

                using (var reader = new StringReader("Jason\r\nsfa\r\n1\r\n1\r\nquit\r\n\r\n"))
                {
                    Console.SetIn(reader);

                    Program.Main(new string[] { });
                }
            }
        }

        [Test]
        public void Test_TuscDoesNotThrowAnException()
        {
            using (var writer = new StringWriter())
            {
                Console.SetOut(writer);

                using (var reader = new StringReader("Jason\r\nsfa\r\n1\r\n1\r\nquit\r\n\r\n"))
                {
                    Console.SetIn(reader);

                    DataManager dataManager = new DataManager(users, products);

                    //Tusc.Run(users, products);
                    User loggedInUser = LoginManager.LogIn(users);
                    Store store = new Store(loggedInUser, dataManager);

                    Tusc tusc = new Tusc(loggedInUser, store);
                    tusc.Run();
                }
            }
        }
        
        [Test]
        public void Test_InvalidUserIsNotAccepted()
        {
            using (var writer = new StringWriter())
            {
                Console.SetOut(writer);

                using (var reader = new StringReader("Joel\r\n"))
                {
                    Console.SetIn(reader);

                    DataManager dataManager = new DataManager(users, products);

                    User loggedInUser = LoginManager.LogIn(users);
                }

                Assert.IsTrue(writer.ToString().Contains("You entered an invalid username or password."));
            }
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

                    User loggedInUser = LoginManager.LogIn(users);
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

                    User loggedInUser = LoginManager.LogIn(users);
                }

                Assert.IsTrue(writer.ToString().Contains("You entered an invalid username or password."));
            }
        }
        
        [Test]
        public void Test_UserCanCancelPurchase()
        {
            using (var writer = new StringWriter())
            {
                Console.SetOut(writer);

                using (var reader = new StringReader("Jason\r\nsfa\r\n1\r\n0\r\nquit\r\n\r\n"))
                {
                    Console.SetIn(reader);

                    DataManager dataManager = new DataManager(users, products);

                    User loggedInUser = LoginManager.LogIn(users);
                    Store store = new Store(loggedInUser, dataManager);

                    Tusc tusc = new Tusc(loggedInUser, store);
                    tusc.Run();
                }

                Assert.IsTrue(writer.ToString().Contains("Purchase cancelled"));

            }
        }
        
        [Test]
        public void Test_ErrorOccursWhenBalanceLessThanPrice()
        {
            // Update data file
            List<User> tempUsers = DeepCopy<List<User>>(originalUsers);
            tempUsers.Where(u => u.Name == "Jason").Single().Balance = 0.0;

            using (var writer = new StringWriter())
            {
                Console.SetOut(writer);

                using (var reader = new StringReader("Jason\r\nsfa\r\n1\r\n1\r\nquit\r\n\r\n"))
                {
                    Console.SetIn(reader);

                    DataManager dataManager = new DataManager(tempUsers, products);

                    User loggedInUser = LoginManager.LogIn(tempUsers);
                    Store store = new Store(loggedInUser, dataManager);

                    Tusc tusc = new Tusc(loggedInUser, store);
                    tusc.Run();
                }

                Assert.IsTrue(writer.ToString().Contains("You do not have enough money to buy that"));
            }
        }
        
        [Test]
        public void Test_ErrorOccursWhenProductOutOfStock()
        {
            // Update data file
            List<Product> tempProducts = DeepCopy<List<Product>>(originalProducts);
            tempProducts.Where(u => u.Name == "Chips").Single().Quantity = 0;

            using (var writer = new StringWriter())
            {
                Console.SetOut(writer);

                using (var reader = new StringReader("Jason\r\nsfa\r\n1\r\n1\r\nquit\r\n\r\n"))
                {
                    Console.SetIn(reader);

                    DataManager dataManager = new DataManager(users, tempProducts);

                    User loggedInUser = LoginManager.LogIn(users);
                    Store store = new Store(loggedInUser, dataManager);

                    Tusc tusc = new Tusc(loggedInUser, store);
                    tusc.Run();
                }

                Assert.IsTrue(writer.ToString().Contains("is out of stock"));
            }
        }

        [Test]
        public void Test_ProductListContainsExitItem()
        {
            using (var writer = new StringWriter())
            {
                Console.SetOut(writer);

                using (var reader = new StringReader("Jason\r\nsfa\r\n1\r\n1\r\nquit\r\n\r\n"))
                {
                    Console.SetIn(reader);

                    DataManager dataManager = new DataManager(users, products);

                    User loggedInUser = LoginManager.LogIn(users);
                    Store store = new Store(loggedInUser, dataManager);

                    Tusc tusc = new Tusc(loggedInUser, store);
                    tusc.Run();
                }

                Assert.IsTrue(writer.ToString().Contains("Type quit to exit the application"));
            }
        }

        [Test]
        public void Test_UserCanPurchaseProductWhenOnlyOneInStock()
        {
            // Update data file
            List<Product> tempProducts = DeepCopy<List<Product>>(originalProducts);
            tempProducts.Where(u => u.Name == "Chips").Single().Quantity = 1;

            using (var writer = new StringWriter())
            {
                Console.SetOut(writer);

                using (var reader = new StringReader("Jason\r\nsfa\r\n1\r\n1\r\nquit\r\n\r\n"))
                {
                    Console.SetIn(reader);

                    DataManager dataManager = new DataManager(users, products);

                    User loggedInUser = LoginManager.LogIn(users);
                    Store store = new Store(loggedInUser, dataManager);

                    Tusc tusc = new Tusc(loggedInUser, store);
                    tusc.Run();
                }

                Assert.IsTrue(writer.ToString().Contains("You bought 1 Chips"));
            }
        }

        [Test]
        public void Test_UserCanExitByEnteringQuit()
        {
            using (var writer = new StringWriter())
            {
                Console.SetOut(writer);

                using (var reader = new StringReader("Jason\r\nsfa\r\nquit\r\n\r\n"))
                {
                    Console.SetIn(reader);

                    DataManager dataManager = new DataManager(users, products);

                    User loggedInUser = LoginManager.LogIn(users);
                    Store store = new Store(loggedInUser, dataManager);

                    Tusc tusc = new Tusc(loggedInUser, store);
                    tusc.Run();
                }

                Assert.IsTrue(writer.ToString().Contains("Type quit to exit the application"));
                Assert.IsTrue(writer.ToString().Contains("Press Enter key to exit"));
            }
        }

        [Test]
        public void Test_ProductsWithZeroQuantityDoNotAppearInMenu()
        {
            // Update data file
            List<Product> tempProducts = DeepCopy<List<Product>>(originalProducts);
            tempProducts.Where(u => u.Name == "Chips").Single().Quantity = 0;
            
            using (var writer = new StringWriter())
            {
                Console.SetOut(writer);

                using (var reader = new StringReader("Jason\r\nsfa\r\nquit\r\n\r\n"))
                {
                    Console.SetIn(reader);

                    DataManager dataManager = new DataManager(users, tempProducts);

                    User loggedInUser = LoginManager.LogIn(users);
                    Store store = new Store(loggedInUser, dataManager);

                    Tusc tusc = new Tusc(loggedInUser, store);
                    tusc.Run();
                }

                Assert.IsFalse(writer.ToString().Contains(": Chips"));
            }
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
