using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
using System.Xml;
using System.Threading;
using System.Collections;

namespace vkAPI
{
    class VKApi
    {
        public int UserId = 0;
        public string AccessToken = "";

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
            result.Load(String.Format("https://api.vkontakte.ru/method/{0}.xml?access_token={1}&{2}",
                        name,
                        AccessToken,
                        String.Join("&", SelectItem(qs))));
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
        #region Photos
        public XmlDocument GetAllAlbums(int uid, bool isGroup)
        {
            NameValueCollection qs = new NameValueCollection();
            qs["owner_id"] = ((isGroup) ? "-" : "") + uid.ToString();
            qs["need_covers"] = "1";
            qs["fileds"] = "owner_id,need_covers";
            return ExecuteCommand("photos.getAlbums", qs);
        }

        public XmlDocument GetPhotosFromPage(int uid)
        {
            NameValueCollection qs = new NameValueCollection();
            qs["uid"] = uid.ToString();
            qs["fileds"] = "uid,extended";
            return ExecuteCommand("albums.getProfile", qs);
        }

        public XmlDocument GetPhotosFromAlbum(int owner_id, int album_id) 
        {
            NameValueCollection qs = new NameValueCollection();
            qs["owner_id"] = owner_id.ToString();
            qs["album_id"] = album_id.ToString();
            qs["fields"] = "owner_id,album_id";
            return ExecuteCommand("photos.get", qs);
        }

        public XmlDocument GetAudioCountFromUser(int uid, bool isGroup)
        {
            NameValueCollection qs = new NameValueCollection();
            qs.Add("owner_id", ((isGroup) ? "-" : "") + uid);
            qs.Add("fields", "uid,aid");
            return ExecuteCommand("audio.getCount", qs);
        }

        public XmlDocument GetAudioFromUser(int uid, bool isGroup, int offset,int counts) 
        {
            NameValueCollection qs = new NameValueCollection();
            qs.Add("owner_id", ((isGroup) ? "-" : "") + uid);
            qs.Add("offset",offset.ToString());
            qs.Add("count",counts.ToString());
            qs.Add("fields", "owner_id,offset,count");
            return ExecuteCommand("audio.get", qs);
        }

        public XmlDocument SendAudioToUserWall(int ownerId, int audioId) 
        {
            NameValueCollection qs = new NameValueCollection();
            qs.Add("owner_id", ownerId.ToString());
            qs.Add("audio_id",audioId.ToString());
            qs.Add("fields", "owner_id,audio_id");
            return ExecuteCommand("audio.get", qs);
        }

        public XmlDocument SendAudioToGroupWall(int groupId, int audioId) 
        {
            NameValueCollection qs = new NameValueCollection();
            qs.Add("owner_id", groupId.ToString());
            qs.Add("audio_id",audioId.ToString());
            qs.Add("fields", "owner_id,audio_id");
            return ExecuteCommand("audio.get", qs);
        }

        public XmlDocument GetAudioRecomendation(int uid, bool isGroup, int offset, int counts) 
        {
            NameValueCollection qs = new NameValueCollection();
            qs.Add("owner_id", ((isGroup) ? "-" : "") + uid.ToString());
            qs.Add("offset",offset.ToString());
            qs.Add("count",counts.ToString());
            qs.Add("fields", "uid,aid");
            return ExecuteCommand("audio.getRecommendations", qs);
        }
        #endregion
    }

    #region Глобальные перечисления
    public enum VkontakteScopeList
    {
        /// <summary>
        /// Пользователь разрешил отправлять ему уведомления. 
        /// </summary>
        notify = 1,
        /// <summary>
        /// Доступ к друзьям.
        /// </summary>
        friends = 2,
        /// <summary>
        /// Доступ к фотографиям. 
        /// </summary>
        photos = 4,
        /// <summary>
        /// Доступ к аудиозаписям. 
        /// </summary>
        audio = 8,
        /// <summary>
        /// Доступ к видеозаписям. 
        /// </summary>
        video = 16,
        /// <summary>
        /// Доступ к предложениям (устаревшие методы). 
        /// </summary>
        offers = 32,
        /// <summary>
        /// Доступ к вопросам (устаревшие методы). 
        /// </summary>
        questions = 64,
        /// <summary>
        /// Доступ к wiki-страницам. 
        /// </summary>
        pages = 128,
        /// <summary>
        /// Добавление ссылки на приложение в меню слева.
        /// </summary>
        link = 256,
        /// <summary>
        /// Доступ заметкам пользователя. 
        /// </summary>
        notes = 2048,
        /// <summary>
        /// (для Standalone-приложений) Доступ к расширенным методам работы с сообщениями. 
        /// </summary>
        messages = 4096,
        /// <summary>
        /// Доступ к обычным и расширенным методам работы со стеной. 
        /// </summary>
        wall = 8192,
        /// <summary>
        /// Доступ к документам пользователя.
        /// </summary>
        docs = 131072
    }
    #endregion

}
