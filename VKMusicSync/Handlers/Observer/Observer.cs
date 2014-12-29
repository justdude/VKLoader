/*
 * Created by SharpDevelop.
 * User: Albantov
 * Date: 09.11.2014
 */
using System;
using System.Collections;
using System.Collections.Generic;

namespace VKMusicSync.Handlers.Observer
{
	/// <summary>
	/// Description of CommandExecutor.
	/// </summary>
	public class Observer : List<IReceiver>
	{

		public Observer(): base()
		{
		}

		public Observer(IEnumerable<IReceiver> items)
			: base(items)
		{
		}

		public Observer(int itemsCount)
			: base(itemsCount)
		{
		}

		public void Send(object target, object sender)
		{
			var items = this;

			foreach(var item in items)
			{
				if (target == item)
					item.HandleRequest(sender);
			}

		}

		public void SendAll(object sender)
		{
			var items = this;

			foreach (var item in items)
			{
				item.HandleRequest(sender);
			}

		}

	}
}
