using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace SimpleTextEditor
{
    public partial class TextEditor : Form
    {
        private readonly LoginScreen _loginForm;
        private readonly User _user;
        public TextEditor(LoginScreen loginForm, User user)
        {
            _loginForm = loginForm;
            _user = user;
            InitializeComponent();
        }
        
        // --------------
        // General Events
        // --------------
        
        private void Form1_Load_1(object sender, EventArgs e)
        {
            textArea.Width = Width;
            textArea.Height = Height;
            fontDropdown.Text = textArea.Font.Name;
            userNameLabel.Text = @"User: " + _user.UserName;
        }
        
        private void TextEditor_FormClosing(object sender, FormClosingEventArgs e)
        {
            _loginForm.Clear();
            _loginForm.Show();
        }
        
        private void textArea_SelectionChanged(object sender, EventArgs e)
        {
            UpdateLabels();
        }
        
        // ----------------------
        // Top Strip Menu Actions
        // ----------------------

        private void boldButton_Click(object sender, EventArgs e)
        {
            textArea.SelectionFont = new Font(textArea.SelectionFont, textArea.SelectionFont.Style ^ FontStyle.Bold);
        }
        
        private void italicButton_Click(object sender, EventArgs e)
        {
            textArea.SelectionFont = new Font(textArea.SelectionFont, textArea.SelectionFont.Style ^ FontStyle.Italic);
        }
        
        private void underlineButton_Click(object sender, EventArgs e)
        {
            textArea.SelectionFont = new Font(textArea.SelectionFont, textArea.SelectionFont.Style ^ FontStyle.Underline);
        }
        
        private void helpButton_Click(object sender, EventArgs e)
        {
            var myAbout = new MyAboutBox();
            myAbout.ShowDialog();
        }
        
        private void fontComboBox_Click(object sender, EventArgs e)
        {
            var fontDialog = new FontDialog();
            fontDialog.ShowDialog();
            //fontDialog.Apply += new FileSystemEventHandler();
        }


        // -----------------------
        // Side Strip Menu Actions
        // -----------------------
        
        private void cutButton_Click(object sender, EventArgs e)
        {
            textArea.Cut();
        }

        private void copyButton_Click(object sender, EventArgs e)
        {
            textArea.Copy();
        }

        private void pasteButton_Click(object sender, EventArgs e)
        {
            textArea.Paste();
        }
        
        
        // ---------------------
        // Dropdown Menu Actions
        // ---------------------

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFile();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFile();
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void logoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textArea.Cut();
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textArea.Copy();
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textArea.Paste();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var about = new MyAboutBox();
            about.ShowDialog();
        }

        // ---------------------
        // Functions
        // ---------------------
        
        private void UpdateLabels()
        {
            boldButton.Checked = textArea.SelectionFont.Bold;
            italicButton.Checked = textArea.SelectionFont.Italic;
            underlineButton.Checked = textArea.SelectionFont.Underline;
            fontDropdown.Text = textArea.SelectionFont.Name;
        }

        private void UpdateFont(Font font)
        {
            textArea.SelectionFont = font;
        }

        private void OpenFile()
        {
            var openDialog = new OpenFileDialog();
            openDialog.Filter = "Rich Text Files (*.rtf)|*.rtf|Text Files (*.txt)|*.txt";

            if (openDialog.ShowDialog() != DialogResult.OK || openDialog.FileName.Length <= 0) return;
            switch (openDialog.FilterIndex)
            {
                case 1: // .rtf files
                    textArea.LoadFile(openDialog.FileName);
                    break;
                case 2: // .txt files
                    textArea.LoadFile(openDialog.FileName, RichTextBoxStreamType.PlainText);
                    break;
            }
        }

        private void SaveFile()
        {
            var saveDialog = new SaveFileDialog();
            saveDialog.Filter = "Rich Text File (*.rtf)|*.rtf|Text File (*.txt)|*.txt";
            saveDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            saveDialog.OverwritePrompt = true;
            saveDialog.ShowDialog();
            
            if (saveDialog.ShowDialog() != DialogResult.OK || saveDialog.FileName.Length <= 0) return;
            switch (saveDialog.FilterIndex)
            {
                case 1: // save as .rtf
                    textArea.SaveFile(saveDialog.FileName);
                    break;
                case 2: // save as .txt
                    textArea.SaveFile(saveDialog.FileName, RichTextBoxStreamType.PlainText);
                    break;
            }
            
        }
    }
}