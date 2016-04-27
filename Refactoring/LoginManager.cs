using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Refactoring
{
    public class LoginManager
    {
        public static User LogIn(List<User> users)
        {
            try
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
            catch (EmptyUsernameException)
            {
                WriteInvalidLoginMessage();
                
                Console.WriteLine();
                Console.WriteLine("Press Enter key to exit");
                Console.ReadLine();

                return null;
            }
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
