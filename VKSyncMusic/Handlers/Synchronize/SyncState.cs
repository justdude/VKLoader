using MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace VKSyncMusic.Handlers.Synchronize
{
    public enum SyncStates
    {
        Default = 0,
        IsNeedDownload,
        IsNeedUpload,
        IsSynced,
        IsCanntUpdate,
    }

    public class SyncState
    {

        public static DependencyProperty State = DependencyProperty.RegisterAttached(
            "State",
            typeof(SyncStates), 
            typeof(SyncState), 
            new PropertyMetadata( SyncStates.Default, StatChanged));

        private static void StatChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Type type = e.Property.GetType();
        }

        public static void SetState(DependencyObject obj, SyncStates value)
        {
            obj.SetValue(State, value);
        }

        public static SyncStates GetState(DependencyObject obj)
        {
            return (SyncStates)obj.GetValue(State);
        }

       
    }
}
