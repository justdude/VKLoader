using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace MVVM
{
	public class TabModelView : ViewModelBase
	{
		public string Header { get; set; }

		public virtual ICommand CloseTab { get; set;}

		public TabModelView():base()
		{
			Header = "dsds";
		}

		////public override string ToString()
		////{
		////return "!!!!!!";
		////	//return base.ToString();
		////}

	}
}
