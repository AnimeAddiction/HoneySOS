using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Speech.Recognition;
using System.Speech.Synthesis;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolTip;

namespace WindowsFormsApp1
{
    public partial class notepad : Form
    {
        private Point _imageLocation = new Point(13, 5);
        private Point _imgHitArea = new Point(13, 2);
        private Image closeR;
        private Image closeRHover;
        private bool isCloseButtonHovered = false;
        private int hoveredTabIndex = -1;


        public notepad()
        {
            

            InitializeComponent();
            tabControl1.TabPages.RemoveAt(0);
            // Add mouse event handlers
            tabControl1.MouseMove += new MouseEventHandler(tabControl1_MouseMove);
            tabControl1.MouseLeave += new EventHandler(tabControl1_MouseLeave);
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void progressBar1_Click(object sender, EventArgs e)
        {

        }

        private void Form2_Load(object sender, EventArgs e)
        {
            tabControl1.DrawMode = TabDrawMode.OwnerDrawFixed;
            tabControl1.DrawItem += tabControl1_DrawItem;
            closeR = WindowsFormsApp1.Properties.Resources.icons8_close_10;
            closeRHover = WindowsFormsApp1.Properties.Resources.icons8_close_10__1_;
            tabControl1.Padding = new Point(10, 3);

            tabControl1.MouseMove += tabControl1_MouseMove;
            tabControl1.MouseLeave += tabControl1_MouseLeave;
        }


        private void tabControl1_DrawItem(object sender, DrawItemEventArgs e)
{
    try
    {
        TabControl tc = (TabControl)sender;
        Rectangle tabRect = tc.GetTabRect(e.Index);

        // Define the close button rectangle with some padding
        int padding = 5;
        Rectangle closeButtonRect = new Rectangle(tabRect.Right - 20 - padding, tabRect.Top + 4, 16, 16);
        Image img = e.Index == hoveredTabIndex ? closeRHover : closeR;

        // Calculate the maximum width available for the text
        int maxTextWidth = closeButtonRect.Left - tabRect.Left - padding;

        // Get the original tab text
        string originalText = tc.TabPages[e.Index].Text;
        tc.TabPages[e.Index].Tag = originalText; // Store the full text in the Tag property
        string tabText = originalText;

        // Measure the text width and truncate if necessary
        while (TextRenderer.MeasureText(tabText, tc.Font).Width > maxTextWidth)
        {
            if (tabText.Length <= 1)
                break;

            tabText = tabText.Substring(0, tabText.Length - 1);
        }

        // If truncated, add ellipsis
        if (tabText != originalText)
        {
            tabText = tabText.Substring(0, tabText.Length - 3) + "...";
        }

        // Draw the tab name
        using (Brush brush = new SolidBrush(tc.TabPages[e.Index].ForeColor))
        {
            Rectangle textRect = new Rectangle(tabRect.Left + padding, tabRect.Top, maxTextWidth, tabRect.Height);
            e.Graphics.DrawString(tabText, tc.Font, brush, textRect, new StringFormat { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Center });
        }

        // Draw the close button image
        e.Graphics.DrawImage(img, closeButtonRect.Location);

        // Update the close button hover state
        if (closeButtonRect.Contains(tc.PointToClient(Control.MousePosition)))
        {
            hoveredTabIndex = e.Index;
            img = closeRHover; // Use the hover icon
        }
        else if (hoveredTabIndex == e.Index)
        {
            hoveredTabIndex = -1;
        }
    }
    catch
    {
        // Handle exceptions
    }
}


        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void tabControl1_MouseClick(object sender, MouseEventArgs e)
        {
            TabControl tc = (TabControl)sender;
            for (int i = 0; i < tc.TabCount; i++)
            {
                Rectangle tabRect = tc.GetTabRect(i);
                Rectangle closeButtonRect = new Rectangle(tabRect.Right - 20, tabRect.Top + 4, 16, 16);

                if (closeButtonRect.Contains(e.Location))
                {
                    tc.TabPages.RemoveAt(i);
                    return;
                }
            }
        }

        private void tabControl1_MouseMove(object sender, MouseEventArgs e)
        {
            TabControl tc = (TabControl)sender;
            for (int i = 0; i < tc.TabCount; i++)
            {
                Rectangle tabRect = tc.GetTabRect(i);
                if (tabRect.Contains(e.Location))
                {
                    string fullText = tc.TabPages[i].Tag as string;
                    if (!string.IsNullOrEmpty(fullText))
                    {
                        toolTip1.SetToolTip(tc, fullText);
                        return;
                    }
                }
                Rectangle closeButtonRect = new Rectangle(tabRect.Right - 20, tabRect.Top + 4, 16, 16);

                if (closeButtonRect.Contains(e.Location))
                {
                    hoveredTabIndex = i;
                    tc.Invalidate();
                    return;
                }
            }
            toolTip1.SetToolTip(tc, null); // Clear the tooltip if not hovering over a tab

            hoveredTabIndex = -1;
            tc.Invalidate();
        }

        private void tabControl1_MouseLeave(object sender, EventArgs e)
        {
            hoveredTabIndex = -1;
            tabControl1.Invalidate();
            toolTip1.SetToolTip(tabControl1, null); // Clear the tooltip when the mouse leaves the tab control
        }

        public void button1_Click(object sender, EventArgs e) //add file
        {
            TabPage newTabPage = new TabPage(); // Create a new tab page
            newTabPage.Text = "NewTab"; // Set the text of the tab page
            RichTextBox richTextBox = new RichTextBox();
            richTextBox.Dock = DockStyle.Fill;
            newTabPage.Controls.Add(richTextBox); // Add the RichTextBox to the new tab page
            tabControl1.TabPages.Add(newTabPage);
            tabControl1.SelectedTab = newTabPage;
        }

        public void button2_Click(object sender, EventArgs e) //open file
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*"; // Set filter to only show .txt files
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = dialog.FileName;
                try
                {
                    // Read the contents of the file using StreamReader
                    using (StreamReader reader = new StreamReader(filePath))
                    {
                        string fileContent = reader.ReadToEnd();

                        // Create a new tab page
                        TabPage newTabPage = new TabPage(Path.GetFileName(filePath));
                        newTabPage.Tag = filePath;
                        RichTextBox richTextBox = new RichTextBox();
                        richTextBox.Dock = DockStyle.Fill;
                        richTextBox.Text = fileContent;

                        // Add the rich text box to the tab page
                        newTabPage.Controls.Add(richTextBox);

                        // Add the new tab page to the tab control
                        tabControl1.TabPages.Add(newTabPage);
                    }
                }
                catch (Exception ex)
                {
                    say("Error: Could not read the file");
                    MessageBox.Show("Error: Could not read the file. Original error: " + ex.Message);
                }
            }
        }

        public void button3_Click(object sender, EventArgs e)
        {
            // Check if there is a selected tab
            if (tabControl1.SelectedTab == null)
            {
                say("No tab selected.");
                MessageBox.Show("No tab selected.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Check if the selected tab contains a RichTextBox control
            RichTextBox richTextBox = tabControl1.SelectedTab.Controls.OfType<RichTextBox>().FirstOrDefault();
            if (richTextBox == null)
            {
                say("No text to save in the selected tab.");
                MessageBox.Show("No text to save in the selected tab.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Check if the selected tab has an associated filename
            string filePath = tabControl1.SelectedTab.Tag as string;
            if (string.IsNullOrEmpty(filePath))
            {
                // Prompt the user to select a file location to save for new tabs
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    filePath = saveFileDialog.FileName;
                    tabControl1.SelectedTab.Tag = filePath; // Associate the file path with the tab
                }
                else
                {
                    return; // User canceled the operation
                }
            }

            try
            {
                // Write the content of the RichTextBox to the file associated with the tab
                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    writer.Write(richTextBox.Text);
                }

                say("Changes saved successfully.");

                MessageBox.Show("Changes saved successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                say($"Error: {ex.Message}");
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }




        public void button4_Click(object sender, EventArgs e)
        {
            // Check if there are any tabs open
            if (tabControl1.TabCount == 0)
            {
                say("No tabs open to save.");
                MessageBox.Show("No tabs open to save.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Get the currently selected tab
            TabPage selectedTabPage = tabControl1.SelectedTab;

            // Check if the selected tab has a RichTextBox
            RichTextBox richTextBox = selectedTabPage.Controls.OfType<RichTextBox>().FirstOrDefault();
            if (richTextBox == null)
            {
                say("No text to save in the selected tab.");
                MessageBox.Show("No text to save in the selected tab.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Prompt the user to select a file location
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = saveFileDialog.FileName;

                try
                {
                    // Write the content of the RichTextBox to the selected file
                    using (StreamWriter writer = new StreamWriter(filePath))
                    {
                        writer.Write(richTextBox.Text);
                    }

                    MessageBox.Show("File saved successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    say($"Error: {ex.Message}");
                    MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
