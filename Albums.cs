using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.IO;
using System.Xml;

namespace VK
{
    public partial class Albums : Form
    {
        public Albums()
        {
            InitializeComponent();
        }

        private void Albums_FormClosed(object sender, FormClosedEventArgs e)
        {
            Main main = this.Owner as Main;
            if (main != null) main.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void Albums_Load(object sender, EventArgs e)
        {
            checkedListBox1.Items.Clear();
            System.Xml.XmlNodeList albums = Program.vk.GetAllAlbums(Program.vk.UserId)["response"].ChildNodes;
            foreach (System.Xml.XmlNode album in albums)
            {
                checkedListBox1.Items.Add(album["title"].InnerText);
            }

        }
    }
}
