using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Xml;
using VKMusicSync.Handlers;
using VKMusicSync.Model;
using System.Web;

namespace vkontakte
{
    public class AudioUploadComman : BaseCommand<Sound>
    {

        public AudioUploadComman(string CommandName, NameValueCollection Params):base(CommandName,Params)
        {
        }

        public AudioUploadComman(AccessData AccessData, string CommandName, NameValueCollection Params)
            : base(AccessData,  CommandName, Params)
        {
        }

        public override List<Sound> ExecuteForList()
        {
            base.ExecuteCommand();
            return this.Bind(base.Result);
        }

        public override object Execute()
        {
            base.ExecuteCommand();
            return null;
        }

        public override void ExecuteNonQuery()
        {
            base.ExecuteCommand();
        }

        public string ServerPath
        {
            get
            {
                try
                {
                     System.Xml.XmlNodeList nodes = base.Result["response"].ChildNodes;
                     return base.Result["response"].FirstChild.InnerText;//["upload_url"].Value;//XMLNodeHelper.TryParse(nodes[0],"upload_url");
                }
               
                catch (Exception)
                {
                    return "";
                }

            }
        }


        public AudioUploadedInfo UploadAudio(string path, string fileName)
        {
            byte[] bytes = System.IO.File.ReadAllBytes(path + fileName);
            string answer = Reqeust.Post(ServerPath, bytes, fileName);
            
            string[] parsed = answer.Split(',');
            string server = parsed[1].Substring(9);
            string audio = parsed[2].Substring(9, (parsed[2].Length-1 - 9));
            string hash = parsed[3].Substring(8, parsed[3].Length - 2 - 8);
            var tempanswer = VKMusicSync.ModelView.SoundDownloaderMovelView.DecodeUrlString(audio);
            //var decoded = UrlDecode.HttpUtility.UrlDecode(audio);
            return new AudioUploadedInfo(server, audio, hash);
        }

        
        private List<Sound> Bind(XmlDocument doc)
        {
            List<Sound> sounds = new List<Sound>();
            Sound s = new Sound();
            System.Xml.XmlNodeList nodes = doc["response"].ChildNodes;
            for (int i = 1; i < nodes.Count; i++)
            {
                s = new Sound();
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