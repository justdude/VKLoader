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
            if (loaded)
            {
                System.Net.WebClient web = new System.Net.WebClient();
                
                List<int> ids = new List<int>();
                foreach (string item in checkedListBox1.CheckedItems)
                    foreach (var obj in albumsList)
                        if (obj.title == item)
                        {
                            XmlNodeList xml = Program.vk.GetPhotosFromAlbum(Program.vk.UserId, obj.id)["response"].ChildNodes;
                            System.IO.Directory.CreateDirectory(@"D:\VK_PHOTOS\" + item);
                            foreach (XmlNode node in xml)
                                web.DownloadFile(node["src_big"].InnerText, @"D:\VK_PHOTOS\" + item + @"\" + node["aid"].InnerText);

                           /* StreamWriter wr = new StreamWriter(@"D:\VK_PHOTOS\" + item + @"\file.txt");
                            wr.WriteLine(xml.InnerXml);
                            wr.Close();
                            */
                        }
            }
        }

        List<VKontakte1.VKApi.Album> albumsList=new List<VKontakte1.VKApi.Album>();
        bool loaded = false;
        private void Albums_Load(object sender, EventArgs e)
        {
            LoadAlbums();
        }


        void LoadAlbums()
        {
            checkedListBox1.Items.Clear();
            System.Xml.XmlNodeList albums = Program.vk.GetAllAlbums(Program.vk.UserId)["response"].ChildNodes;
            albumsList.Clear();
            VKontakte1.VKApi.Album t = new VKontakte1.VKApi.Album();

            foreach (System.Xml.XmlNode album in albums)
            {
                t = new VKontakte1.VKApi.Album();
                checkedListBox1.Items.Add(album["title"].InnerText);
                t.id = int.Parse(album["aid"].InnerText);
                t.title = album["title"].InnerText;
                t.picture = album["thumb_id"].InnerText;
                t.comment = album["description"].InnerText;
                albumsList.Add(t);
            }
            loaded = true;
        }
        
    }
}
