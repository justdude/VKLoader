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
        public MusicSync()
        {
            //InitializeComponent();						
						SoundDownloaderMovelView modelView = new SoundDownloaderMovelView();
						DataContext = modelView;
        }

        public void HelpClick(Object sender,RoutedEventArgs e)
        { 

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {


						//modelView.Window_Loaded();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
        }

        private void OnAuthDone()
        {

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }
    }
}
