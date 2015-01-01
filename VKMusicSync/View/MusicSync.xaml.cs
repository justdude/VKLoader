using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Elysium;
using VKMusicSync;
using VKMusicSync.Model;
using VKMusicSync.ModelView;

namespace VKMusicSync
{
    /// <summary>
    /// Логика взаимодействия для MusicSync.xaml
    /// </summary>
    public partial class MusicSync : Elysium.Controls.Window
    {
				private Auth authWindow;

        public MusicSync()
        {
            //InitializeComponent();
        }

        public void HelpClick(Object sender,RoutedEventArgs e)
        { 

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var modelView = new MainMovelView();
            this.DataContext = modelView;

        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            authWindow = new Auth();
            authWindow.auth.OnInit += OnAuthDone;
            authWindow.ShowDialog();
        }
        private void OnAuthDone()
        {
            if (authWindow != null) authWindow.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }
    }
}
