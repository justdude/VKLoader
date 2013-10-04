using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
using System.Xml;
using System.Threading;
using System.Collections;

namespace VKontakte1
{
    class VKApi
    {
        public int UserId = 0;
        public string AccessToken = "";

        public class Wall
        {
            public int id { get; set; }
            public int from_id { get; set; }
            public int to_id { get; set; }
            public int date { get; set; }
            public string text { get; set; }
            public Object comments { get; set; }
            public Object likes { get; set; }
            public Object reposts { get; set; }
        }
        public class Album
        {
            public int id { get; set; }
            public string title { get; set; }
            public string comment { get; set; }
            public string picture { get; set; }
        }

        public VKApi(string accessToken)
        {
            this.AccessToken = accessToken;
            accessToken = AccessToken;
        }
        public VKApi(int userId, string accessToken)
        {
            this.UserId = userId;
            this.AccessToken = accessToken;
        }
        public string GetDataFromXMLNode(XmlNode node) {
            if (node == null || String.IsNullOrEmpty(node.InnerText)) {
                return "нету данных";
            }
            else return node.InnerText;
        }

        private XmlDocument ExecuteCommand(string name, NameValueCollection qs)
        {
            XmlDocument result = new XmlDocument();
            
          //  System.Windows.Forms.MessageBox.Show("" + String.Join("&", from item in qs.AllKeys select item + "=" + qs[item]));
            result.Load(String.Format("https://api.vkontakte.ru/method/{0}.xml?access_token={1}&{2}", name, AccessToken, String.Join("&", SelectItem(qs))));
            return result;
        }

        string[] SelectItem(NameValueCollection qs) { 
        //from item in qs.AllKeys select item + "=" + qs[item]
           List<string> str=new List<string>();
           foreach (string obj in qs.AllKeys)
               str.Add(obj+"="+qs[obj]);
               return str.ToArray();
        }

        public XmlDocument GetProfile(int uid)
        {
            NameValueCollection qs = new NameValueCollection();
            qs["uid"] = uid.ToString();
            qs["fields"] = "uid, first_name, last_name, nickname, sex, bdate (birthdate), city, country, timezone,"+
                           "photo, photo_medium, photo_big, photo_rec, connections";
            return ExecuteCommand("users.get", qs);
        }
        public XmlDocument GetFriends(int uid)
        {
            NameValueCollection qs = new NameValueCollection();
            qs["uid"] = uid.ToString();
            qs["fields"] = "uid,first_name,last_name,nickname,domain,sex,bdate,city,country,timezone,photo,has_mobile,rate,contacts,education,online";
            return ExecuteCommand("friends.get", qs);
        }

        public void PublishOnTheWall(int uid, string message)
        {
            NameValueCollection collection = new NameValueCollection();
            collection["owner_id"] = uid.ToString();
            collection["message"] = message;
            ExecuteCommand("wall.post", collection);
        }

        public XmlDocument GetAllAlbums(int uid)
        {
            NameValueCollection qs = new NameValueCollection();
            qs["uid"] = uid.ToString();
            qs["fileds"] = "uid,need_covers";
            return ExecuteCommand("photos.getAlbums", qs);
        }

        public XmlDocument GetPhotosFromPage(int uid)
        {
            NameValueCollection qs = new NameValueCollection();
            qs["uid"] = uid.ToString();
            qs["fileds"] = "uid,extended";
            return ExecuteCommand("photos.getProfile", qs);
        }

        public XmlDocument GetPhotosFromAlbum(int uid, int album) 
        {
            NameValueCollection qs = new NameValueCollection();
            qs["owner_id"] = uid.ToString();
            qs["aid"] = album.ToString();
            qs["fields"] = "uid,aid";
            return ExecuteCommand("photos.get", qs);
        }

    }
}
