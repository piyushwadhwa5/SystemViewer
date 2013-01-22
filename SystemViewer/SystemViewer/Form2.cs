using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SystemViewer
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                Form1.newprodet = textBox1.Text.Trim();
                this.DialogResult = DialogResult.OK;
            }
            catch
            {
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.CheckFileExists = false;
                ofd.ShowDialog();
                textBox1.Text = ofd.FileName;
            }
            catch
            {
            }
         }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                Form1.newprodet = "";
                this.DialogResult = DialogResult.Cancel;
            }
            catch
            {
            }
        }
    }
}
