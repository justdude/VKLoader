using System;
using System.Windows.Input;

namespace MVVM
{
	public interface ITabModelView
	{
		ICommand CloseTab { get; set; }
		string Header { get; set; }
	}
}
