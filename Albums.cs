using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

using System.IO;
using System.Xml;
using System.Net;
using System.Threading;

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

        string ParseName(string adress)
        {
            int i = adress.LastIndexOf('/');
            return adress.Substring(i);
        }

        string FindPhotoSize(XmlNode node) {
            if (node["src_xxbig"] != null) return node["src_xxbig"].InnerText;
            else if (node["src_xbig"] != null) return  node["src_xbig"].InnerText;
            else if (node["src_big"] != null) return node["src_big"].InnerText;
            else if (node["src"] != null) return node["src_small"].InnerText;
            else if (node["src_small"] != null) return node["src"].InnerText;
            return null;
        }


        private void button1_Click(object sender, EventArgs e)
        {
            if (loaded)
            {
                
              //  button1.Hide();

                List<int> ids = new List<int>();
                foreach (string item in checkedListBox1.CheckedItems)
                    foreach (var obj in albumsList)
                        if (obj.title == item)
                        {
                            XmlNodeList xml = Program.vk.GetPhotosFromAlbum(Program.vk.UserId, obj.id)["response"].ChildNodes;
                            System.IO.Directory.CreateDirectory(@"VK_PHOTOS\" + item);
                            foreach (XmlNode node in xml)
                            {
                                string path = @"VK_PHOTOS\" + item + @"\" + ParseName(node["src"].InnerText);
                                string name=FindPhotoSize(node);
                                Download(path, name);
                                /* StreamWriter wr = new StreamWriter(@"D:\VK_PHOTOS\" + item + @"\file.txt");
                                 wr.WriteLine(xml.InnerXml);
                                 wr.Close();
                                 */
                            }
                        }
            }
        }


        void Download( string path, string value)
        {
            System.Net.WebClient web = new System.Net.WebClient();
            web.DownloadProgressChanged += new System.Net.DownloadProgressChangedEventHandler(OnDownloading);
            web.DownloadFileCompleted += new AsyncCompletedEventHandler(OnCompleted);
            web.DownloadFile(new Uri(value), path);
        }

        void OnDownloading(object sender, DownloadProgressChangedEventArgs e)
        {
           // progressBar1.Value = e.ProgressPercentage;
        }

        void OnCompleted(object sender, AsyncCompletedEventArgs e)
        {
          //  progressBar1.Hide();
          //  button1.Show();

        }

        List<VKontakte1.VKApi.Album> albumsList = new List<VKontakte1.VKApi.Album>();
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