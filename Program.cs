using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace VKMusicSync
{
    static class Program
    {
<<<<<<< HEAD
        public static VKontakte1.VKApi vk = null;
=======
        public static vkAPI.VKApi vk = null;
>>>>>>> origin/vanchik
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
<<<<<<< HEAD
            Application.Run(new Form1());
=======
            Application.Run(new Login());
>>>>>>> origin/vanchik
        }
    }
}
