using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Collections.ObjectModel;
using MVVM;
using VKMusicSync.Model;
using VKMusicSync.ModelView;

namespace VKMusicSync.ModelView
{
    class SoundDownloaderMovelView: ViewModelBase
    {
        public ObservableCollection<SoundModelView> Sounds { get; set; }
        
        public SoundDownloaderMovelView(List<Sound> sounds)
        {
            Sounds = new ObservableCollection<SoundModelView>( sounds.Select(s => new SoundModelView(s)) );
        }

        private DelegateCommand sync;
        public ICommand Sync
        {
            get
            {
                if (sync==null)
                {
                    sync = new DelegateCommand(OnSync);
                }
                return sync;
            }

        }

        private void OnSync()
        {
            System.Windows.Forms.MessageBox.Show("Hello");
        }

    }
}
