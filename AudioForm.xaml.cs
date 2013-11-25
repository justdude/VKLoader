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

using System.Threading;

namespace VK
{
    /// <summary>
    /// Логика взаимодействия для UserControl1.xaml
    /// </summary>
    public partial class AudioForm : Window
    {
        public AudioForm()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            int count = int.Parse(Program.vk.GetAudioCountFromUser(Program.vk.UserId, false).SelectSingleNode("response").InnerText);
           MessageBox.Show(""+count);
           if (count > 0) 
           {
               List<Sound> sounds = new List<Sound>();
               AudiosContainer container = new AudiosContainer();
               container.Bind(Program.vk.GetAudioFromUser(Program.vk.UserId, false, 0, 100));
               listbox1.DataContext = container.getSound();
           }
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Next_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Sync_Click(object sender, RoutedEventArgs e)
        {

        }

        private void PhotosFormAc_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ShowSetteng_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {

        }


        private void Share_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
