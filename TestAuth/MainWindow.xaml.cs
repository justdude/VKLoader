using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TestAuth
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
		}

		public void Test()
		{ 
			
		}
		const string VKAdress= @"http://m.vk.com/login";
		public static string Get(string adress)
		{
			string res = string.Empty;
			
			try
			{
				ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
				WebRequest request = WebRequest.Create(adress);
				WebResponse resp = request.GetResponse();
				Stream stream = resp.GetResponseStream();
				StreamReader reader = new StreamReader(stream);
				res = reader.ReadToEnd();
			}
			catch(Exception)
			{ }

			return res;
		}

		private void btnAutorize_Click(object sender, RoutedEventArgs e)
		{
			string webPage = Get(VKAdress);
		}
	}
}
