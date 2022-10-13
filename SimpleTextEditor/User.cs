using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleTextEditor
{
    internal class User
    {
        private string UserName { get; }
        private string Password { get; }
        private string UserType { get; }
        private string FirstName { get; }
        private string LastName { get; }
        DateTime dateOfBirth { get; }

        public User(string userName, string password, string userType, string firstName, string lastName, DateTime dateOfBirth)
        {
            UserName = userName;
            Password = password;
            UserType = userType;
            FirstName = firstName;
            LastName = lastName;
            this.dateOfBirth = dateOfBirth;
        }
    }
}
