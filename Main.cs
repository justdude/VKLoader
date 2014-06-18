using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace VKMusicSync
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        public static Main instance = null;

        private void button1_Click(object sender, EventArgs e)
        {
            Messages messages = new Messages();
            messages.Owner = this;
            this.Hide();
            messages.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Wall wall = new Wall();
            wall.Owner = this;
            this.Hide();
            wall.Show();
        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
<<<<<<< HEAD
            this.Owner.Show();
            this.Hide();
=======
          /*  this.Owner.Show();
            this.Hide();*/
>>>>>>> origin/vanchik
        }


        bool onStart = false;
        private void Main_Load(object sender, EventArgs e)
        {
            if (!onStart) {
<<<<<<< HEAD
                Program.vk = new VKontakte1.VKApi(Form1.token.id, Form1.token.accesToken);
                onStart = true;
            }

=======
                
                onStart = true;
            }


>>>>>>> origin/vanchik
        }

        private void button3_Click(object sender, EventArgs e)
        {
<<<<<<< HEAD
=======
             AlbumForm albumForm = new AlbumForm();
             //albumForm.Owner = (Window])this;
             this.Hide();
             albumForm.Show();
<<<<<<< HEAD
>>>>>>> origin/vanchik
            Albums albums = new Albums();
=======
            /*Albums albums = new Albums();
>>>>>>> origin/vanchik
            albums.Owner = this;
            this.Hide();
            albums.Show();*/
        }
<<<<<<< HEAD

        private void Main_FormClosed(object sender, FormClosedEventArgs e)
        {
       /*     Form1 form1 = (Form1)this.Owner;
            //Пытаемся разлогиниться

            form1.webBrowser1.Navigate("https://login.vk.com/?act=logout&hash=14466908cac58bbe4b&_origin=http://vk.com");
            //Ждем,пока все операции завершатся
            while (form1.webBrowser1.IsBusy == true)
            {
                Application.DoEvents();
            }
            //Переходим на страничку входа
            form1.webBrowser1.Navigate("http://oauth.vkontakte.ru/authorize?client_id=myappid&scope=friends,groups&redirect_uri=http://oauth.vk.com/blank.html&display=page&response_type=token");
        */
        }

    }
=======
        Logoff form1;
        private void Main_FormClosed(object sender, FormClosedEventArgs e)
        {
            LogOff(this,this.OnLoggOff);
        }

        private void OnLoggOff(object sender, WebBrowserDocumentCompletedEventArgs e) {
            //on logg off
        }

        private void button4_Click(object sender, EventArgs e)
        {
            AudioForm form = new AudioForm();
            form.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            LogOff(this,OnLoggOff);
        }

        public static void LogOff(Object sender, WebBrowserDocumentCompletedEventHandler e)
        {
            //this.Close();
            //Logoff form1 = new Logoff();

            WebBrowser browser = new WebBrowser();
            browser.Navigate("http://vk.com/login.php?op=logout");
            browser.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(e);
            
        }

    }//class
>>>>>>> origin/vanchik
}
