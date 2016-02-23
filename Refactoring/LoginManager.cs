using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Refactoring
{
    public class LoginManager
    {
        private readonly IConsole console;

        public LoginManager(IConsole console)
        {
            this.console = console;
        }

        public User LogIn(List<User> users)
        {
            try
            {
                User loggedInUser = null;
                while (loggedInUser == null)
                {
                    Authenticator authenticator = new Authenticator(users);
                    LoginView loginView = new LoginView(console, authenticator);

                    loggedInUser = loginView.Login();

                    WriteLoginMessage(loggedInUser);
                }

                return loggedInUser;
            }
            catch (EmptyUsernameException)
            {
                WriteInvalidLoginMessage();
                
                // Exit gracefully
                console.WriteLine();
                console.WriteLine("Press Enter key to exit");
                console.ReadLine();

                return null;
            }
        }

        private void WriteLoginMessage(User user)
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

        private void WriteSuccessfulLoginMessage(User user)
        {
            console.Clear();
            console.ForegroundColor = ConsoleColor.Green;
            console.WriteLine();
            console.WriteLine(String.Format("Login successful! Welcome {0}!", user.Name));
            console.ResetColor();
        }

        private void WriteInvalidLoginMessage()
        {
            console.Clear();
            console.ForegroundColor = ConsoleColor.Red;
            console.WriteLine();
            console.WriteLine("You entered an invalid username or password.");
            console.ResetColor();
        }
    }
}
