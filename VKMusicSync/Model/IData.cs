using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VKMusicSync.Model
{
    public interface IDownnloadedData
    {
        double Size { get; set; }
        double LoadedSize { get; set; }
        bool SyncState { get; set; }

        string GenerateFileName();
        string GenerateFileExtention();
        string GetUrl();
    }
}
