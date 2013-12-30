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
using System.ComponentModel;

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
        List<Sound> sounds = new List<Sound>();

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //this.Init(sender, null);
            BackgroundWorker backgroundWorker = new BackgroundWorker();
            backgroundWorker.DoWork += this.Init;
            backgroundWorker.RunWorkerCompleted += this.InitDone;
            backgroundWorker.RunWorkerAsync();
        }

        private void Init(object sender, DoWorkEventArgs e)
        {
            int count = int.Parse(Program.vk.GetAudioCountFromUser(Program.vk.UserId, false).SelectSingleNode("response").InnerText);
            if (count > 0)
            {
                sounds = new List<Sound>();
                AudiosContainer container = new AudiosContainer();
                double value = 0;
                //Dispatcher.Invoke(,new object[]{ ProgressBar.ValueProperty, value++ });
                container.Bind(Program.vk.GetAudioFromUser(Program.vk.UserId, false, 0, 2));
                //listbox1.DataContext = container.getSound();
                sounds = container.getSound();
            }
        }

        private void InitDone(object sender, RunWorkerCompletedEventArgs e)
        {
            listbox1.DataContext = sounds;
        }

        #region sync audio
        private void Sync_Click(object sender, RoutedEventArgs e)
        {
            if (sounds!=null)
            { 
                BackgroundWorker backgroundWorker = new BackgroundWorker();
                backgroundWorker.DoWork += this.DoWork;
                backgroundWorker.RunWorkerCompleted += this.OnCompletedLoad;
                backgroundWorker.RunWorkerAsync(sounds);
            }
        }

        private void OnCompletedLoad(object sender, RunWorkerCompletedEventArgs e)
        {
            string directory = @"audio\";
            IOHandler.OpenPath(directory);
        }

        private void DoWork(object sender, DoWorkEventArgs e)
        {
            List<Sound> sounds = (List<Sound>)e.Argument;
            SynhronizeAdapter adapter = new SynhronizeAdapter();
            adapter.SyncFolderWithList<Sound>(sounds, @"audio\");
            
        }
        #endregion

        private void PhotosFormAc_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("PhotosFormAc_Click");
        }

        private void ShowSetteng_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("ShowSetteng_Click");
        }

        private void Audio_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Audio_Click");
        }




    }
    /*
    public class Comparer:IComparer
    {
        public bool CompareAudio(stri)
    }*/
}
