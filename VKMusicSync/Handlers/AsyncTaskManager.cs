using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace VKMusicSync.Handlers
{
    public class Pack<T>
    {
        public ManualResetEvent endEvent;
        public T value;
        public Pack(ManualResetEvent endEvent, T val)
        {
            this.endEvent = endEvent;
            this.value = val;
        }
    }

    public class AsyncTaskManager<T> //where T: class
    {
        private readonly object locker = new object();
        private int executedTaskCount = 0;
        public int ExecutedTaskCount
        {
            get
            {
                lock (locker)
                {
                    return executedTaskCount;
                }
            }
            private set
            {
                lock (locker)
                {
                    executedTaskCount = value;
                }
            }
        
        }

        public int TaskCount
        {
            get
            {
                lock (locker)
                {
                    return toProcess.Count;
                }
            }

        }

        public delegate void ExecuteWork(T item);
        private ExecuteWork Execute { get; set; }
        private List<T> toProcess;
        private ManualResetEvent[] endEvents;


        public AsyncTaskManager(ExecuteWork del, IList<T> parametrs, int maxThreadsCount)
        {
            this.Execute = del;
            this.toProcess = parametrs.ToList();
            this.ExecutedTaskCount = maxThreadsCount;
            this.endEvents = new ManualResetEvent[1];
            ThreadPool.SetMaxThreads(executedTaskCount, executedTaskCount);
        }

        private void DoTask(object parametr)
        {
            Pack<T> pack = (Pack<T>)parametr;
            Execute(pack.value);
            ExecutedTaskCount++;
            if (ExecutedTaskCount>=TaskCount)
                pack.endEvent.Set();
        }

        public void Start()
        {
            endEvents[0] = new ManualResetEvent(false);
            for (int i = 0; i < toProcess.Count; i++)
            {
                Pack<T> pack = new Pack<T>(endEvents[0], toProcess[i]);
                ThreadPool.QueueUserWorkItem(new WaitCallback(DoTask), pack);
            }
            WaitHandle.WaitAll(endEvents);

        }

    }
}
