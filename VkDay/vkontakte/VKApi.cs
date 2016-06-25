using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
using System.Xml;
using System.Threading;
using System.Collections;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using VKLib.Model;
using VKLib;

namespace VKLib
{

	public class VKApi
	{
		public enum ConnectionState
		{
			None,
			Loaded,
			Failed
		}


		private DownloadProgressChangedEventHandler modDownloadFileProgressChanged;
		public event Action<VKApi.ConnectionState> OnConnectionStateChanged;
		private AccessDataInfo acessInfo;

		public int UserId { get { return AcessInfo.UserId; } }
		public string AccessToken { get { return AcessInfo.AccessToken; } }
		public ConnectionState State { get; private set; }

		private AccessDataInfo AcessInfo
		{
			get
			{
				if (acessInfo == null)
					acessInfo = new AccessDataInfo(int.MinValue, null);

				return acessInfo;
			}
		}

		public void Init(AccessDataInfo acessInfo)
		{
			this.acessInfo = acessInfo;

			ChangeConnectionState(ConnectionState.Loaded);
		}

		private void ChangeConnectionState(VKApi.ConnectionState state)
		{
			State = state;

			if (OnConnectionStateChanged == null)
				return;

			OnConnectionStateChanged(state);
		}

		private XmlDocument ExecuteCommand(string name, NameValueCollection param)
		{
			XmlDocument result = new XmlDocument();
			System.Net.WebClient downloader = new System.Net.WebClient();
			FileStream fileStream = new FileStream(name, FileMode.Open, FileAccess.Read);

			try
			{
				string str = MakeQueryString(name, AccessToken, param);

				downloader.DownloadProgressChanged += modDownloadFileProgressChanged;
				downloader.DownloadFileAsync(new Uri(str), name);

				while (true)
					if (!downloader.IsBusy) break;


				result.Load(fileStream);

			}
			catch (Exception ex)
			{
				State = ConnectionState.Failed;
			}
			finally
			{
				downloader.DownloadProgressChanged -= modDownloadFileProgressChanged;
				downloader.Dispose();
				fileStream.Dispose();
			}
			return result;
		}



		public string GetDataFromXMLNode(XmlNode node)
		{
			if (node == null || String.IsNullOrEmpty(node.InnerText))
			{
				return "нету данных";
			}
			else return node.InnerText;
		}

		public void SetDownloadXMLProgressChanged(DownloadProgressChangedEventHandler value)
		{
			this.modDownloadFileProgressChanged = new DownloadProgressChangedEventHandler(value);
		}


		private string MakeQueryString(string name, string accessToken, NameValueCollection param)
		{
			return String.Format("https://api.VKLib.ru/method/{0}.xml?access_token={1}&{2}",
									name,
									accessToken,
									String.Join("&", SelectItem(param)));
		}

		//private XmlDocument ExecuteCommand(string name)
		//{
		//		XmlDocument result = new XmlDocument();

		//		result.Load(String.Format("https://api.VKLib.ru/method/{0}.xml?access_token={1}&{2}",
		//								name,
		//								AccessToken));

		//		return result;
		//}

		private string[] SelectItem(NameValueCollection qs)
		{
			List<string> str = new List<string>();

			foreach (string obj in qs.AllKeys)
			{
				str.Add(obj + "=" + qs[obj]);
			}

			return str.ToArray();
		}

		//Old methods
		//public XmlDocument GetProfile(int uid)
		//{
		//		NameValueCollection qs = new NameValueCollection();
		//		qs["uid"] = uid.ToString();
		//		qs["fields"] = "uid, first_name, last_name, nickname, sex, bdate (birthdate), city, country, timezone,"+
		//									 "photo, photo_medium, photo_big, photo_rec, connections";
		//		return ExecuteCommand("users.get", qs);
		//}
		//public XmlDocument GetFriends(int uid)
		//{
		//		NameValueCollection qs = new NameValueCollection();
		//		qs["uid"] = uid.ToString();
		//		qs["fields"] = "uid,first_name,last_name,nickname,domain,sex,bdate,city,country,timezone,photo,has_mobile,rate,contacts,education,online";
		//		return ExecuteCommand("friends.get", qs);
		//}

		//public void PublishOnTheWall(int uid, string message)
		//{
		//		NameValueCollection collection = new NameValueCollection();
		//		collection["owner_id"] = uid.ToString();
		//		collection["message"] = message;
		//		ExecuteCommand("wall.post", collection);
		//}
		//#region Photos
		//public XmlDocument GetAllAlbums(int uid, bool isGroup)
		//{
		//		NameValueCollection qs = new NameValueCollection();
		//		qs["owner_id"] = ((isGroup) ? "-" : "") + uid.ToString();
		//		qs["need_covers"] = "1";
		//		qs["fileds"] = "owner_id,need_covers";
		//		return ExecuteCommand("photos.getAlbums", qs);
		//}

		//public XmlDocument GetPhotosFromPage(int uid)
		//{
		//		NameValueCollection qs = new NameValueCollection();
		//		qs["uid"] = uid.ToString();
		//		qs["fileds"] = "uid,extended";
		//		return ExecuteCommand("albums.getProfile", qs);
		//}

		//public XmlDocument GetPhotosFromAlbum(int owner_id, int album_id) 
		//{
		//		NameValueCollection qs = new NameValueCollection();
		//		qs["owner_id"] = owner_id.ToString();
		//		qs["album_id"] = album_id.ToString();
		//		qs["fields"] = "owner_id,album_id";
		//		return ExecuteCommand("photos.get", qs);
		//}

		//public XmlDocument GetAudioCountFromUser(int uid, bool isGroup)
		//{
		//		NameValueCollection qs = new NameValueCollection();
		//		qs.Add("owner_id", ((isGroup) ? "-" : "") + uid);
		//		qs.Add("fields", "uid,aid");
		//		return ExecuteCommand("audio.getCount", qs);
		//}

		//public XmlDocument GetAudioFromUser(int uid, bool isGroup, int offset,int counts) 
		//{
		//		NameValueCollection qs = new NameValueCollection();
		//		qs.Add("owner_id", ((isGroup) ? "-" : "") + uid);
		//		qs.Add("offset",offset.ToString());
		//		qs.Add("count",counts.ToString());
		//		qs.Add("fields", "owner_id,offset,count");
		//		return ExecuteCommand("audio.get", qs);
		//}

		//public XmlDocument SendAudioToUserWall(int ownerId, int audioId) 
		//{
		//		NameValueCollection qs = new NameValueCollection();
		//		qs.Add("owner_id", ownerId.ToString());
		//		qs.Add("audio_id",audioId.ToString());
		//		qs.Add("fields", "owner_id,audio_id");
		//		return ExecuteCommand("audio.get", qs);
		//}

		//public XmlDocument SendAudioToGroupWall(int groupId, int audioId) 
		//{
		//		NameValueCollection qs = new NameValueCollection();
		//		qs.Add("owner_id", groupId.ToString());
		//		qs.Add("audio_id",audioId.ToString());
		//		qs.Add("fields", "owner_id,audio_id");
		//		return ExecuteCommand("audio.get", qs);
		//}

		//public XmlDocument GetAudioRecomendation(int uid, bool isGroup, int offset, int counts) 
		//{
		//		NameValueCollection qs = new NameValueCollection();
		//		qs.Add("owner_id", ((isGroup) ? "-" : "") + uid.ToString());
		//		qs.Add("offset",offset.ToString());
		//		qs.Add("count",counts.ToString());
		//		qs.Add("fields", "uid,aid");
		//		return ExecuteCommand("audio.getRecommendations", qs);
		//}

		//public XmlDocument GetAudioUpload()
		//{
		//		return ExecuteCommand("audio.getUploadServer");
		//}

		//public XmlDocument GetPhotosUploadToWall(int uid, bool isGroup)
		//{
		//		NameValueCollection qs = new NameValueCollection();
		//		qs.Add("group_id", ((isGroup) ? "-" : "") + uid.ToString());
		//		qs.Add("fields", "group_id");
		//		return ExecuteCommand("photos.getWallUploadServer", qs);
		//}

		//#endregion

	}

	#region Глобальные перечисления

	#endregion

}
