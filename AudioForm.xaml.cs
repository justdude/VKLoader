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
using System.Net;

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
        SynhronizeAdapter adapter;


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //this.Init(sender, null);
            BackgroundWorker backgroundWorker = new BackgroundWorker();
            backgroundWorker.DoWork += this.Init;
            backgroundWorker.RunWorkerCompleted += this.InitDone;
            InitProgressbar(this.progressBar);
            backgroundWorker.RunWorkerAsync();
        }

        private void InitProgressbar(ProgressBar progressBar)
        {
            progressBar.Maximum = 100;
            progressBar.Minimum = 0;
            progressBar.SetValue(ProgressBar.ValueProperty, (double)0);
        }

        private void Init(object sender, DoWorkEventArgs e)
        {
            
            int count = int.Parse(Program.vk.GetAudioCountFromUser(Program.vk.UserId, false).SelectSingleNode("response").InnerText);
            //count = 20;
            count = (count>Properties.Settings.Default.maxAudiosCount?Properties.Settings.Default.maxAudiosCount:count);
            if (count > 0)
            {
                sounds = new List<Sound>();
                AudiosContainer container = new AudiosContainer();
                Program.vk.SetDownloadXMLProgressChanged(OnLoadingXML);
                container.Bind(Program.vk.GetAudioFromUser(Program.vk.UserId, false, 0, count));
                //listbox1.DataContext = container.getSound();
                sounds = container.getSound();
            }
        }
        private void OnLoadingXML(Object sender, DownloadProgressChangedEventArgs e)
        {
            //MessageBox.Show(""+e.ProgressPercentage);
            Dispatcher.Invoke( new Action<DependencyProperty,object>(progressBar.SetValue),
                               new object[]{ ProgressBar.ValueProperty,
                                            (double)Math.Abs(1-e.ProgressPercentage)});
        }

        private void InitDone(object sender, RunWorkerCompletedEventArgs e)
        {
            Dispatcher.Invoke(new Action<DependencyProperty, object>(progressBar.SetValue),
                             new object[]{ ProgressBar.ValueProperty,
                                            progressBar.Maximum});
            Dispatcher.BeginInvoke(new Action(this.SetListBox), new object[] { });
        }

        private void SetListBox()
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
                backgroundWorker.RunWorkerCompleted += this.OnCompletedAllLoad;
                backgroundWorker.RunWorkerAsync(sounds);
            }
        }

        private void OnChangeLoadFile(object sender, DownloadProgressChangedEventArgs e)
        {
            Dispatcher.Invoke(new Action<DependencyProperty, object>(progressBar.SetValue),
                       new object[]{ ProgressBar.ValueProperty,
                                            (double)Math.Abs(1-e.ProgressPercentage)});
            Dispatcher.Invoke(new Action(() => { downloadStatus.Text = "" 
                                                                      + adapter.GetDownloadedFileNumber()
                                                                      +"/"+adapter.GetDownloadedFilesCount();}),
                              new object[0]{});
            
        }

        private void OnCompleteLoadFile(object sender, DownloadDataCompletedEventArgs e)
        {
            Dispatcher.Invoke(new Action<DependencyProperty, object>(progressBar.SetValue),
                       new object[]{ ProgressBar.ValueProperty,
                                            (double)Math.Abs(100)});
        }

        private void OnCompletedAllLoad(object sender, RunWorkerCompletedEventArgs e)
        {
            if (!Properties.Settings.Default.isOpenFolderAfterLoad) return;
            string directory = @"audio\";
            IOHandler.OpenPath(directory);
        }

        private void DoWork(object sender, DoWorkEventArgs e)
        {
            List<Sound> sounds = (List<Sound>)e.Argument;
            adapter = new SynhronizeAdapter();
            adapter.OnLoaded = this.OnCompleteLoadFile;
            adapter.OnProgress = this.OnChangeLoadFile;
            adapter.SyncFolderWithList<Sound>(sounds, @"audio\");
        }
        #endregion


        #region buttons
        private void PhotosFormAc_Click(object sender, RoutedEventArgs e)
        {
            AlbumForm albumForm = new AlbumForm();
            albumForm.Owner = this;
            this.Hide();
            albumForm.Show();
        }

        private void ShowSetteng_Click(object sender, RoutedEventArgs e)
        {
            Settings settings = new Settings();
            settings.ShowDialog();
        }

        private void Audio_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Audio_Click");
        }
        #endregion


        private void Window_Closed(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.isUseAutoLogOff) Environment.Exit(0);
            Login.loaded = false;
            Main.LogOff(this, this.OnLoggOff);
        }

        private void OnLoggOff(object sender, System.Windows.Forms.WebBrowserDocumentCompletedEventArgs e)
        {
            //on logg off
            
            if (Login.loaded) return;
            Login form = new Login();
            form.Show();
            WebBrowser browser = (WebBrowser)sender;
            browser.Dispose();
            //Application.Current.Shutdown();
            
        }

    }
}
