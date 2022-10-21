using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace SimpleTextEditor
{
    public partial class TextEditor : Form
    {
        private readonly LoginScreen _loginForm;
        private readonly User _user;
        private string _filePath = "";

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
            userNameLabel.Text = $"User: {_user.UserName} ({_user.UserType})";
            
            if (_user.UserType == "View")
            {
                textArea.ReadOnly = true;
                boldButton.Enabled = false;
                italicButton.Enabled = false;
                underlineButton.Enabled = false;
                fontDropdown.Enabled = false;
            }
        }

        private void TextEditor_FormClosing(object sender, FormClosingEventArgs e)
        {
            CheckForChanges();
            if (new StackTrace().GetFrames().Any(x => x.GetMethod().Name == "Close")) // was Close() called via logout button?
            {
                _loginForm.Clear();
                _loginForm.Show();
            }
            else // ...or was the 'X' pressed? thus terminating application
                _loginForm.Close();
            
        }

        private void textArea_SelectionChanged(object sender, EventArgs e)
        {
            UpdateLabels();
        }

        // ----------------------
        // Top Strip Menu Actions
        // ----------------------

        private void newFileButton_Click(object sender, EventArgs e)
        {
            NewFile();
        }

        private void openButton_Click(object sender, EventArgs e)
        {
            OpenFile();
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            Save();
        }

        private void saveAsButton_Click(object sender, EventArgs e)
        {
            SaveAs();
        }

        private void boldButton_Click(object sender, EventArgs e)
        {
            textArea.SetSelectionBold(boldButton.Checked);
        }

        private void italicButton_Click(object sender, EventArgs e)
        {
            textArea.SetSelectionItalic(italicButton.Checked);
        }

        private void underlineButton_Click(object sender, EventArgs e)
        {
            textArea.SetSelectionUnderline(underlineButton.Checked);
        }

        private void helpButton_Click(object sender, EventArgs e)
        {
            var aboutDialog = new MyAboutBox();
            aboutDialog.ShowDialog();
        }

        private void fontComboBox_Click(object sender, EventArgs e)
        {
            UpdateFont();
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
            NewFile();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFile();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Save();
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveAs();
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
            if (textArea.SelectionFont == null) // i.e. if the selection has more than one font
            {
                boldButton.Checked = false;
                italicButton.Checked = false;
                underlineButton.Checked = false;
                fontDropdown.Text = "[Multiple Fonts]";
                return;
            }

            boldButton.Checked = textArea.SelectionFont.Bold;
            italicButton.Checked = textArea.SelectionFont.Italic;
            underlineButton.Checked = textArea.SelectionFont.Underline;
            fontDropdown.Text = textArea.SelectionFont.Name;
        }

        private void UpdateFont()
        {
            var fontDialog = new FontDialog();
            fontDialog.FontMustExist = true;

            if (fontDialog.ShowDialog() != DialogResult.OK) return;
            textArea.SelectionFont = fontDialog.Font;
        }

        private void OpenFile()
        {
            var openDialog = new OpenFileDialog();
            openDialog.Filter = "Rich Text Files (*.rtf)|*.rtf|Text Files (*.txt)|*.txt";
            openDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            openDialog.RestoreDirectory = true;

            if (openDialog.ShowDialog() != DialogResult.OK || openDialog.FileName.Length <= 0) return;
            _filePath = openDialog.FileName;
            switch (openDialog.FilterIndex)
            {
                case 1: // .rtf files
                    textArea.LoadFile(_filePath);
                    break;
                case 2: // .txt files
                    textArea.LoadFile(_filePath, RichTextBoxStreamType.PlainText);
                    break;
            }
        }

        private void Save()
        {
            if (_filePath == "") SaveAs();
            else SaveFile();
        }

        private void SaveAs()
        {
            var saveDialog = new SaveFileDialog();
            saveDialog.Filter = "Rich Text File (*.rtf)|*.rtf|Text File (*.txt)|*.txt";
            saveDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            saveDialog.OverwritePrompt = true;
            saveDialog.RestoreDirectory = true;

            if (saveDialog.ShowDialog() != DialogResult.OK || saveDialog.FileName.Length <= 0) return;
            _filePath = saveDialog.FileName;
            SaveFile();
        }

        private void SaveFile()
        {
            var ext = Path.GetExtension(_filePath);
            switch (ext)
            {
                case ".rtf":
                    textArea.SaveFile(_filePath);
                    break;
                case ".txt":
                    textArea.SaveFile(_filePath, RichTextBoxStreamType.PlainText);
                    break;
            }
        }

        private void NewFile()
        {
            var newFile = new TextEditor(_loginForm, _user);
            newFile.Show();
            Dispose(false);
        }

        private void CheckForChanges()
        {
            if (_filePath == "" && textArea.Text == "") return; // if file is not saved anywhere, but it has no text
            if (!IsFileChanged()) return; // if the file is saved, but no changes have been made
            
            var result = MessageBox.Show("You have unsaved changes. Would you like to save this file?",
                "File not saved",
                MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
            if (result != DialogResult.Yes) return;
            Save();
            MessageBox.Show("File saved successfully.", "File Saved", MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        private bool IsFileChanged()
        {
            if (_filePath == "") return true;
            
            var existingFile = File.ReadAllText(_filePath);
            var currentText = "";
            switch (Path.GetExtension(_filePath))
            {
                case ".rtf":
                    currentText = textArea.Rtf;
                    break;
                case ".txt":
                    textArea.SaveFile("tempFile.txt", RichTextBoxStreamType.PlainText); // no other way of removing RTF encoding for comparison
                    currentText = File.ReadAllText("tempFile.txt");
                    break;
            }
            return !string.Equals(existingFile, currentText, StringComparison.InvariantCulture);
        }
    }
}