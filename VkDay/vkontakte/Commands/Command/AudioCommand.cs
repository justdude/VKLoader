using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Xml;
using VKMusicSync.Model;

namespace vkontakte
{
    public class AudiosCommand : BaseCommand<SoundBase>
    {
        public List<SoundBase> SoundsRes
        { get; set; }


        public AudiosCommand(string CommandName, NameValueCollection Params):base(CommandName,Params)
        {
        }

        public AudiosCommand(AccessData AccessData,string CommandName, NameValueCollection Params):base(AccessData,
                                                                                                        CommandName,
                                                                                                        Params)
        {
        }

        public List<SoundBase> Fill()
        {
            XmlDocument doc = base.xmlRes;
            List<SoundBase> sounds = new List<SoundBase>();
            SoundBase s = new SoundBase();
            System.Xml.XmlNodeList nodes = doc["response"].ChildNodes;
            for (int i = 1; i < nodes.Count; i++)
            {
                s = new SoundBase();
                s.artist = nodes[i]["artist"].InnerText;
                s.title = nodes[i]["title"].InnerText;
                s.duration = (int.Parse(nodes[i]["duration"].InnerText));
                s.url = nodes[i]["url"].InnerText;
                sounds.Add(s);
            }
            return sounds;
        }

        //public List<SoundBase> FillWithAddons(DotLastFm.LastFmApi lastFmApi)
        //{
        //    XmlDocument doc = base.xmlRes;
        //    List<SoundBase> CachedSounds = new List<SoundBase>();
        //    SoundBase s = new SoundBase();
        //    System.Xml.XmlNodeList nodes = doc["response"].ChildNodes;
        //    for (int i = 1; i < nodes.Count; i++)
        //    {
        //        s = new SoundBase();
        //        s.artist = nodes[i]["artist"].InnerText;
        //        s.title = nodes[i]["title"].InnerText;
        //        s.duration = (int.Parse(nodes[i]["duration"].InnerText));
        //        s.url = nodes[i]["url"].InnerText;
        //        s.Size = ComputeSize(s.url);
        //        s.authorPhotoPath = lastFmApi.Artist.GetInfo(s.artist).Images[3].Value; // little spike 
        //        CachedSounds.Add(s);
        //    }
        //    return CachedSounds;
        //}

        //private Int64 ComputeSize(string link)
        //{
        //    System.Net.WebClient client = new System.Net.WebClient();
        //    client.OpenRead(link);
        //    return Convert.ToInt64(client.ResponseHeaders["Content-Length"]);
        //}


        public int GetCount()
        {
            return int.Parse(base.xmlRes.SelectSingleNode("response").InnerText);
        }
    }
}
