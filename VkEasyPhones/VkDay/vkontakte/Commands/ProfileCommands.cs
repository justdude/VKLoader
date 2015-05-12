using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Xml;
using System.Net;
using VkDay.Model;

namespace VkDay
{
    public class ProfileCommands
    {
			const string Fields =	"uid, first_name, " +
														"last_name, nickname, sex, contacts, relation, " +
														"bdate, city, country, " + 
														"photo, photo_medium, photo_big";

        #region Profile
        public Profile GetUser(int uid)
        {
            NameValueCollection Params = new NameValueCollection();
            string CommandName;
            Params.Add("uid", uid.ToString());
            Params.Add("fields", Fields);
            CommandName = "users.get";

            var command = new ProfileCommand(CommandName, Params);
            command.ExecuteCommand();
            return command.Fill().FirstOrDefault<Profile>();
        }

				public List<Profile> Search(string target, int count, int offset)
				{
					NameValueCollection Params = new NameValueCollection();
					string CommandName;
					int countRes = count >= 1000 ? 1000 : count;

					Params.Add("q", target);
					Params.Add("fields", Fields);
					Params.Add("count", countRes.ToString());
					Params.Add("offset", offset.ToString());

					CommandName = "users.search";

					var command = new ProfileCommand(CommandName, Params);
					command.ExecuteCommand();
					return command.Search();
				}
        #endregion
    }
}
