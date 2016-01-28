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
            // Load users from data file
            List<User> users = JsonConvert.DeserializeObject<List<User>>(File.ReadAllText(@"Data\Users.json"));

            // Load products from data file
            List<Product> products = JsonConvert.DeserializeObject<List<Product>>(File.ReadAllText(@"Data\Products.json"));

            WriteWelcomeToTUSCMessage();

            User loggedInUser = LogIn(users);
            Store store = new Store(loggedInUser, products);

            Tusc tusc = new Tusc(loggedInUser, store);
            tusc.Run();
        }

        private static void WriteWelcomeToTUSCMessage()
        {
            // Write welcome message
            Console.WriteLine("Welcome to TUSC");
            Console.WriteLine("---------------");
        }

        private static User LogIn(List<User> users)
        {
            User loggedInUser = null;
            while (loggedInUser == null)
            {
                Authenticator authenticator = new Authenticator(users);
                LoginView loginView = new LoginView(authenticator);

                loggedInUser = loginView.Login();

                WriteLoginMessage(loggedInUser);
            }

            return loggedInUser;
        }

        private static void WriteLoginMessage(User user)
        {
            if (user != null)
            {
                WriteSuccessfulLoginMessage(user);
            }
            else
            {
                WriteInvalidLoginMessage();
            }
        }

        private static void WriteSuccessfulLoginMessage(User user)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine();
            Console.WriteLine(String.Format("Login successful! Welcome {0}!", user.Name));
            Console.ResetColor();
        }

        private static void WriteInvalidLoginMessage()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine();
            Console.WriteLine("You entered an invalid username or password.");
            Console.ResetColor();
        }   

    }
}
