using System;
using System.Windows.Forms;

namespace SimpleTextEditor
{
    public partial class TextEditor : Form
    {
        private readonly Form _login;
        public TextEditor(Form login)
        {
            _login = login;
            InitializeComponent();
        }

        private void Form1_Load_1(object sender, EventArgs e)
        {
            textBox.Width = Width;
            textBox.Height = Height;
        }
        // ---------------------
        // Dropdown Menu Actions
        // ---------------------

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void logoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var about = new MyAboutBox();
            about.ShowDialog();
        }

        // ----------------------
        // Top Strip Menu Actions
        // ----------------------

        private void toolStripButton8_Click(object sender, EventArgs e)
        {
            var myAbout = new MyAboutBox();
            myAbout.ShowDialog();
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
        }

        private void toolStripTextBox1_Click(object sender, EventArgs e)
        {
            var fontDialog = new FontDialog();
            fontDialog.ShowDialog();
        }


        // -----------------------
        // Side Strip Menu Actions
        // -----------------------
        private void TextEditor_FormClosing(object sender, FormClosingEventArgs e)
        {
            _login.Show();
        }
    }
}