using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using VkDay.Model;
using VkEasyPhones.VkDay.Model;
using VkEasyPhones.VkDay.vkontakte.Commands.Command;

namespace VkDay
{
	public class GeolocationCommands
	{
		public const string CountriesCodes = "RU,UA,BY";

		#region Profile
		public List<Country> GetCountries(bool isNoNeedFull, string codes)
		{
			NameValueCollection Params = new NameValueCollection();
			string CommandName;

			if (!isNoNeedFull)
			{ 
			Params.Add("need_full", isNoNeedFull.ToString());
			
			if (string.IsNullOrEmpty(codes) == false)
				Params.Add("code", codes);
			}
			CommandName = "places.getCountries";

			var command = new GeolocationCommand(CommandName, Params);
			command.ExecuteCommand();
			return command.FillCountries();
		}

		public List<City> GetSities(string countryId, string searchingString)
		{
			NameValueCollection Params = new NameValueCollection();
			string CommandName;

			Params.Add("country", countryId);

			if (string.IsNullOrEmpty(searchingString) == false)
				Params.Add("q", searchingString);

			CommandName = "places.getCities";

			var command = new GeolocationCommand(CommandName, Params);
			command.ExecuteCommand();
			return command.FillCities();
		}

		#endregion
	}
}
