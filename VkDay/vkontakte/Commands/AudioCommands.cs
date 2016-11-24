using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Xml;
using System.Net;
using VKLib.Model;

namespace VKLib
{
    public enum Genre
    {
        Rock = 1,
        Pop = 2,
        RapHip_Hop = 3,
        EasyListening = 4,
        DanceHouse = 5,
        Instrumental = 6,
        Metal = 7,
        Alternative = 8,
        Dubstep = 9,
        JazzBlues = 10,
        DrumBass = 11,
        Trance = 12,
        Chanson = 13,
        Ethnic = 14,
        AcousticVocal = 15,
        Reggae = 16,
        Classical = 17,
        IndiePop = 18,
        Speech = 19,
        ElectropopDisco = 20,
        Other = 21
    }

    public class AudioCommands
    {

        public DownloadProgressChangedEventHandler OnCommandExecuting
        {
            get;
            set;
        }

        #region Audio
        public int GetAudioCount(AccessDataInfo accessData, int uid, bool isGroup)
        {
            NameValueCollection Params = new NameValueCollection();
            string CommandName;
            Params.Add("owner_id", ((isGroup) ? "-" : "") + uid);
            Params.Add("fields", "uid,aid");
            CommandName = "audio.getCount";
            var command = new AudiosCommand(accessData, CommandName, Params);
            command.ExecuteCommand();
            return command.GetCount();

        }

		public List<SoundBase> GetAudioFromUser(AccessDataInfo accessData, int uid, bool isGroup, int offset, int counts)
        {
            NameValueCollection Params = new NameValueCollection();
            string CommandName;
            Params.Add("owner_id", ((isGroup) ? "-" : "") + uid);
            Params.Add("offset", offset.ToString());
            Params.Add("count", counts.ToString());
           // Params.Add("fields", "owner_id,offset,count");
            CommandName = "audio.get";
			var command = new AudiosCommand(accessData, CommandName, Params);
            command.ExecuteCommand();
            return command.Fill();
        }

        public List<SoundBase> GetAudioRecomendation(int user_id, int audioId,  int offset, bool isShuflem, int count)
        {
            NameValueCollection Params = new NameValueCollection();
            string CommandName;

            string target_audio = user_id.ToString() + "_" + audioId;
            Params.Add("target_audio", target_audio);
            Params.Add("offset", offset.ToString());
            Params.Add("count", count.ToString());
            Params.Add("shuffle", (isShuflem) ? "1" : "0");

            Params.Add("fields", "target_audio,offset,count,shuffle");
            CommandName = "audio.getRecommendations";

            var command = new AudiosCommand(CommandName, Params);
            command.ExecuteCommand();
            return command.Fill();
        }


        public List<SoundBase> GetAudioRecomendation(int user_id, int offset, bool isShuflem, int count)
        {
            NameValueCollection Params = new NameValueCollection();
            string CommandName;

            Params.Add("user_id", user_id.ToString());
            Params.Add("offset", offset.ToString());
            Params.Add("count", count.ToString());
            Params.Add("shuffle", (isShuflem) ? "1" : "0");

            Params.Add("fields", "user_id,offset,count,shuffle");
            CommandName = "audio.getRecommendations";

            var command = new AudiosCommand(CommandName, Params);
            command.ExecuteCommand();
            return command.Fill();
        }

        public List<SoundBase> GetAudioPopular(bool isOnly_eng, int genre_id, int offset, int count)
        {
            NameValueCollection Params = new NameValueCollection();
            string CommandName;

            Params.Add("only_eng", (isOnly_eng) ? "1" : "0");
            Params.Add("genre_id", genre_id.ToString());
            Params.Add("offset", offset.ToString());
            Params.Add("count", count.ToString());

            Params.Add("fields", "only_eng,genre_id,offset,count");
            CommandName = "audio.getPopular";

            var command = new AudiosCommand(CommandName, Params);
            command.ExecuteCommand();
            return command.Fill();
        }

        /*public List<SoundBase> GetAudioFromUserWithLast(int uid, bool isGroup, int offset, int count, DotLastFm.LastFmApi api)
        {
            NameValueCollection Params = new NameValueCollection();
            string CommandName;
            Params.Add("owner_id", ((isGroup) ? "-" : "") + uid);
            Params.Add("offset", offset.ToString());
            Params.Add("count", count.ToString());
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

        public AudioUploadedInfo GetUploadServer(string path, string fileName)
        {
            NameValueCollection Params = new NameValueCollection();
            string CommandName;
            CommandName = "audio.getUploadServer";

            AudioUploadComman command = new AudioUploadComman(CommandName, Params);
            command.ExecuteCommand();

            return command.UploadAudio(path, fileName);//DOWNLOAD PROGRESS!!!!!
        }

        public string SaveAudio(VKLib.Model.AudioUploadedInfo info)
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
            return VKLib.Handlers.Reqeust.POST("https://api.API.com/method/audio.save", paramsAndtoken);
        }

        #endregion

    }
}
