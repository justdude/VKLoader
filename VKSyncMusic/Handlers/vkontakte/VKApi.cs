﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
using System.Xml;
using System.Threading;
using System.Collections;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using VKSyncMusic.Model;

namespace vkontakte
{
    public class APIManager
    {
        public static VKApi vk = null;

        public static AccessData AccessData { get; set; }
        public static Profile Profile { get; set; }
    }

    public class VKApi
    {
        public int UserId = 0;
        public string AccessToken = "";


        public VKApi(AccessData data)
        {
            this.UserId = data.UserId;
            this.AccessToken = data.AccessToken;
        }

        public VKApi(string url)
        {
            Parse(url, out this.UserId, out this.AccessToken);
        }

        private void Parse(string url, out int id, out string token)
        {
            token = "";
            id = 0;
            Regex reg = new Regex(@"(?<CommandName>[\w\d\x5f]+)=(?<value>[^\x26\item]+)",
                      RegexOptions.IgnoreCase | RegexOptions.Singleline);
            var mathes = reg.Matches(url);
            foreach (Match m in mathes)
                if (m.Groups["CommandName"].Value == "access_token")
                    token = m.Groups["value"].Value;
                else if (m.Groups["CommandName"].Value == "user_id")
                    id = int.Parse(m.Groups["value"].Value);
        }


        public string GetDataFromXMLNode(XmlNode node) {
            if (node == null || String.IsNullOrEmpty(node.InnerText)) {
                return "нету данных";
            }
            else return node.InnerText;
        }

        private XmlDocument ExecuteCommand(string name, NameValueCollection param)
        {
            XmlDocument result = new XmlDocument();
            System.Net.WebClient downloader = new System.Net.WebClient();

            string str = MakeQueryString(name, AccessToken, param);

            downloader.DownloadProgressChanged += downloadFileProgressChanged;
            downloader.DownloadFileAsync(new Uri(str), name);
            while (true)
                if (!downloader.IsBusy) break;
            FileStream fileStream = new FileStream(name, FileMode.Open, FileAccess.Read);
            
            result.Load(fileStream);

            downloader.DownloadProgressChanged -= downloadFileProgressChanged;
            downloader.Dispose();
            fileStream.Dispose();
            
            return result;
        }

        private DownloadProgressChangedEventHandler downloadFileProgressChanged;

        public void SetDownloadXMLProgressChanged(DownloadProgressChangedEventHandler value)
        {
            this.downloadFileProgressChanged = new DownloadProgressChangedEventHandler(value);
        }


        private string MakeQueryString(string name, string accessToken, NameValueCollection param)
        {
            return String.Format("https://api.vkontakte.ru/method/{0}.xml?access_token={1}&{2}",
                                    name,
                                    accessToken,
                                    String.Join("&", SelectItem(param)));
        }

        private XmlDocument ExecuteCommand(string name)
        {
            XmlDocument result = new XmlDocument();
            result.Load(String.Format("https://api.vkontakte.ru/method/{0}.xml?access_token={1}&{2}",
                        name,
                        AccessToken));
            return result;
        }

        string[] SelectItem(NameValueCollection qs) { 
        //from item in Params.AllKeys select item + "=" + Params[item]
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

        public XmlDocument GetAudioUpload()
        {
            return ExecuteCommand("audio.getUploadServer");
        }

        public XmlDocument GetPhotosUploadToWall(int uid, bool isGroup)
        {
            NameValueCollection qs = new NameValueCollection();
            qs.Add("group_id", ((isGroup) ? "-" : "") + uid.ToString());
            qs.Add("fields", "group_id");
            return ExecuteCommand("photos.getWallUploadServer", qs);
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
        docs = 131072,
        /// <summary>
        /// Доступ к API в любое время со стороннего сервера (при использовании этой опции параметр expires_in, возвращаемый вместе с access_token, содержит 0 — токен бессрочный).
        /// </summary>
        offline = 65536
    }
    #endregion

}
