using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace VKLib
{
    public class AccessData
    {
        public int UserId { get; set; }
        public string AccessToken { get; set; }

        public AccessData(int userId, string accessToken)
        {
            this.UserId = userId;
            this.AccessToken = accessToken;
        }

        public AccessData(string url)
        {
            Parse(url);
        }

        private void Parse(string url)
        {
            AccessToken = "";
            UserId = 0;
            Regex reg = new Regex(@"(?<name>[\w\d\x5f]+)=(?<value>[^\x26\s]+)",
                      RegexOptions.IgnoreCase | RegexOptions.Singleline);
            var mathes = reg.Matches(url);
            foreach (Match m in mathes)
                if (m.Groups["name"].Value == "access_token")
                    AccessToken = m.Groups["value"].Value;
                else if (m.Groups["name"].Value == "user_id")
                    UserId = int.Parse(m.Groups["value"].Value);
        }
    }
}
