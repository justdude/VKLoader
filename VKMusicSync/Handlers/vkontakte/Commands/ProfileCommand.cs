﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Xml;
using VKMusicSync.Model;
namespace vkontakte
{
    public class ProfileCommand : BaseCommand<Profile>
    {

        public Profile Profile { get; private set; }

        public ProfileCommand(string CommandName, NameValueCollection Params)
            : base(CommandName, Params)
        {
        }

        public ProfileCommand(AccessData AccessData, string CommandName, NameValueCollection Params)
            : base(AccessData,
                   CommandName,
                   Params)
        {
        }

        public override List<Profile> ExecuteForList()
        {
            base.ExecuteCommand();
            return this.BindItems(base.Result);
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

        private List<Profile> BindItems(XmlDocument doc)
        {
            List<Profile> data = new List<Profile>();
            var item = new Profile();
            System.Xml.XmlNodeList nodes = doc["response"].ChildNodes;
            for (int i = 0; i < nodes.Count; i++)
            {
                item = new Profile();
                item.uid = int.Parse(nodes[i]["uid"].InnerText);
                item.first_name = nodes[i]["first_name"].InnerText;
                item.last_name = (nodes[i]["last_name"].InnerText);
                item.nickname = XMLNodeHelper.TryParse(nodes[i],"nickname");
                item.sex = nodes[i]["sex"].InnerText;
                item.city = XMLNodeHelper.TryParse(nodes[i],"city");
                item.country = XMLNodeHelper.TryParse(nodes[i],"country");
                item.photo = XMLNodeHelper.TryParse(nodes[i],"photo");
                item.photoBig = XMLNodeHelper.TryParse(nodes[i], "photo_big");
                item.photoMedium = XMLNodeHelper.TryParse(nodes[i], "photo_medium");
                data.Add(item);
            }
            return data;
        }

    }
}
