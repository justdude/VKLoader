using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VkDay.Model
{
	public class Profile
	{
		public int uid { get; set; }
		public string first_name { get; set; }
		public string last_name { get; set; }
		public string nickname { get; set; }
		public string sex { get; set; }
		public string birthdate { get; set; }
		public string city { get; set; }
		public string country { get; set; }
		public string photo { get; set; }
		public string photoMedium { get; set; }
		public string photoBig { get; set; }
		public string mobile_phone { get; set; }
		public string home_phone { get; set; }

		public string FullName
		{
			get
			{
				return first_name + " " + last_name;
			}
		}

		public Profile()
		{
			first_name = "";
			last_name = "";
			nickname = "";
			sex = "";
			birthdate = "";
			city = "";
			country = "";
			photo = "";
			photoMedium = "";
			photoBig = "";
		}

		public Profile(int id, string first_name, string last_name)
		{

			first_name = "";
			last_name = "";
			nickname = "";
			sex = "";
			birthdate = "";
			city = "";
			country = "";
			photo = "";
			photoMedium = "";
			photoBig = "";

			this.uid = id;
			this.first_name = first_name;
			this.last_name = last_name;
		}

		public Profile(int id)
		{

			first_name = "";
			last_name = "";
			nickname = "";
			sex = "";
			birthdate = "";
			city = "";
			country = "";
			photo = "";
			photoMedium = "";
			photoBig = string.Empty;

			this.uid = id;
			this.first_name = string.Empty;
			this.last_name = string.Empty;
		}


		public string GetMaxPhotoSize()
		{
			var paths = (new List<string>() 
						{ 
							photo, 
							photoMedium, 
							photoBig });

			var leng = paths.Max(p => p.Length);

			return paths.FirstOrDefault(p => p.Length == leng);
		}

		public override string ToString()
		{
			return this.first_name + " " + this.last_name;
		}


		public bool IsHasNonEmptyNumbers 
		{ 
			get
			{
				return string.IsNullOrEmpty(home_phone)==false 
			|| string.IsNullOrEmpty(mobile_phone)==false;
			} 
		}

		//public bool IsPhone(string text)
		//{

		//}

		public string PhoneOut 
		{ 
			get
			{
				//string mphone = IsPhone(mobile_phone)? mobile_phone : string.Empty;

				return FullName + "(" + uid + ")" + " : " + mobile_phone + " ; " + home_phone;
			} 
		}

	}
}
