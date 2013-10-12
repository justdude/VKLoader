using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

using System.Text.RegularExpressions;

using System.Globalization;
using Microsoft.Win32;

namespace VK
{
    public partial class Autorisation : Form
    {
        public Autorisation()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            webBrowser1.Navigate(KickAssString(Autorisation.tok));
        }

        String KickAssString(String str)
        {
            String st="";
            foreach (var c in str)
                if (c!=' ') st += c;
            return st;

        }

        static string tok= @"https://oauth.vk.com/authorize?
                    client_id=3793209&
                    scope=friends,photos,wall&
                    redirect_uri=https://oauth.vk.com/blank.html&
                    display=page&
                    response_type=token";

        /*
         http://REDIRECT_URI#access_token= 533bacf01e11f55b536a565b57531ad114461ae8736d6506a3&expires_in=86400&user_id=8492 
         */
        public class UserInfo {
           public int id;
           public string accesToken;
          public UserInfo()
           {
               id = 0;
               accesToken = "";
           }

        }
        public static UserInfo token=new UserInfo();
        string url="";
        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            url = e.Url.ToString();
            if (url.Length>0)
            {
                if (url.IndexOf("access_token") > 0)
                {
                    Regex reg = new Regex(@"(?<name>[\w\d\x5f]+)=(?<value>[^\x26\s]+)", RegexOptions.IgnoreCase | RegexOptions.Singleline);
                    var mathes = reg.Matches(url);
                    foreach (Match m in mathes)
                           if (m.Groups["name"].Value == "access_token")
                           {
                               token.accesToken = m.Groups["value"].Value;
                           }
                           else if (m.Groups["name"].Value == "user_id")
                           {
                               token.id =int.Parse(m.Groups["value"].Value);
                           }


                }
                else if (url.IndexOf("access_denied") > 0)
                {
                    return;
                    //вызвать реконект или еще че нить
                }
                Main main = new Main();
                main.Owner = this;
                main.Show();
                this.Hide();
            }

        }

        
    }
}
