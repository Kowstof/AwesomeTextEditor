using System;
using System.Diagnostics;
using System.Drawing;
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
            // Form properties
            KeyPreview = true;
            textArea.Width = Width;
            textArea.Height = Height;
            userNameLabel.Text = $"User: {_user.UserName} ({_user.UserType})";


            // Font dropdown extra prep
            fontDropdown.ComboBox.DrawMode = DrawMode.OwnerDrawFixed;
            fontDropdown.ComboBox.DrawItem += fontDropdown_DrawItem;
            var installedFontList = FontFamily.Families.ToList();
            var fontNameList = installedFontList.Select(font => font.Name).ToList();
            fontDropdown.ComboBox.DataSource = fontNameList;
            fontDropdown.Text = "Segoe UI";

            // Font size dropdown prep
            fontSizeComboBox.SelectedIndex = 4; // default font size 12

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
        
        private void fontDropdown_DrawItem(object sender, DrawItemEventArgs e) 
        {
            var fontName = (string)fontDropdown.Items[e.Index];
            var font = new Font(fontName, fontDropdown.Font.Size);
            e.DrawBackground();
            e.Graphics.DrawString(fontName, font, Brushes.Black, e.Bounds.X, e.Bounds.Y);
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

        private void fontDropdown_DropDownClosed(object sender, EventArgs e)
        {
            if (textArea.SelectionFont != null) textArea.SelectionFont.Dispose(); // save those precious resources ;)
            var newFont = new Font(fontDropdown.SelectedItem.ToString(), Convert.ToInt32(fontSizeComboBox.Text));
            textArea.SelectionFont = newFont;
            UpdateLabels();
        }

        private void fontSizeComboBox_DropDownClosed(object sender, EventArgs e)
        {
            UpdateFontSize();
        }

        private void fontSizeComboBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) 
            { 
                UpdateCustomFontSize();
                e.Handled = true;
                e.SuppressKeyPress = true;
            }

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
                fontDropdown.Font = new Font("Segoe UI", fontDropdown.Font.Size);
                fontSizeComboBox.Text = "12"; // resets to a default, since they might be different
                return;
            }

            boldButton.Checked = textArea.SelectionFont.Bold;
            italicButton.Checked = textArea.SelectionFont.Italic;
            underlineButton.Checked = textArea.SelectionFont.Underline;
            fontDropdown.Text = textArea.SelectionFont.Name;
            fontDropdown.Font = new Font(textArea.SelectionFont.Name, fontDropdown.Font.Size);
            fontDropdown.SelectionLength = 0;
            fontSizeComboBox.Text = textArea.SelectionFont.SizeInPoints.ToString();
        }

        private void UpdateFontSize()
        {
            if (textArea.SelectionFont == null)
            {
                MessageBox.Show("Sorry, changing the size of multiple fonts at once isn't supported yet!", "Uh oh!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (fontSizeComboBox.Text == "") return;
            float.TryParse(fontSizeComboBox.SelectedItem.ToString(), out float fontSize);
            var newFont = ChangeFontSize(textArea.SelectionFont, fontSize);
            textArea.SelectionFont.Dispose();
            textArea.SelectionFont = newFont;
        }

        private void UpdateCustomFontSize()
        {
            if (textArea.SelectionFont == null)
            {
                MessageBox.Show("Sorry, changing the size of multiple fonts at once isn't supported yet!", "Uh oh!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (!float.TryParse(fontSizeComboBox.Text, out float fontSize)) return;
            var newFont = ChangeFontSize(textArea.SelectionFont, fontSize);
            textArea.SelectionFont.Dispose();
            textArea.SelectionFont = newFont;
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

        private Font ChangeFontSize(Font font, float fontSize)
        {
            if (font != null)
            {
                float currentSize = font.Size;
                font = new Font(font.Name, fontSize, font.Style, font.Unit, font.GdiCharSet, font.GdiVerticalFont);
            }
            return font;
        }
    }
}