using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Refactoring
{
    public class Program
    {
        public static void Main(string[] args)
        {
            IConsole console = new ConsoleImpl();

            // Load users from data file
            List<User> users = JsonConvert.DeserializeObject<List<User>>(File.ReadAllText(@"Data/Users.json"));

            // Load products from data file
            List<Product> products = JsonConvert.DeserializeObject<List<Product>>(File.ReadAllText(@"Data/Products.json"));

            DataManager dataManager = new DataManager(users, products);
            WriteWelcomeToTUSCMessage(console);

            LoginManager loginManager = new LoginManager(console);
            User loggedInUser = loginManager.LogIn(users);
            if (loggedInUser != null)
            {
                Store store = new Store(console, loggedInUser, dataManager);

                Tusc tusc = new Tusc(console, loggedInUser, store);
                tusc.Run();
            }
        }

        private static void WriteWelcomeToTUSCMessage(IConsole console)
        {
            console.WriteLine("Welcome to TUSC");
            console.WriteLine("---------------");
        }
    }
}
