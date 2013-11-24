using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using vkAPI;
namespace VK
{

    public class AudioContainer
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

   /* public class PhotoContainer
    {
        List<Photo> photos = new List<Photo>();

        public void Bind(XmlDocument doc)
        {
            photos.Clear();
            Photo s = new Photo();
            System.Xml.XmlNodeList nodes = doc["response"].ChildNodes;
            //System.Windows.Forms.MessageBox.Show(nodes.Count);
            for (int i = 1; i < nodes.Count; i++)
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

        public List<Photo> getPhotos()
        {
            return photos;
        }
    }*/
}
