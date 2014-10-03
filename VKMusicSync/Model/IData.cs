using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using VKMusicSync.Handlers.Synchronize;
using VKMusicSync.Delegates;

namespace VKMusicSync.Model
{


    public interface IDownnloadedData : IData
    {
        IStateChanged InstanceWithEvents { get; }

        double LoadedSize { get; set; }
        bool IsLoadedToDisk { get; set; }
        SyncStates State { get; set; }

        bool IsEqual(IDownnloadedData data);
    }

    public interface IStateChanged
    {
        void OnLoadStarted(object sender, Argument state);

        void OnProgresChanged(object sender, Argument state);

        void OnLoadEnded(object sender, Argument state);

        void OnRaiseError(object sender, Argument state);
    
    }

    public interface IData
    {
        string PathWithFileName { get; }
        string FileName { get; set; }

        string Path { get; set; }

        double Size { get; set; }
        string FileExtention { get; }

        string MD5 { get; set; }
    }

}
