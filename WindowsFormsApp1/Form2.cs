using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolTip;

namespace WindowsFormsApp1
{
    public partial class Form2 : Form
    {
        private Point _imageLocation = new Point(13, 5);
        private Point _imgHitArea = new Point(13, 2);
        Image closeR;
        private Image closeRHover;
        private bool isCloseButtonHovered = false;
        private int hoveredTabIndex = -1;

        public Form2()
        {
            InitializeComponent();
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
                Rectangle closeButtonRect = new Rectangle(tabRect.Right - 20, tabRect.Top + 4, 16, 16);
                Image img = e.Index == hoveredTabIndex ? closeRHover : closeR;

                e.Graphics.DrawImage(img, closeButtonRect.Location);
            }
            catch
            {
                // Handle exceptions
            }
        }

        private void tabPage2_Click(object sender, EventArgs e)
        {

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
                Rectangle closeButtonRect = new Rectangle(tabRect.Right - 20, tabRect.Top + 4, 16, 16);

                if (closeButtonRect.Contains(e.Location))
                {
                    hoveredTabIndex = i;
                    tc.Invalidate();
                    return;
                }
            }

            hoveredTabIndex = -1;
            tc.Invalidate();
        }


        private void tabControl1_MouseLeave(object sender, EventArgs e)
        {
            hoveredTabIndex = -1;
            tabControl1.Invalidate();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            TabPage newTabPage = new TabPage("New Tab");
            tabControl1.TabPages.Add(newTabPage);
            tabControl1.SelectedTab = newTabPage;
        }

    }
}
