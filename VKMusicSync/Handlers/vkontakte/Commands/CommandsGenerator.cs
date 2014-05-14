using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Xml;


namespace vkontakte
{
    class CommandsGenerator
    {
        #region Profile
        public static ProfileCommand GetUsers(int uid)
        {
            NameValueCollection Params = new NameValueCollection();
            string CommandName;
            Params.Add("uid",uid.ToString());
            Params.Add("fields","uid, first_name, last_name, nickname, sex, bdate, city, country" +
                           "photo, photo_medium, photo_big");
            CommandName = "users.get";
            return new ProfileCommand(CommandName, Params);
        }
        #endregion

        #region Audio
        public static AudioCommand GetAudioCountFromUser(int uid, bool isGroup)
        {
            NameValueCollection Params = new NameValueCollection();
            string CommandName;
            Params.Add("owner_id", ((isGroup) ? "-" : "") + uid);
            Params.Add("fields", "uid,aid");
            CommandName = "audio.getCount";
            return new AudioCommand(CommandName, Params);
        }

        public static AudioCommand GetAudioFromUser(int uid, bool isGroup, int offset, int counts)
        {
            NameValueCollection Params = new NameValueCollection();
            string CommandName;
            Params.Add("owner_id", ((isGroup) ? "-" : "") + uid);
            Params.Add("offset", offset.ToString());
            Params.Add("count", counts.ToString());
            Params.Add("fields", "owner_id,offset,count");
            CommandName = "audio.get";
            return new AudioCommand(CommandName, Params);
        }

        public static AudioCommand SendAudioToUserWall(int ownerId, int audioId)
        {
            NameValueCollection Params = new NameValueCollection();
            string CommandName;
            Params.Add("owner_id", ownerId.ToString());
            Params.Add("audio_id", audioId.ToString());
            Params.Add("fields", "owner_id,audio_id");
            CommandName = "audio.get";
            return new AudioCommand(CommandName, Params);
        }

        public static AudioCommand SendAudioToGroupWall(int groupId, int audioId)
        {
            NameValueCollection Params = new NameValueCollection();
            string CommandName;
            Params.Add("owner_id", groupId.ToString());
            Params.Add("audio_id", audioId.ToString());
            Params.Add("fields", "owner_id,audio_id");
            CommandName = "audio.get";
            return new AudioCommand(CommandName, Params);
        }

        public static AudioCommand GetAudioRecomendation(int uid, bool isGroup, int offset, int counts)
        {
            NameValueCollection Params = new NameValueCollection();
            string CommandName;
            Params.Add("owner_id", ((isGroup) ? "-" : "") + uid.ToString());
            Params.Add("offset", offset.ToString());
            Params.Add("count", counts.ToString());
            Params.Add("fields", "uid,aid");
            CommandName = "audio.getRecommendations";
            return new AudioCommand(CommandName, Params);
        }

        public static AudioUploadComman GetUploadServer()
        {
            NameValueCollection Params = new NameValueCollection();
            string CommandName;
            CommandName = "audio.getUploadServer";
            return new AudioUploadComman(CommandName, Params);
        }

        public static AudioUploadComman SaveAudio(VKMusicSync.Model.AudioUploadedInfo info)
        {
            NameValueCollection Params = new NameValueCollection();
            string CommandName;
            Params.Add("server", info.Server);
            Params.Add("audio", info.Audio);
            Params.Add("hash", info.Hash);
            Params.Add("title", "title");
            Params.Add("artist", "artist");
            //Params.Add("v", "5.21");
            CommandName = "audio.save";
            return new AudioUploadComman(CommandName, Params);
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
