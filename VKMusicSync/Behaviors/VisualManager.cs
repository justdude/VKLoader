using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Behaviors
{
	public static class VisualManager
	{

		#region Hide if disabled

		public static bool GetIsHideOnDisabled(DependencyObject obj)
		{
			return (bool)obj.GetValue(IsHideOnDisabledProperty);
		}

		public static void SetIsHideOnDisabled(DependencyObject obj, bool value)
		{
			obj.SetValue(IsHideOnDisabledProperty, value);
		}

		public static readonly DependencyProperty IsHideOnDisabledProperty =
			DependencyProperty.RegisterAttached("IsHideOnDisabled", typeof(bool), typeof(VisualManager), new PropertyMetadata(false, OnIsHideOnDisabledPropertyChangedCallback));


		private static void OnIsHideOnDisabledPropertyChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
		{
			var uiElement = dependencyObject as UIElement;

			if (uiElement == null)
				return;

			var isShowOnDisabled = (bool)args.NewValue;

			uiElement.IsEnabledChanged -= OnUiElementIsEnabledChanged;

			if (isShowOnDisabled)
			{
				uiElement.Visibility = Visibility.Visible;
			}
			else
			{
				uiElement.IsEnabledChanged += OnUiElementIsEnabledChanged;
			}
		}

		private static void OnUiElementIsEnabledChanged(object sender, DependencyPropertyChangedEventArgs args)
		{
			var uiElelement = sender as UIElement;

			if (uiElelement == null)
				return;

			uiElelement.Visibility = (bool)args.NewValue ? Visibility.Visible : Visibility.Collapsed;
		}

		#endregion


	}
}
