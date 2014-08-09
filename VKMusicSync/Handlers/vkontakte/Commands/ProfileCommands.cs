using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Xml;
using System.Net;
using VKMusicSync.Model;

namespace vkontakte
{
    public class ProfileCommands
    {
        #region Profile
        public Profile GetUser(int uid)
        {
            NameValueCollection Params = new NameValueCollection();
            string CommandName;
            Params.Add("uid", uid.ToString());
            Params.Add("fields", "uid, first_name, last_name, nickname, sex, bdate, city, country" +
                           "photo, photo_medium, photo_big");
            CommandName = "users.get";

            var command = new ProfileCommand(CommandName, Params);
            command.ExecuteCommand();
            return command.Fill().FirstOrDefault<Profile>();
        }
        #endregion
    }
}
