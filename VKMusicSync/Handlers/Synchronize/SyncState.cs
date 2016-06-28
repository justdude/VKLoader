using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace VKMusicSync.Handlers.Synchronize
{
    public enum SyncStates
    {
        Unknown = 0x0,
        NeedDownloaded = 0x2,
        NeedUpload = 0x4,
		NeedCustomUpload = 0x40,
		Synced = 0x20,
        SyncFailed = Unknown
    }

    public class SyncState
    {

        public static DependencyProperty State = DependencyProperty.RegisterAttached(
            "State",
            typeof(SyncStates), 
            typeof(SyncState), 
            new PropertyMetadata( SyncStates.Unknown, StatChanged));

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
