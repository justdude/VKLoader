using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using GalaSoft.MvvmLight;

namespace MVVM
{
	/// <summary>
	/// Provides common functionality for ViewModel classes
	/// </summary>
	public abstract class AdwancedViewModelBase : ViewModelBase
	{
		#region Fields

		private bool mvIsBusy;
		private bool mvIsLoading;
		private string mvToken;

		#endregion

		#region Properties

		public Dispatcher CurrentDispatcher { get; set; }

		public bool IsBusy
		{
			get
			{
				return mvIsBusy;
			}
			set
			{
				if (value == mvIsBusy)
					return;

				mvIsBusy = value;

				RaisePropertyChanged<bool>(() => IsBusy);
			}
		}

		public bool IsLoading
		{
			get
			{
				return mvIsLoading;
			}
			set
			{
				if (value == mvIsLoading)
					return;

				mvIsLoading = value;

				RaisePropertyChanged<bool>(() => IsLoading);
			}
		}


		public virtual string Token
		{
			get
			{
				return mvToken;
			}
			set
			{
				if (mvToken == value)
					return;

				mvToken = value;

				OnTokenChanged();

				RaisePropertyChanged<string>(() => Token);
			}

		}
		#endregion

		#region .Ctr

		public AdwancedViewModelBase()
		{
			CurrentDispatcher = Application.Current.Dispatcher;
		}
		#endregion

		#region Helpers methods
		public void Execute(Action action)
		{
			if (action == null)
				return;

			CurrentDispatcher.Invoke(action);
		}

		public void BeginExecute(Action action)
		{
			if (CurrentDispatcher == null || action == null)
				return;

			CurrentDispatcher.BeginInvoke(action);
		}

		protected void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler handler = PropertyChanged;

			if (handler != null)
			{
				handler(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		#endregion

		#region Public methods

		public void CleanViewModel()
		{
			OnCleanup();
		}

		#endregion

		#region Methods virtual
		protected virtual void OnTokenChanged()
		{
			
		}

		protected virtual void RefreshPrivate()
		{
		}

		protected virtual void OnCleanup()
		{
			Cleanup();
		}

		#endregion

		#region Events

		public event PropertyChangedEventHandler PropertyChanged;

		#endregion

		#region IReceiver Members


		#endregion
	}
}
