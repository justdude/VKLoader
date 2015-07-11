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

namespace VkEasyPhones.View
{
	/// <summary>
	/// Interaction logic for ctrPlacementSelect.xaml
	/// </summary>
	public partial class ctrPlacementSelect : UserControl
	{
		public ctrPlacementSelect()
		{
			InitializeComponent();
		}

		public string SelectedCityId
		{
			get { return (string)GetValue(SelectedCityIdProperty); }
			set { SetValue(SelectedCityIdProperty, value); }
		}

		public static readonly DependencyProperty SelectedCityIdProperty =
				DependencyProperty.Register("SelectedCityId", typeof(string), typeof(ctrPlacementSelect), new PropertyMetadata(string.Empty, PropertyChanged));

		private static void PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			//cmbCityes.Text = SelectedCityId;
		}

		private void cmbCityes_TextChanged(object sender, TextChangedEventArgs e)
		{
			SelectedCityId = cmbCityes.Text;
		}

	}
}
