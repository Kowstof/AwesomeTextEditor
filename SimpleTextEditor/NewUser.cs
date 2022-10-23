using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace SimpleTextEditor
{
    public partial class NewUserScreen : Form
    {
        private readonly List<User> _users;
        private readonly UserList _userList;
        private readonly LoginScreen _loginForm;

        private bool _uFilled, _pFilled, _p2Filled, _fFilled, _lFilled;

        public NewUserScreen(List<User> users, UserList userList, LoginScreen loginForm)
        {
            _users = users;
            _userList = userList;
            _loginForm = loginForm;
            InitializeComponent();
        }

        // -------------
        // Load Defaults
        // -------------

        private void NewUserScreen_Load(object sender, EventArgs e)
        {
            usernameWarningLabel.Hide();
            passwordWarningLabel.Hide();
            dobWarningLabel.Hide();
            userTypeComboBox.SelectedIndex = 0;
            MaximizeBox = false;
            dobDatePicker.Value = DateTime.Today;
        }

        // -------
        // Buttons
        // -------

        private void submitButton_Click(object sender, EventArgs e)
        {
            var username = usernameTextBox.Text;
            var password = passwordTextBox.Text;
            var firstName = FirstNameTextBox.Text;
            var lastName = LastNameTextBox.Text;
            var dobRaw = dobDatePicker.Value.ToString("dd-MM-yyyy");
            var dob = DateTime.ParseExact(dobRaw, "dd-MM-yyyy", null);
            var type = userTypeComboBox.GetItemText(userTypeComboBox.SelectedItem);

            var newUser = new User(username, password, type, firstName, lastName, dob);
            _users.Add(newUser);
            _userList.WriteUser(newUser);

            MessageBox.Show(
                $"{firstName} {lastName} has been added to the user list. Please login using your set credentials",
                "User Successfully Created", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Close();
            _loginForm.Show();
        }

        private void backButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void NewUser_FormClosing(object sender, FormClosingEventArgs e)
        {
            _loginForm.Clear();
            _loginForm.Show();
        }

        // ---------
        // Live Form
        // ---------

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
            CheckForm();
        }

        private void dobDatePicker_ValueChanged(object sender, EventArgs e)
        {
            CheckDate();
            CheckForm();
        }

        private void passwordTextBox2_TextChanged(object sender, EventArgs e)
        {
            ComparePasswords();
            CheckForm();
        }

        private void ComparePasswords()
        {
            if (_pFilled && _p2Filled && passwordTextBox2.Text != passwordTextBox.Text)
                passwordWarningLabel.Show();
            else passwordWarningLabel.Hide();
        }

        private void CheckDate()
        {
            var enteredDate = dobDatePicker.Value;
            if (enteredDate > DateTime.Today)
                dobWarningLabel.Show();
            else dobWarningLabel.Hide();
        }

        private void CheckForm()
        {
            _uFilled = usernameTextBox.Text.Length != 0;
            _pFilled = passwordTextBox.Text.Length != 0;
            _p2Filled = passwordTextBox2.Text.Length != 0;
            _fFilled = FirstNameTextBox.Text.Length != 0;
            _lFilled = LastNameTextBox.Text.Length != 0;

            if (_uFilled && _pFilled && _p2Filled && _fFilled && _lFilled && !usernameWarningLabel.Visible &&
                !passwordWarningLabel.Visible && !dobWarningLabel.Visible)
                submitButton.Enabled = true;
            else submitButton.Enabled = false;
        }
    }
}