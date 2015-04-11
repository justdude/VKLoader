using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace VKMusicSync.Handlers
{
	public class Pack<T>
	{
		public ManualResetEvent EndEvent { get; private set; }
		public Action<T> TargetAction { get; private set; }
		private Thread modThread;
		public Pack(ManualResetEvent endEvent, Action<T> targetAction)
		{
			if (endEvent == null || targetAction == null)
				throw new ArgumentNullException();

			TargetAction = targetAction;
			EndEvent = endEvent;
			modThread = new Thread(new ParameterizedThreadStart(Execute));
		}

		public void Start(T param)
		{
			modThread.Start(param);
		}

		private void Execute(object par)
		{
			T parm = default(T);

			if (par != null)
			{
				parm = (T)par;
			}

			TargetAction(parm);

			EndEvent.Set();
		}
	}

	public class AsyncTaskManager<T>
	{
		#region Fields
		private readonly object locker = new object();
		private int executedTaskCount = 0;
		private Queue<T> modEvents;
		private ManualResetEvent resetEvent = null;
		private int mvCurrentThreadsCount;
		private ManualResetEvent[] endEvents;
		#endregion

		#region Properties

		public Action<T> Execute { get; set; }

		public int ThreadsCount
		{
			get
			{

				return executedTaskCount;
			}
			private set
			{
				executedTaskCount = value;
			}
		}

		public int CurrentThreadsCount
		{
			get
			{
				return mvCurrentThreadsCount;
			}
			private set
			{
				lock (locker)
				{
					mvCurrentThreadsCount = value;
				}
			}
		}

		public ManualResetEvent AllDone { get; private set; }

		#endregion

		#region Ctr

		public AsyncTaskManager(Action<T> action)
		{
			this.Execute += action;
		}

		#endregion

		#region Methods

		public void ProcessAsync(IList<T> data, int maxThreadsCount)
		{
			AllDone = new ManualResetEvent(false);

			if (Execute == null)
			{
				EndExecute();
				return;
			}

			if (data == null || data.Count == 0)
			{
				EndExecute();
				return;
			}

			modEvents = new Queue<T>(data);
			ThreadsCount = maxThreadsCount;

			resetEvent = new ManualResetEvent(false);
			CurrentThreadsCount = 0;

			new Thread(() =>
				{
					for (int i = 0; i < modEvents.Count; i++)
					{
						int threadsCount = Math.Min(ThreadsCount, modEvents.Count);

						ManualResetEvent[] waitEvents = new ManualResetEvent[threadsCount];

						while (CurrentThreadsCount < threadsCount)
						{
							ProcessItem(waitEvents);
						}

						ManualResetEvent.WaitAll(waitEvents);
						CurrentThreadsCount = 0;

					}

					EndExecute();

				}).Start();

		}//end

		private void EndExecute()
		{
			AllDone.Set();
		}

		private void ProcessItem(ManualResetEvent[] waitEvents)
		{
			ManualResetEvent manualEvent = new ManualResetEvent(false);
			waitEvents[CurrentThreadsCount] = manualEvent;

			Pack<T> pack = new Pack<T>(manualEvent, Execute);
			pack.Start(modEvents.Dequeue());

			CurrentThreadsCount++;
		}

		#endregion
	}

}
