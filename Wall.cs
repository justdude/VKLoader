using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace VKMusicSync
{
    public partial class Wall : Form
    {
        public Wall()
        {
            InitializeComponent();
        }

        private void Wall_FormClosed(object sender, FormClosedEventArgs e)
        {
            Main main = this.Owner as Main;
            if (main != null) main.Show();
            
        }

        private void Wall_FormClosing(object sender, FormClosingEventArgs e)
        {
            //this.Owner.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Program.vk.PublishOnTheWall(Program.vk.UserId, textBox1.Text);
        }
    }
}
