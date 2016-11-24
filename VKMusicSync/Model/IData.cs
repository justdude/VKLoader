using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using VKMusicSync.Delegates;

namespace VKMusicSync.Model
{
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
