using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VKMusicSync.ViewModel
{
	public interface IDataState
	{
		bool IsNeedFill { get; set;}
		bool IsFirstLoadDone { get;}
	}
}
