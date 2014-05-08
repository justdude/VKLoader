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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace VKMusicSync
{
    /// <summary>
    /// Логика взаимодействия для Settings.xaml
    /// </summary>
    public partial class Settings : Window
    {
        public Settings()
        {
            InitializeComponent();
        }



        private void Window_Closed(object sender, EventArgs e)
        {
            //this.Owner.Show();
        }

        private void Apply_Click(object sender, RoutedEventArgs e)
        {
            this.ApplySettings();
        }


        private void OK_Click(object sender, RoutedEventArgs e)
        {
            this.ApplySettings();
            this.Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.LoadFromSetting();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.LoadFromSetting();
        }

        private void LoadFromSetting()
        {
            isUseAutoLogOff.IsChecked = Properties.Settings.Default.isUseAutoLogOff;
            isCanBeInTray.IsChecked = Properties.Settings.Default.isCanBeInTray;
            isOpenFolderAfterLoad.IsChecked = Properties.Settings.Default.isOpenFolderAfterLoad;
            maxCountSongs.Text = Properties.Settings.Default.maxAudiosCount.ToString();
        }

        private void ApplySettings()
        {
            Properties.Settings.Default.isUseAutoLogOff = (bool)isUseAutoLogOff.IsChecked;
            Properties.Settings.Default.isCanBeInTray = (bool)isCanBeInTray.IsChecked;
            Properties.Settings.Default.isOpenFolderAfterLoad = (bool)isOpenFolderAfterLoad.IsChecked;
            Properties.Settings.Default.maxAudiosCount = int.Parse(maxCountSongs.Text);

            Properties.Settings.Default.Save();
        }


    }
}
