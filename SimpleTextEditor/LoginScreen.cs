using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace SimpleTextEditor
{
    public partial class LoginScreen : Form
    {
        private readonly List<User> _users;
        private readonly UserList _userList;
        public LoginScreen(List<User> users, UserList userList)
        {
            InitializeComponent();
            _users = users;
            _userList = userList;
        }
        // --------------
        // Button Actions
        // --------------

        private void ExitButton_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void LoginButton_Click(object sender, EventArgs e)
        {
            if (_userList.Validate(userNameTextBox.Text, passwordTextBox.Text) != null)
            {
                var editor = new TextEditor();
                editor.Show();
                Hide();
            }
            else
                MessageBox.Show(@"Incorrect username or password!", @"Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Stop);
        }

        private void NewUserButton_Click(object sender, EventArgs e)
        {
            var newUser = new NewUserScreen(_users, _userList);
            newUser.Show();
            Hide();
        }

        private void LoginButtonStatusCheck()
        {
            if (userNameTextBox.Text != "" && passwordTextBox.Text != "")
                LoginButton.Enabled = true;
            else
                LoginButton.Enabled = false;
        }

        private void userNameTextBox_TextChanged(object sender, EventArgs e)
        {
            LoginButtonStatusCheck();
        }

        private void passwordTextBox_TextChanged(object sender, EventArgs e)
        {
            LoginButtonStatusCheck();
        }
    }
}