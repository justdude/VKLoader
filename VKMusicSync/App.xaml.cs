using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;

namespace VKSyncMusic
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        public void OnStartup(object sender, StartupEventArgs e)
        {
            MusicSync musicSync = new MusicSync();
            musicSync.Show();
        }
    }
}
