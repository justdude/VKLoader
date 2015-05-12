using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VkEasyPhones.VkDay.Model
{
	public enum ProfileFields
	{
		uid,
		first_name,
		last_name,
		nickname,
		sex,
		bdate,
		city,
		country,
		photo_50,
		photo_100,
		photo_200_orig,
		photo_200,
		photo_400_orig,
		photo_max,
		photo_max_orig,
		online,
		lists,
		screen_name,
		has_mobile,
		contacts,
		education,
		universities,
		schools,
		can_post,
		can_see_all_posts,
		can_write_private_message,
		activity,
		last_seen,
		relation,
		counters,
		nickname,
		wall_comments,
		relatives,
		interests,
		movies,
		tv,
		books,
		games,
		about
	}
	public static class ProfileFields
	{
		//public static string ConvertToString<T>(this T typedData, T type, string separator)
		//{
		//	string = string.Empty;

		//	((int)(object)T) == ((int)(object)type)
		//	return string.Empty;
		//}
	}
}
