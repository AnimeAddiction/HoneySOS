using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace WindowsFormsApp1
{
    public partial class paging : Form
    {
        int[] frames;
        private List<ImageTextObject> ImageTextList;
        private Image image1 = Image.FromFile("C:\\Users\\Nikka Mendoza\\source\\repos\\HoneySOS\\WindowsFormsApp1\\Resources\\Group 102 (1).png");
        public paging(int[] frames)
        {
            InitializeComponent();
            this.frames = frames;
        }

        public void UpdateDataGrid(int[] frames)
        {
            ImageTextList = new List<ImageTextObject>();

            for (int i = 0; i < frames.Length; i++)
            {
                if (frames[i] == -1)
                {
                    ImageTextList.Add(new ImageTextObject(image1, "FREE"));
                }
                else
                {
                    ImageTextList.Add(new ImageTextObject(image1, frames[i].ToString()));
                }
            }

            dataGridView1.DataSource = null; // Reset the data source
            dataGridView1.DataSource = ImageTextList; // Set the new data source
            dataGridView1.Refresh();
        }


        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }


        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

  
        private void paging_Load(object sender, EventArgs e)
        {
           List<ImageTextObject> ImageTextList = new List<ImageTextObject>();

            Image image1 = Image.FromFile("C:\\Users\\Nikka Mendoza\\source\\repos\\HoneySOS\\WindowsFormsApp1\\Resources\\Group 102 (1).png");


            for (int i = 0; i < frames.Length; i++)
            {
                if (frames[i] == -1)
                {
                    ImageTextList.Add(new ImageTextObject(image1, "FREE"));

                }
                else
                {
                    ImageTextList.Add(new ImageTextObject(image1, frames[i].ToString()));
                }
                
            }





            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.DataSource = ImageTextList;

            // Configure the DataGridView columns
            DataGridViewTextBoxColumn imageTextColumn = new DataGridViewTextBoxColumn
            {
                HeaderText = "Image and Text",
                DataPropertyName = "Text" // This can be any property; the drawing will be handled in CellPainting
            };
            dataGridView1.Columns.Add(imageTextColumn);

            // Configure the DataGridView to use custom cell painting
        }

        private void dataGridView1_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex == 0) // Assuming the first column holds the ImageTextObject
            {
                e.Handled = true;
                e.PaintBackground(e.CellBounds, true);

                ImageTextObject imageTextObject = (ImageTextObject)dataGridView1.Rows[e.RowIndex].DataBoundItem;

                // Create a new bitmap to combine image and text
                Bitmap combinedImage = new Bitmap(imageTextObject.Image.Width, imageTextObject.Image.Height);
                using (Graphics g = Graphics.FromImage(combinedImage))
                {
                    // Draw the original image
                    g.DrawImage(imageTextObject.Image, 0, 0);

                    // Draw the text inside the image
                    Font font = e.CellStyle.Font;
                    Brush brush = new SolidBrush(e.CellStyle.ForeColor);
                    SizeF textSize = g.MeasureString(imageTextObject.Text, font);
                    PointF textPosition = new PointF(
                        (combinedImage.Width - textSize.Width) / 2,
                        (combinedImage.Height - textSize.Height) / 2);
                    g.DrawString(imageTextObject.Text, font, brush, textPosition);
                }

                // Draw the combined image in the cell
                e.Graphics.DrawImage(combinedImage, e.CellBounds.Left + 5, e.CellBounds.Top + 5, combinedImage.Width, combinedImage.Height);
            }
        }
    }
    public class ImageTextObject
    {
        public Image Image { get; set; }
        public string Text { get; set; }

        public ImageTextObject(Image image, string text)
        {
            Image = image;
            Text = text;
        }
    }

}
