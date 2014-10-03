﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Xml;
using System.Net;
using VKMusicSync.Model;

namespace vkontakte
{
    public class AudioCommands
    {
        public DownloadProgressChangedEventHandler OnCommandExecuting
        {
            get;
            set;
        }

        #region Audio
        public int GetAudioCount(int uid, bool isGroup)
        {
            NameValueCollection Params = new NameValueCollection();
            string CommandName;
            Params.Add("owner_id", ((isGroup) ? "-" : "") + uid);
            Params.Add("fields", "uid,aid");
            CommandName = "audio.getCount";
            var command = new AudiosCommand(CommandName, Params);
            command.ExecuteCommand();
            return command.GetCount();

        }

        public List<Sound> GetAudioFromUser(int uid, bool isGroup, int offset, int counts)
        {
            NameValueCollection Params = new NameValueCollection();
            string CommandName;
            Params.Add("owner_id", ((isGroup) ? "-" : "") + uid);
            Params.Add("offset", offset.ToString());
            Params.Add("count", counts.ToString());
            Params.Add("fields", "owner_id,offset,count");
            CommandName = "audio.get";
            var command = new AudiosCommand(CommandName, Params);
            command.ExecuteCommand();
            return command.Fill();
        }

        /*public List<Sound> GetAudioFromUserWithLast(int uid, bool isGroup, int offset, int counts, DotLastFm.LastFmApi api)
        {
            NameValueCollection Params = new NameValueCollection();
            string CommandName;
            Params.Add("owner_id", ((isGroup) ? "-" : "") + uid);
            Params.Add("offset", offset.ToString());
            Params.Add("count", counts.ToString());
            Params.Add("fields", "owner_id,offset,count");
            CommandName = "audio.get";
            var command = new AudiosCommand(CommandName, Params);
            if (OnCommandExecuting!=null)
                command.OnCommandExecuting += OnCommandExecuting;
            command.ExecuteCommand();
            var res = command.FillWithAddons(api);
            return res;
        }*/


        #region UNWORKED!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        public AudiosCommand SendAudioToUserWall(int ownerId, int audioId)
        {
            NameValueCollection Params = new NameValueCollection();
            string CommandName;
            Params.Add("owner_id", ownerId.ToString());
            Params.Add("audio_id", audioId.ToString());
            Params.Add("fields", "owner_id,audio_id");
            CommandName = "audio.get";
            return new AudiosCommand(CommandName, Params);
        }

        public AudiosCommand SendAudioToGroupWall(int groupId, int audioId)
        {
            NameValueCollection Params = new NameValueCollection();
            string CommandName;
            Params.Add("owner_id", groupId.ToString());
            Params.Add("audio_id", audioId.ToString());
            Params.Add("fields", "owner_id,audio_id");
            CommandName = "audio.get";
            return new AudiosCommand(CommandName, Params);
        }

        #endregion


        public List<Sound> GetAudioRecomendation(int uid, bool isGroup, int offset, int counts)
        {
            NameValueCollection Params = new NameValueCollection();
            string CommandName;
            Params.Add("owner_id", ((isGroup) ? "-" : "") + uid.ToString());
            Params.Add("offset", offset.ToString());
            Params.Add("count", counts.ToString());
            Params.Add("fields", "uid,aid");
            CommandName = "audio.getRecommendations";

            var command = new AudiosCommand(CommandName, Params);
            command.ExecuteCommand();
            return command.Fill();
        }

        public AudioUploadedInfo GetUploadServer(string path, string fileName)
        {
            NameValueCollection Params = new NameValueCollection();
            string CommandName;
            CommandName = "audio.getUploadServer";

            AudioUploadComman command = new AudioUploadComman(CommandName, Params);
            command.ExecuteCommand();

            return command.UploadAudio(path, fileName);//DOWNLOAD PROGRESS!!!!!
        }

        public string SaveAudio(VKMusicSync.Model.AudioUploadedInfo info)
        {
            NameValueCollection Params = new NameValueCollection();
            string CommandName;
            Params.Add("server", info.Server);
            Params.Add("audio", Uri.EscapeUriString(info.Audio));
            Params.Add("hash", info.Hash);
            Params.Add("title", info.Title);
            Params.Add("artist", info.Artist);
            //Params.Add("v", "5.21");
            CommandName = "audio.save";
            AudioUploadComman command = new AudioUploadComman(CommandName, Params);
            var paramsAndtoken = @"" + command.GetParamsWithToken();
            return VKMusicSync.Handlers.Reqeust.POST("https://api.vk.com/method/audio.save", paramsAndtoken);
        }

        /*public static AudioUploadComman SaveAudio(string server, string audio, string hash)
        {
            NameValueCollection Params = new NameValueCollection();
            string CommandName;
            Params.Add("server", server.ToString());
            Params.Add("audio", audio);
            Params.Add("hash", hash);
            //Params.Add("fields", "audio,hash,server");
            CommandName = "audio.save";
            return new AudioUploadComman(CommandName, Params);
        }*/
        #endregion

    }
}