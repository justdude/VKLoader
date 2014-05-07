using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VKMusicSync.Model
{
    public interface IData
    {
        string GenerateFileName();
        string GenerateFileExtention();
        string GetUrl();
    }
}
