using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Web;
using System.Windows.Forms;

using VK.Model;
using VK.Handlers;
using VK.vkAPI;
namespace VK.Auth
{
    public class AuthWindowed
    {
        public readonly static int appId = 3793209;//4345193
        public readonly static int scope = (int)(VkontakteScopeList.audio |
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

        public WebBrowser webBrowser { get; private set; }

        public delegate void OnInitDelegate();
        public OnInitDelegate OnInit;

        public AuthWindowed(WebBrowser browser)
        {
            this.webBrowser = browser;
            webBrowser.DocumentCompleted += this.webBrowser_DocumentCompleted;
        }

        public void Navigate(object sender, EventArgs e)
        {
            webBrowser.Navigate(String.Format("https://oauth.vk.com/authorize?client_id={0}&scope={1}&redirect_uri=https://oauth.vk.com/blank.html&display=popup&v=5.0&response_type=token", appId, scope));
        }

        private void webBrowser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            string url = e.Url.ToString();
            if (url.Length>0)
            {
                if (url.IndexOf("access_token") > 0)
                {
                    APIManager.vk = new VKApi(url);
                    if (OnInit != null) OnInit();
                }
                else if (url.IndexOf("access_denied") > 0)
                {
                    //MessageBox.Show("Acces denied");
                }

            }

        }

    }
}
