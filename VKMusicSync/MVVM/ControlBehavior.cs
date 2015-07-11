namespace MVVM.AttachedProperties
{
	using System;
	using System.Diagnostics;
	using System.Windows;
	using System.Windows.Controls;
	using System.Windows.Input;

	/// <summary>
	///   Represents 
	/// </summary>
	public static class ControlBehavior
	{

		public static bool IsDesignerMode()
		{
			if (System.Reflection.Assembly.GetExecutingAssembly().Location.Contains("VisualStudio"))
			{
				return true;
			}
			return false;
		}

		#region Is Read Only

		public static readonly DependencyProperty IsReadOnlyProperty = DependencyProperty.RegisterAttached("IsReadOnly", typeof(bool), typeof(ControlBehavior), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.Inherits));

		public static bool GetIsReadOnly(DependencyObject control)
		{
			return (bool)control.GetValue(IsReadOnlyProperty);
		}

		public static void SetIsReadOnly(DependencyObject control, bool isReadOnly)
		{
			control.SetValue(IsReadOnlyProperty, isReadOnly);
		}

		#endregion // Is Read Only

		#region Is Visible

		public static readonly DependencyProperty IsVisibleProperty = DependencyProperty.RegisterAttached("IsVisible", typeof(bool?), typeof(ControlBehavior), new FrameworkPropertyMetadata(null));

		public static bool GetIsVisible(UIElement control)
		{
			return (bool)control.GetValue(IsVisibleProperty);
		}

		public static void SetIsVisible(UIElement control, bool? isVisible)
		{
			control.SetValue(IsVisibleProperty, isVisible);

			if (isVisible == null)
				return;

			control.SetValue(UIElement.VisibilityProperty, isVisible.Value ? Visibility.Visible : Visibility.Collapsed);
		}

		#endregion // Is Visible

		#region Token

		private static readonly DependencyPropertyKey TokenPropertyKey = DependencyProperty.RegisterAttachedReadOnly("Token", typeof(string), typeof(ControlBehavior), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.Inherits));

		public static readonly DependencyProperty TokenProperty = TokenPropertyKey.DependencyProperty;

		public static string GetToken(DependencyObject dependencyObject)
		{
			if (IsDesignerMode())
				return string.Empty;

			var token = string.Empty;

			if (dependencyObject is Window || dependencyObject is UserControl || dependencyObject is ContentControl)
			{
				token = (string)dependencyObject.GetValue(TokenProperty);
			}

			if (string.IsNullOrEmpty(token))
			{
				token = Guid.NewGuid().ToString();

				SetToken(dependencyObject, token);
			}

			return token;
		}

		private static void SetToken(DependencyObject dependencyObject, string token)
		{
			dependencyObject.SetValue(TokenPropertyKey, token);
		}

		#endregion // Token

		#region Is Show On Disabled

		public static readonly DependencyProperty IsShowOnDisabledProperty = DependencyProperty.RegisterAttached("IsShowOnDisabled", typeof(bool), typeof(ControlBehavior), new FrameworkPropertyMetadata(true, OnIsShowOnDisabledPropertyChangedCallback));

		[AttachedPropertyBrowsableForType(typeof(UIElement))]
		public static bool GetIsShowOnDisabled(UIElement uiElement)
		{
			return (bool)uiElement.GetValue(IsShowOnDisabledProperty);
		}

		public static void SetIsShowOnDisabled(UIElement uiElement, bool isShowOnDisabled)
		{
			uiElement.SetValue(IsShowOnDisabledProperty, isShowOnDisabled);
		}

		private static void OnIsShowOnDisabledPropertyChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
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

		#endregion // Is Show On Disabled

		#region IsBlocked

		public static readonly DependencyProperty IsBlockedProperty = DependencyProperty.RegisterAttached("IsBlocked", typeof(bool), typeof(ControlBehavior), new FrameworkPropertyMetadata(true, OnIsBlockedPropertyChangedCallback));

		public static bool GetIsBlocked(UIElement uiElement)
		{
			return (bool)uiElement.GetValue(IsBlockedProperty);
		}

		public static void SetIsBlocked(UIElement uiElement, bool isShowOnDisabled)
		{
			uiElement.SetValue(IsBlockedProperty, isShowOnDisabled);
		}

		private static void OnIsBlockedPropertyChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
		{
			var depObj = dependencyObject as FrameworkElement;

			if (depObj == null)
				return;

			depObj.ContextMenuOpening -= ControlBehavior_ContextMenuOpening;
			depObj.PreviewMouseDown -= ControlBehavior_PreviewMouseDown;
			depObj.PreviewKeyDown -= ControlBehavior_PreviewKeyDown;

			var isBlocked = (bool)args.NewValue;

			if (isBlocked)
			{
				depObj.ContextMenuOpening += ControlBehavior_ContextMenuOpening;
				depObj.PreviewMouseDown += ControlBehavior_PreviewMouseDown;
				depObj.PreviewKeyDown += ControlBehavior_PreviewKeyDown;
			}
		}

		private static void ControlBehavior_ContextMenuOpening(object sender, ContextMenuEventArgs e)
		{
			e.Handled = true;
		}

		private static void ControlBehavior_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
		{
			e.Handled = true;
		}

		private static void ControlBehavior_PreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
			e.Handled = true;
		}

		#endregion IsBlocked

		#region Prevent View Update

		public static readonly DependencyProperty PreventViewUpdateProperty = DependencyProperty.RegisterAttached("PreventViewUpdate", typeof(bool), typeof(ControlBehavior), new FrameworkPropertyMetadata(false));

		[AttachedPropertyBrowsableForType(typeof(ScrollViewer))]
		public static bool GetPreventViewUpdate(ScrollViewer scrollViewer)
		{
			return (bool)scrollViewer.GetValue(PreventViewUpdateProperty);
		}

		[AttachedPropertyBrowsableForType(typeof(ScrollViewer))]
		public static void SetPreventViewUpdate(ScrollViewer scrollViewer, bool preventViewUpdate)
		{
			scrollViewer.SetValue(PreventViewUpdateProperty, preventViewUpdate);
		}

		#endregion // Prevent View Update
	}
}