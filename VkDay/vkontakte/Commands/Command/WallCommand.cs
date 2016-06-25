using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Xml;
using System.Web;
using VKLib.Model;

namespace VKLib
{
    public class WallCommand : BaseCommand<SoundBase>
    {

        public WallCommand(string CommandName, NameValueCollection Params):base(CommandName,Params)
        {
        }

        public WallCommand(AccessDataInfo accessData, string CommandName, NameValueCollection Params)
            : base(accessData, CommandName, Params)
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
