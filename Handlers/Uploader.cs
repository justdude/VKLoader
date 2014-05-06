using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Net;
using System.IO;
namespace VK
{
    class Uploader
    {
        public Uploader(string path)
        {
        }

        public static HttpWebResponse PostMethod(string postedData, string postUrl)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(postUrl);
            request.Method = "POST";
            request.Credentials = CredentialCache.DefaultCredentials;

            UTF8Encoding encoding = new UTF8Encoding();
            var bytes = encoding.GetBytes(postedData);

            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = bytes.Length;

            using (var newStream = request.GetRequestStream())
            {
                newStream.Write(bytes, 0, bytes.Length);
                newStream.Close();
            }
            return (HttpWebResponse)request.GetResponse();
        }

        public string GetResponse(HttpWebResponse response)
        {
            if (response != null)
            {
              var strreader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
              return strreader.ReadToEnd();

            }
            return "";
        }

        public string GetResponse(HttpWebResponse response, Encoding encoding)
        {
            if (response != null)
            {
                var strreader = new StreamReader(response.GetResponseStream(), encoding);
                return strreader.ReadToEnd();

            }
            return "";
        }


    }
}
