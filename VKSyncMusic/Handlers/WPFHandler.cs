using MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using VKSyncMusic.ModelView.Tabs;

namespace VKSyncMusic.Handlers
{
	public static class WPFExtension
	{

		public static TabItem[] GetConstantsContentElement(string key)
		{
			var coll = Application.Current.TryFindResource(key) as CompositeCollection;
			
			if (coll == null)
				return null;

			return TotabItems(coll);
		}

		public static TabItem[] TotabItems(CompositeCollection collection)
		{
			List<TabItem> targetArray = new List<TabItem>();
			for (int i = 0; i < collection.Count; i++)
			{
				var targetValue = collection[i] as TabItem;
				
				if (targetValue == null)
					continue;

				targetArray.Add(targetValue);
			}
			return targetArray.ToArray();
		}

		public static T GetData<T>(this TabItem tab)where T : class, new()
		{
			if (tab == null)
				return null;

			 return tab.DataContext as T;
		}

		public static void SetData<T>(this TabItem tab, T data) where T : class, new()
		{
			if (tab == null)
				return;

			tab.DataContext = data;
		}
			 
		public static object GetDataToTag(this FrameworkElement item)
		{
			if (item == null)
				return null;

			return item.Tag;
		}

		public static void SetDataToTag(this FrameworkElement item, object targetValue)
		{
			if (item == null)
				return;

			item.Tag = targetValue;
		}

		public static TabItem AddTab(TabModelView modelView, string templateKey)
		{
			var test = System.Windows.Application.Current.MainWindow.TryFindResource(templateKey);
			ControlTemplate template = System.Windows.Application.Current.MainWindow.TryFindResource(templateKey) as ControlTemplate;


			template = Application.Current.TryFindResource(templateKey) as ControlTemplate;
			

			if (template == null)
				return null;

			var content = new ContentControl();
			content.Template = template;
			content.DataContext = modelView;

			var tab = new TabItem { Content = content, Tag = modelView };
			tab.SetDataToTag(modelView);

			tab.Content = content;

			//if (content.DataContext as TabModelView != null)
			//{
			//	Binding tabTitleBinding = new Binding { Path = new PropertyPath("TypeName"), Mode = BindingMode.TwoWay };
			//	tabTitleBinding.Source = modelView;
			//	tab.SetBinding(HeaderedContentControl.HeaderProperty, tabTitleBinding);
			//}
			
			return tab;
		}


	}
}
