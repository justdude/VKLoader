using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using System.Threading;
using System.ComponentModel;

namespace VKMusicSync.Handlers
{
	public class ProgressArgs
	{
		public ProgressArgs()
		{
			Current = -1;
			Total = -1;
		}

		public ProgressArgs(int current, int total)
		{
			Current = current;
			Total = total;
		}

		/// <summary>
		/// Возвращает максимальное число полученных байтов.
		/// </summary>
		public double Current
		{
			get;
			internal set;
		}

		/// <summary>
		/// bозвращает общее количество байтов в операции по загрузке данных
		/// </summary>
		public double Total
		{
			get;
			internal set;
		}

		public double Progress
		{
			get { return Current / (double)Total; }
		}

	}

}
