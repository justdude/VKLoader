using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using VKMusicSync.Handlers.Observer;

namespace MVVM
{
    /// <summary>
    /// Provides common functionality for ViewModel classes
    /// </summary>
    public abstract class ViewModelBase : INotifyPropertyChanged, IReceiver
    {
				
				private bool mvIsBusy;

				private static Observer mvMessenger;

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

				public Observer Messenger 
				{
					get
					{
						return mvMessenger;
					}
				}

				public Dispatcher WindowDispatcher { get; set; }

				public event PropertyChangedEventHandler PropertyChanged;

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
					mvMessenger = new Observer();
				}

				public ViewModelBase()
				{
					Messenger.Add(this);
				}

				public void Execute(Action action)
				{
					if (WindowDispatcher == null || action == null)
						return;

					WindowDispatcher.Invoke(action);
				}

				public void BeginExecute(Action action)
				{
					if (WindowDispatcher == null || action == null)
						return;

					WindowDispatcher.BeginInvoke(action);
				}

				#region IReceiver Members

				public virtual void HandleRequest(object sender)
				{

				}

				#endregion
		}
}
