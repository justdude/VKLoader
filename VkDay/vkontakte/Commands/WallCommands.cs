using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Xml;
using System.Net;
using VKMusicSync.Model;
using VKMusicSync.Handlers.vkontakte.Commands;
//using UrlDecode;

namespace vkontakte
{
    public class WallCommands
    {
        public DownloadProgressChangedEventHandler OnCommandExecuting
        {
            get;
            set;
        }

        public int Post(int owner_id, string message)
        {
            NameValueCollection Params = new NameValueCollection();
            string CommandName;
            Params.Add("owner_id", owner_id.ToString());
            Params.Add("message", message);
            Params.Add("fields", "owner_id,message");
            CommandName = "wall.post";
            var command = new WallCommand(CommandName, Params);
            command.ExecuteCommand();
            return command.GetPostAnswer();

        }

        public int Post(int owner_id, string message, string attachment, string captcha_sid, string captcha_key)
        {
            NameValueCollection Params = new NameValueCollection();
            string CommandName;
            Params.Add("owner_id", owner_id.ToString());
            Params.Add("message", message);

            string added = string.Empty;

            CommandHelper.AddIfExict(Params, "attachment", Uri.EscapeUriString(attachment), ref added);
            if (CommandHelper.IsExist(added))
            {
                var res = CommandHelper.AddIfExict(Params, "captcha_sid", captcha_sid, ref added);
                if (res)
                    CommandHelper.AddIfExict(Params, "captcha_key", captcha_key, ref added);
            }

            Params.Add("fields", "owner_id,message"+ added);
            CommandName = "wall.post";
            var command = new WallCommand(CommandName, Params);
            command.ExecuteCommand();
            return command.GetPostAnswer();

        }
    }
}
