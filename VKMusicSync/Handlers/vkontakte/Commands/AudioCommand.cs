using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Xml;
using VKMusicSync.Model;

namespace vkontakte
{
    public class AudioCommand : BaseCommand<Sound>
    {

        public Sound Result
        {
            get;
            private set;
        }

        public AudioCommand(string CommandName, NameValueCollection Params):base(CommandName,Params)
        {
        }

        public AudioCommand(AccessData AccessData,string CommandName, NameValueCollection Params):base(AccessData,
                                                                                                        CommandName,
                                                                                                        Params)
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
            
           /* s = new Sound();
            s.artist = base.Result[["artist"].InnerText;
            s.title = nodes[i]["title"].InnerText;
            s.duration = (int.Parse(nodes[i]["duration"].InnerText));
            s.url = nodes[i]["url"].InnerText;
            sounds.Add(s);*/

            return base.Result;
        }

        public override void ExecuteNonQuery()
        {
            base.ExecuteCommand();
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
