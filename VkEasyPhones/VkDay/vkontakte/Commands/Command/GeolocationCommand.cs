using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Xml;
using VkDay;
using VkDay.Model;
using VkEasyPhones.VkDay.Model;

namespace VkEasyPhones.VkDay.vkontakte.Commands.Command
{
	public class GeolocationCommand : BaseCommand<ILocation>
	{

		public GeolocationCommand(string CommandName, NameValueCollection Params)
			: base(CommandName, Params)
		{

		}

		public GeolocationCommand(AccessData AccessData, string CommandName, NameValueCollection Params)
			: base(AccessData,
						 CommandName,
						 Params)
		{
		}


		public List<City> FillCities()
		{
			XmlDocument doc = base.xmlRes;
			List<City> data = new List<City>();
			var item = new City();
			System.Xml.XmlNodeList nodes = doc["response"].ChildNodes;
			for (int i = 0; i < nodes.Count; i++)
			{
				item = new City();

				item.cid = XMLNodeHelper.TryParse(nodes[i], "cid");
				item.title = XMLNodeHelper.TryParse(nodes[i], "title");

				data.Add(item);
			}
			return data;
		}

		public List<Country> FillCountries()
		{
			XmlDocument doc = base.xmlRes;
			List<Country> data = new List<Country>();
			var item = new Country();
			System.Xml.XmlNodeList nodes = doc["response"].ChildNodes;
			for (int i = 0; i < nodes.Count; i++)
			{
				item = new Country();

				item.cid = XMLNodeHelper.TryParse(nodes[i], "cid");
				item.title = XMLNodeHelper.TryParse(nodes[i], "title");

				data.Add(item);
			}
			return data;
		}

	}
}
