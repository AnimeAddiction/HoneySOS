namespace WindowsFormsApp1
{
    partial class notepad
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            //this.panel1 = new System.Windows.Forms.Panel();
            //this.button6 = new System.Windows.Forms.Button();
            //this.button5 = new System.Windows.Forms.Button();
            //this.button4 = new System.Windows.Forms.Button();
            //this.button3 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.tab = new System.Windows.Forms.TabPage();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Black;
            this.panel1.Controls.Add(this.button6);
            this.panel1.Controls.Add(this.button5);
            this.panel1.Controls.Add(this.button4);
            this.panel1.Controls.Add(this.button3);
            this.panel1.Controls.Add(this.button2);
            this.panel1.Controls.Add(this.button1);
            this.panel1.Location = new System.Drawing.Point(-2, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1428, 48);
            this.panel1.TabIndex = 0;
            // 
            // button6
            // 
            this.button6.Image = global::WindowsFormsApp1.Properties.Resources.icons8_undo_30__1_;
            this.button6.Location = new System.Drawing.Point(1055, 3);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(41, 40);
            this.button6.TabIndex = 5;
            this.button6.UseVisualStyleBackColor = true;
            // 
            // button5
            // 
            this.button5.Image = global::WindowsFormsApp1.Properties.Resources.icons8_redo_30;
            this.button5.Location = new System.Drawing.Point(1102, 3);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(41, 40);
            this.button5.TabIndex = 4;
            this.button5.UseVisualStyleBackColor = true;
            // 
            // button4
            // 
            this.button4.Image = global::WindowsFormsApp1.Properties.Resources.icons8_save_as_30__1_;
            this.button4.Location = new System.Drawing.Point(155, 3);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(41, 40);
            this.button4.TabIndex = 3;
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button3
            // 
            this.button3.Image = global::WindowsFormsApp1.Properties.Resources.icons8_save_30__1_;
            this.button3.Location = new System.Drawing.Point(108, 3);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(41, 40);
            this.button3.TabIndex = 2;
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button2
            // 
            this.button2.Image = global::WindowsFormsApp1.Properties.Resources.icons8_open_file_30;
            this.button2.Location = new System.Drawing.Point(61, 3);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(41, 40);
            this.button2.TabIndex = 1;
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.Image = global::WindowsFormsApp1.Properties.Resources.icons8_add_file_30;
            this.button1.Location = new System.Drawing.Point(14, 3);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(41, 40);
            this.button1.TabIndex = 0;
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.tabControl1);
            this.panel2.Location = new System.Drawing.Point(34, 80);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1350, 520);
            this.panel2.TabIndex = 1;
            this.panel2.Paint += new System.Windows.Forms.PaintEventHandler(this.panel2_Paint);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // tab
            // 
            this.tab.BackColor = System.Drawing.Color.Transparent;
            this.tab.Location = new System.Drawing.Point(4, 29);
            this.tab.Name = "tab";
            this.tab.Padding = new System.Windows.Forms.Padding(3);
            this.tab.Size = new System.Drawing.Size(1336, 487);
            this.tab.TabIndex = 0;
            this.tab.Text = "new file";
            this.tab.Click += new System.EventHandler(this.tabPage1_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tab);
            this.tabControl1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabControl1.Location = new System.Drawing.Point(3, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1344, 520);
            this.tabControl1.TabIndex = 0;
            this.tabControl1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.tabControl1_MouseClick);
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1426, 771);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "Form2";
            this.Text = "Form2";
            this.Load += new System.EventHandler(this.Form2_Load);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tab;
    }
}