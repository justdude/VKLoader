using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using VKLib;

namespace VKLib
{
    public partial class Auth : Form
    {
        public AuthWindowed auth;

        public Auth()
        {
            InitializeComponent();
            this.auth = new AuthWindowed(webBrowser1);
        }

        private void Auth_Load(object sender, EventArgs e)
        {
            this.auth.Navigate(sender, e);
        }

    }
}
