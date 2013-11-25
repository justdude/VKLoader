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


        List<Album> albums = new List<Album>();
        Downloader downloader;
        string path = @"VK_PHOTOS\";
        private void Albums_Load(object sender, EventArgs e)
        {
            checkedListBox1.Items.Clear();
            AlbumsContainer albumContainer = new AlbumsContainer();

            albumContainer.Bind(Program.vk.GetAllAlbums(Program.vk.UserId,false));
            albums = albumContainer.getAlbums();
            foreach (var album in albums)
                checkedListBox1.Items.Add(album.title);

            downloader = new Downloader(path, this.OnDownloading, this.OnCompleted);

        }

        string ParseName(string adress)
        {
            int i = adress.LastIndexOf('/');
            return adress.Substring(i);
        }


        enum PhotosSize { 
            xxx,
            xx,
            x,
            medium,
            small
        }
        string GetMaxPhotoAdress(XmlNode node,PhotosSize size)
        {
            if (node["src_xxbig"] != null && size == PhotosSize.xxx) return node["src_xxbig"].InnerText;
            else if (node["src_xbig"] != null && size == PhotosSize.xx) return node["src_xbig"].InnerText;
            else if (node["src_big"] != null && size == PhotosSize.x) return node["src_big"].InnerText;
            else if (node["src"] != null && size == PhotosSize.medium) return node["src_small"].InnerText;
            else if (node["src_small"] != null && size == PhotosSize.small) return node["src"].InnerText;
            string str = GetMaxPhotoAdress(node);
            if (null != str && str != string.Empty) return str; 
            return null;
        }

        string GetMaxPhotoAdress(XmlNode node) {
            if (node["src_xxbig"] != null) return node["src_xxbig"].InnerText;
            else if (node["src_xbig"] != null) return  node["src_xbig"].InnerText;
            else if (node["src_big"] != null) return node["src_big"].InnerText;
            else if (node["src"] != null) return node["src_small"].InnerText;
            else if (node["src_small"] != null) return node["src"].InnerText;
            return null;
        }


        private void button1_Click(object sender, EventArgs e)
        {
            foreach (string item in checkedListBox1.CheckedItems)
                foreach (var obj in albums)
                    if (obj.title == item)
                    {
                        XmlNodeList xml = Program.vk.GetPhotosFromAlbum(Program.vk.UserId, obj.id)["response"].ChildNodes;
                        System.IO.Directory.CreateDirectory(@"VK_PHOTOS\" + item);
                        foreach (XmlNode node in xml)
                        {
                            string path_ = path + item + @"\" + ParseName(node["src"].InnerText);
                            string name = GetMaxPhotoAdress(node,PhotosSize.xxx);
                            downloader.Download(name, path_);
                        }
                    }
            downloader.OpenPath();
        }



        void OnDownloading(object sender, DownloadProgressChangedEventArgs e)
        {

        }

        void OnCompleted(object sender, AsyncCompletedEventArgs e)
        {
            MessageBox.Show("done");
        }

        private void Albums_FormClosed(object sender, FormClosedEventArgs e)
        {
            Main main = this.Owner as Main;
            if (main != null) main.Show();
        }



    }
}