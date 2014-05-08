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
namespace VKMusicSync
{
    public partial class Login : Form
    {
        public Login()
        {
            loaded = true;
            InitializeComponent();
            SetSetting();
        }

        public static bool loaded = false;

        public static int appId = 3793209;//4345193
        public static int scope = (int)(VkontakteScopeList.audio |
                                        VkontakteScopeList.friends | 
                                        VkontakteScopeList.link | 
                                        //VkontakteScopeList.messages | 
                                        //VkontakteScopeList.notify |
                                        VkontakteScopeList.photos | 
                                        // VkontakteScopeList.docs | 
                                        //VkontakteScopeList.notes | 
                                        //VkontakteScopeList.offers | 
                                        //VkontakteScopeList.pages | 
                                        //VkontakteScopeList.questions | 
                                        //VkontakteScopeList.video | 
                                        VkontakteScopeList.wall);

        private void Form1_Load(object sender, EventArgs e)
        {
            webBrowser1.Navigate(String.Format("https://oauth.vk.com/authorize?client_id={0}&scope={1}&redirect_uri=https://oauth.vk.com/blank.html&display=popup&v=5.0&response_type=token", Login.appId, Login.scope));
        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            string url = e.Url.ToString();
            if (url.Length>0)
            {
                if (url.IndexOf("access_token") > 0)
                {
                    Program.vk = new vkAPI.VKApi(url);

                    AudioForm form = new AudioForm();
                    this.Hide();
                    form.Show();

                }
                else if (url.IndexOf("access_denied") > 0)
                {
                    MessageBox.Show("Acces denied");
                }

            }

        }

        


        private void SetSetting()
        {
            Properties.Settings.Default.ProgramPath = Environment.CurrentDirectory;
            Properties.Settings.Default.SettingPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        }

        private void Login_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}
