using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;

namespace VKMusicSync
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        public void OnStartup(object sender, StartupEventArgs e)
        {
			Application.Current.DispatcherUnhandledException += Current_DispatcherUnhandledException;
            MusicSync musicSync = new MusicSync();
            musicSync.Show();
        }

		void Current_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
		{
			MessageBox.Show("Error happened : {0}" + e.Exception.Message);
		}
		
    }
}
