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
}
