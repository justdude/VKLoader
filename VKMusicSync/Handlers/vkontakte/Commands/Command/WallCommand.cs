using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Xml;
using System.Web;
using VKMusicSync.Model;

namespace vkontakte
{
    public class WallCommand : BaseCommand<Sound>
    {

        public WallCommand(string CommandName, NameValueCollection Params):base(CommandName,Params)
        {
        }

        public WallCommand(AccessData AccessData, string CommandName, NameValueCollection Params)
            : base(AccessData, CommandName, Params)
        {
        }

        public int GetPostAnswer()
        {
            XmlDocument doc = base.xmlRes;
            var nodes = doc["response"];
            return int.Parse(nodes["post_id"].InnerText);
        }



    }
}
