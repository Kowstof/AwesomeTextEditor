﻿using System;
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
    public partial class LoginScreen : Form
    {
        public LoginScreen()
        {
            InitializeComponent();
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
            var editor = new TextEditor();
            editor.Show();
            Hide();
        }

        private void NewUserButton_Click(object sender, EventArgs e)
        {
            var newUser = new NewUserScreen();
            newUser.Show();
            Hide();
        }
    }
}