using System;

namespace SimpleTextEditor
{
    public class User
    {
        public string UserName { get; }
        private string Password { get; }
        public string UserType { get; }
        private string FirstName { get; }
        private string LastName { get; }
        private DateTime DateOfBirth { get; }

        public User(string userName, string password, string userType, string firstName, string lastName,
            DateTime dateOfBirth)
        {
            UserName = userName;
            Password = password;
            UserType = userType;
            FirstName = firstName;
            LastName = lastName;
            DateOfBirth = dateOfBirth;
        }

        public override string ToString()
        {
            return $"{UserName},{Password},{UserType},{FirstName},{LastName},{DateOfBirth:dd-MM-yyyy}";
        }

        public bool Validate(string userName, string password)
        {
            return userName == UserName && password == Password;
        }
    }
}