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

namespace VKMusicSync.View
{
	/// <summary>
	/// Interaction logic for AudioList.xaml
	/// </summary>
	public partial class AudioList : UserControl
	{
		public AudioList()
		{
			InitializeComponent();
		}

		//public static DependencyProperty TypeConstProperty = DependencyProperty.RegisterAttached("TypeConst",
		//typeof(string),
		//typeof(AudioList));

		//public string TypeConst
		//	{
		//		get { return (string)GetValue(TypeConstProperty); }
		//		set { SetValue(TypeConstProperty, value); }
		//	}

		//public static DependencyProperty TypeProperty = DependencyProperty.Register("Type", 
		//	typeof(string), 
		//	typeof(AudioList));

		//public string Type
		//{
		//	get { return (string)GetValue(TypeConstProperty); }
		//	set { SetValue(TypeProperty, value); }
		//}

	}
}
