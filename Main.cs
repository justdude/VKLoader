using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace VK
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

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
            this.Owner.Show();
            this.Hide();
        }


        bool onStart = false;
        private void Main_Load(object sender, EventArgs e)
        {
            if (!onStart) {
                Program.vk = new VKontakte1.VKApi(Form1.token.id, Form1.token.accesToken);
                onStart = true;
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            Albums albums = new Albums();
            albums.Owner = this;
            this.Hide();
            albums.Show();
        }

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
}
