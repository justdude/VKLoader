using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace MVVM
{
    /// <summary>
    /// Provides common functionality for ViewModel classes
    /// </summary>
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
				
				private bool mvIsBusy;

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

						OnPropertyChanged("IsBusy");
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

						OnPropertyChanged("IsLoading");
					}
				}

				public Dispatcher CurrentDispatcher { get; set; }

				public event PropertyChangedEventHandler PropertyChanged;
				private bool mvIsLoading;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;

            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

				static ViewModelBase()
				{
				}

				public ViewModelBase()
				{
					CurrentDispatcher = Application.Current.Dispatcher;
				}

				public void Execute(Action action)
				{
					if (CurrentDispatcher == null || action == null)
						return;

					CurrentDispatcher.Invoke(action);
				}

				public void BeginExecute(Action action)
				{
					if (CurrentDispatcher == null || action == null)
						return;

					CurrentDispatcher.BeginInvoke(action);
				}

				#region IReceiver Members


				#endregion
		}
}
