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
using System.Web;
using vkAPI;
namespace VK
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        public static int appId = 3793209;
        public static int scope = (int)(VkontakteScopeList.audio |
                                       // VkontakteScopeList.docs | 
                                        VkontakteScopeList.friends | 
                                        VkontakteScopeList.link | 
                                        VkontakteScopeList.messages | 
                                        //VkontakteScopeList.notes | 
                                        VkontakteScopeList.notify | 
                                        //VkontakteScopeList.offers | 
                                        //VkontakteScopeList.pages | 
                                        VkontakteScopeList.photos | 
                                        //VkontakteScopeList.questions | 
                                        //VkontakteScopeList.video | 
                                        VkontakteScopeList.wall);

        private void Form1_Load(object sender, EventArgs e)
        {
            //webBrowser1.Navigate(StringHelper.ClearSpaces(tok));
            webBrowser1.Navigate(String.Format("https://oauth.vk.com/authorize?client_id={0}&scope={1}&redirect_uri=https://oauth.vk.com/blank.html&display=popup&response_type=token", Login.appId, Login.scope));
        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            string url = e.Url.ToString();
            if (url.Length>0)
            {
                if (url.IndexOf("access_token") > 0)
                {
                    int id = 0;
                    string token = " ";
                    Parse(url, out id, out token);
                    
                    Program.vk = new vkAPI.VKApi(id, token);

                    Main main = new Main();
                    main.Owner = this;
                    main.Show();
                    this.Hide();

                }
                else if (url.IndexOf("access_denied") > 0)
                {
                    MessageBox.Show("Acces denied");
                }

            }

        }

        private void Parse(string url,out int id,out string token) 
        {
            token = "";
            id = 0;
            Regex reg = new Regex(@"(?<name>[\w\d\x5f]+)=(?<value>[^\x26\s]+)",
                      RegexOptions.IgnoreCase | RegexOptions.Singleline);
            var mathes = reg.Matches(url);
            foreach (Match m in mathes)
                if (m.Groups["name"].Value == "access_token")
                    token = m.Groups["value"].Value;
                else if (m.Groups["name"].Value == "user_id")
                    id = int.Parse(m.Groups["value"].Value);
        }
        
    }
}
