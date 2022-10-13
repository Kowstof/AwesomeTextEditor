using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SimpleTextEditor
{
    public partial class NewUserScreen : Form
    {
        private readonly List<User> _users;
        private readonly UserList _userList;

        private bool _uFilled, _pFilled, _p2Filled, _fFilled, _lFilled = false;
        public NewUserScreen(List<User> users, UserList userList)
        {
            _users = users;
            _userList = userList;
            InitializeComponent();
        }

        private void NewUserScreen_Load(object sender, EventArgs e)
        {
            usernameWarningLabel.Hide();
            passwordWarningLabel.Hide();
            dobWarningLabel.Hide();
            userTypeComboBox.SelectedIndex = 0;
        }

        private void submitButton_Click(object sender, EventArgs e)
        {
        }

        private void backButton_Click(object sender, EventArgs e)
        {
            Close();
            var login = new LoginScreen(_users, _userList);
            login.Show();
        }

        private void usernameTextBox_TextChanged(object sender, EventArgs e)
        {
            if (_userList.IsUsernameUnique(usernameTextBox.Text)) usernameWarningLabel.Hide();
            else usernameWarningLabel.Show();
            CheckForm();
        }

        private void FirstNameTextBox_TextChanged(object sender, EventArgs e)
        {
            CheckForm();
        }

        private void LastNameTextBox_TextChanged(object sender, EventArgs e)
        {
            CheckForm();
        }

        private void passwordTextBox_TextChanged(object sender, EventArgs e)
        {
            ComparePasswords();
        }

        private void passwordTextBox2_TextChanged(object sender, EventArgs e)
        {
            ComparePasswords();
        }



        private void ComparePasswords()
        {
            CheckForm();
            if (_pFilled && _p2Filled && passwordTextBox2.Text != passwordTextBox.Text)
                passwordWarningLabel.Show();
            else passwordWarningLabel.Hide();
        }

        private void CheckForm()
        {
            _uFilled = usernameTextBox.Text.Length != 0;
            _pFilled = passwordTextBox.Text.Length != 0;
            _p2Filled = passwordTextBox2.Text.Length != 0;
            _fFilled = FirstNameTextBox.Text.Length != 0;
            _lFilled = LastNameTextBox.Text.Length != 0;

            if (_uFilled && _pFilled && _p2Filled && _fFilled && _lFilled && !usernameWarningLabel.Visible && !passwordWarningLabel.Visible && !dobWarningLabel.Visible)
                submitButton.Enabled = true;
            else submitButton.Enabled = false;
        }
    }
}