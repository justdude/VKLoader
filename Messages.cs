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
    public partial class Messages : Form
    {
        public Messages()
        {
            InitializeComponent();
        }

       /* void LoadMessages()
        {
            treeView1.Nodes.Clear();
            System.Xml.XmlNodeList friends = Program.vk.GetFriends(Program.vk.UserId)["response"].ChildNodes;
            albumsList.Clear();
            vkAPI.VKApi.Friend t = new vkAPI.VKApi.Friend();

            foreach (System.Xml.XmlNode friend in friends)
            {
                t = new vkAPI.VKApi.Friend();
                checkedListBox1.Items.Add(album["title"].InnerText);
                t.id = int.Parse(album["uid"].InnerText);
                t.last_name = int.Parse(album["uid"].InnerText);
                albumsList.Add(t);
            }
            loaded = true;
        }*/

        private void button1_Click(object sender, EventArgs e)
        {

        }
    }
}
