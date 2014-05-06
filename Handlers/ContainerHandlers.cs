using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using vkAPI;
namespace VK
{

    public enum PhotosSize
    {
        xxx,
        xx,
        x,
        medium,
        small
    }

    public class AudiosContainer
    {
        List<Sound> sounds = new List<Sound>();
        public void Bind(XmlDocument doc)
        {
            sounds.Clear();
            Sound s = new Sound();
            System.Xml.XmlNodeList nodes = doc["response"].ChildNodes;
            //System.Windows.Forms.MessageBox.Show(nodes.Count);
            for (int i = 1; i<nodes.Count; i++)
            {
               // System.Windows.Forms.MessageBox.Show(nodes[i]);
                s = new Sound();
                s.artist = nodes[i]["artist"].InnerText;
                s.title = nodes[i]["title"].InnerText;
                s.duration = nodes[i]["duration"].InnerText;
                s.url = nodes[i]["url"].InnerText;
                sounds.Add(s);
            }
        }

        public List<Sound> getSound()
        {
            return sounds;
        }
    }

    public class AlbumsContainer
    {
        List<Album> albums = new List<Album>();

        public void Bind(XmlDocument doc)
        {
            albums.Clear();
            Album t = new Album();
            XmlNodeList albumList = doc["response"].ChildNodes;
            foreach (System.Xml.XmlNode album in albumList)
            {
                t = new Album();
                t.id = int.Parse(album["aid"].InnerText);
                t.title = album["title"].InnerText;
                t.picture = album["thumb_id"].InnerText;
                t.comment = album["description"].InnerText;
                albums.Add(t);
            }
        }

        public List<Album> getAlbums()
        {
            return albums;
        }
    }

    public class PhotosAndAlbumBinder:AlbumsContainer 
    {
        List<MyCategory> albums = new List<MyCategory>();

        public void Bind(XmlDocument doc)
        {
            albums.Clear();
            MyCategory t = new MyCategory();
            XmlNodeList albumList = doc["response"].ChildNodes;
            foreach (System.Xml.XmlNode album in albumList)
            {
                t = new MyCategory();
                t.Name = album["title"].InnerText;
                t.Images = this.GetPhotosListFromAlbum(int.Parse(album["aid"].InnerText));
                albums.Add(t);
            }
        }

        public List<MyImage> GetPhotosListFromAlbum(int albumID) {
            List<MyImage> photos = new List<MyImage>();
                XmlNodeList xml = Program.vk.GetPhotosFromAlbum(Program.vk.UserId, albumID)["response"].ChildNodes;
                foreach (XmlNode node in xml)
                {
                   photos.Add(new MyImage(Albums.GetMaxPhotoAdress(node,PhotosSize.x))); 
                }
            return photos;
        }

        public List<MyCategory> getAlbums()
        {
            return albums;
        }
    }
}
