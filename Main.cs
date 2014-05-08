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
          /*  this.Owner.Show();
            this.Hide();*/
        }


        bool onStart = false;
        private void Main_Load(object sender, EventArgs e)
        {
            if (!onStart) {
                
                onStart = true;
            }


        }

        private void button3_Click(object sender, EventArgs e)
        {
             AlbumForm albumForm = new AlbumForm();
             //albumForm.Owner = (Window])this;
             this.Hide();
             albumForm.Show();
            /*Albums albums = new Albums();
            albums.Owner = this;
            this.Hide();
            albums.Show();*/
        }
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
}
