using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VKSyncMusic.Delegates
{
    public class ActionsDelegates
    {
        public delegate void DataStateChangedDelegate(object sender, Argument state);


        public static void Execute(DataStateChangedDelegate del, object sender, Argument state)
        {
            if (del != null)
            {
                del(sender, state);
            }
        }

    }
}
