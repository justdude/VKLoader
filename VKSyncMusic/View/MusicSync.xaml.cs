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
using System.Windows.Shapes;
using Elysium;

using VKSyncMusic;
using VKSyncMusic.Model;
using VKSyncMusic.ModelView;
using VKSyncMusic.ModelView.Tabs;

namespace VKSyncMusic
{

		//public class TabControlPropties : TabControl
		//{
		//	public static DependencyProperty ItemsProperty = DependencyProperty.Register("Item", typeof(TabModelView), typeof(TabItem));

		//	public TabModelView Item
		//	{
		//		get { return (TabModelView)GetValue(ItemsProperty); }
		//		set { SetValue(ItemsProperty, value); }
		//	}

		//	public static DependencyProperty ItemsCollectionProperty = DependencyProperty.("ItemsCollection", typeof(IEnumerable<TabModelView>), typeof(TabControl), new PropertyMetadata(new PropertyChangedCallback(OnItemsCollectionChanged)));


		//	public IEnumerable<TabModelView> ItemsCollection
		//	{
		//		get { return (IEnumerable<TabModelView>)GetValue(ItemsCollectionProperty); }
		//		set { SetValue(ItemsCollectionProperty, value); }
		//	}

		//	private static void OnItemsCollectionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		//	{
		//		TabControl tbCr = d as TabControl;

		//		if (tbCr == null)
		//			return;

		//		tbCr.Items.Add(e.NewValue);
		//	}
		//}



    /// <summary>
    /// Логика взаимодействия для MusicSync.xaml
    /// </summary>
    public partial class MusicSync : Elysium.Controls.Window
    {

        public MusicSync()
        {
            //InitializeComponent();						
						MainModelView modelView = new MainModelView();
						DataContext = modelView;
        }

        public void HelpClick(Object sender,RoutedEventArgs e)
        { 

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {


						//modelView.Window_Loaded();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
        }

        private void OnAuthDone()
        {

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }
    }
}
