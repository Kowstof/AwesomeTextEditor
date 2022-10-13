using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleTextEditor
{
    internal class UserList
    {
        private List<User> _users;

        public UserList(List<User> users)
        {
            _users = users;
        }

        public void LoadUsers()
        {
            var file = File.ReadAllLines("login.txt");

            foreach (var line in file)
            {
                var accountData = new string[6];
                for (var i = 0; i < line.Length; i++) accountData[i] = line.Split(',')[i];

                var userName = accountData[0];
                var password = accountData[1];
                var type = accountData[2];
                var firstName = accountData[3];
                var lastName = accountData[4];
                var dob = DateTime.ParseExact(accountData[5], "dd-MM-yyyy", null);

                var newUser = new User(userName, password, type, firstName, lastName, dob);
                _users.Add(newUser);
            }
        }

        public void WriteUser(User newUser)
        {
            File.AppendAllText("login.txt", newUser + Environment.NewLine);
        }
    }
}