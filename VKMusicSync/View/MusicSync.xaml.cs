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
using VkDay;
using MIP.MVVM.View;
using VKMusicSync.CustomControls;

namespace VKMusicSync
{
    /// <summary>
    /// Логика взаимодействия для MusicSync.xaml
    /// </summary>
    public partial class MusicSync : WindowExtended
    {
        public MusicSync():base()
        {
            //InitializeComponent();
        }

        public void HelpClick(Object sender,RoutedEventArgs e)
        { 

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var modelView = new MainModelView();
            this.DataContext = modelView;
			modelView.Token = this.Token;
			//modelView.CurrentDispatcher = Dispatcher;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }
    }
}
