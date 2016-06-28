using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Xml;
using VKLib.Handlers;
using VKLib.Model;
using System.Web;

namespace VKLib
{
    public class AudioUploadComman : BaseCommand<SoundBase>
    {

        public AudioUploadComman(string CommandName, NameValueCollection Params):base(CommandName,Params)
        {
        }

        public AudioUploadComman(AccessDataInfo accessData, string CommandName, NameValueCollection Params)
            : base(accessData,  CommandName, Params)
        {
        }


        public string ServerPath
        {
            get
            {
                try
                {
                     System.Xml.XmlNodeList nodes = base.xmlRes["response"].ChildNodes;
                     return base.xmlRes["response"].FirstChild.InnerText;//["upload_url"].Value;//XMLNodeHelper.TryParse(nodes[0],"upload_url");
                }
               
                catch (Exception)
                {
                    return "";
                }

            }
        }


        public AudioUploadedInfo UploadAudio(string path, string fileName)
        {
            string fullPath = path +@"\" + fileName;
            //var test = System.IO.File.Exists(fullPath); 
            byte[] bytes = System.IO.File.ReadAllBytes(fullPath);
            
            string answer = Reqeust.Post(ServerPath, bytes, fileName);
            
            string[] parsed = answer.Split(',');
            string server = parsed[1].Substring(9);
            string audio = parsed[2].Substring(9, (parsed[2].Length-1 - 9));
            string hash = parsed[3].Substring(8, parsed[3].Length - 2 - 8);
           // var tempanswer = VKLib.ViewModel.SoundDownloaderMovelView.DecodeUrlString(audio);
            //var decoded = UrlDecode.HttpUtility.UrlDecode(audio);
            return new AudioUploadedInfo(server, audio, hash);
        }

        
        private List<SoundBase> Bind(XmlDocument doc)
        {
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

     

    }
}