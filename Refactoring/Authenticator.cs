using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Refactoring
{
    public class Authenticator
    {
        private readonly List<User> users;

        public Authenticator(List<User> users)
        {
            this.users = users;
        }

        public User Authenticate(string username, string password)
        {
            return FindUserByCredentials(username, password);
        }

        private User FindUserByCredentials(string username, string password)
        {
            if (IsValidUsername(username)) 
            {
                return users.FirstOrDefault(user => user.Name.Equals(username) && (password == null || user.Password.Equals(password)));
            }
            else
            {
                return null;
            }
        }

        private bool IsValidUsername(string username)
        {
            return !string.IsNullOrEmpty(username);
        }
    }
}
