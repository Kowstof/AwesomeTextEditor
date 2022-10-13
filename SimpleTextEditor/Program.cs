using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace SimpleTextEditor
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            var users = new List<User>();
            var userList = new UserList(users);
            userList.LoadUsers();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new LoginScreen(users, userList));
        }
    }
}