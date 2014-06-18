using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace vkontakte
{
    class XMLNodeHelper
    {
        public static string TryParse(XmlNode node,string name)
        {
            if (node.ChildNodes.Count > 0)
            {
                if (node[name]!=null)
                {
                    return node[name].InnerText;
                }
            }
            return string.Empty;
        }
    }
}
