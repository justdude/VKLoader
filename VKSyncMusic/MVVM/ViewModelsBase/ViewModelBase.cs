using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;

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
					protected set
					{
						if (mvIsBusy == value)
							return;

						mvIsBusy = value;

						OnPropertyChanged("IsBusy");
					}
				}

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;

            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

				public void Execute(Action act)
				{
					if (act == null)
						return;

					Application.Current.Dispatcher.Invoke(act, new object[] { });
				}
				public Type Type
				{
					get { return this.GetType(); }
				}

    }
}
