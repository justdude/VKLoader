using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Net;
using System.IO;
using System.Security.Cryptography;
namespace VkDay.Handlers
{
    public class Reqeust
    {
        public static string GetMD5(byte[] input)
        {
            // создаем объект этого класса. Отмечу, что он создается не через new, а вызовом метода Create
            MD5 md5Hasher = MD5.Create();

            // Преобразуем входную строку в массив байт и вычисляем хэш
            byte[] data = md5Hasher.ComputeHash(input);

            // Создаем новый Stringbuilder (Изменяемую строку) для набора байт
            StringBuilder sBuilder = new StringBuilder();

            // Преобразуем каждый байт хэша в шестнадцатеричную строку
            for (int i = 0; i < data.Length; i++)
            {
                //указывает, что нужно преобразовать элемент в шестнадцатиричную строку длиной в два символа
                sBuilder.Append(data[i].ToString("x2"));
            }
            return sBuilder.ToString();
        }

        /// <summary>
        /// GetPostAnswer метод
        /// </summary>
        /// <param name="upload_url">Адрес запроса</param>
        /// <param name="sendingData">Обьект который отправляем</param>
        /// <param name="FileName">Имя файла</param>
        /// <param name="FileType">Тип файла. например, audio/mp3 </param>
        /// <returns>Строка ответ </returns>
        public static string Post(string upload_url, byte[] sendingData, string FileName, string FileType = "audio/mp3")
        {
            Stream requstedStream;

            string boundary = String.Format("----WebKitFormBoundary{0}", GetMD5(sendingData));
            string templateFile = "--{0}\r\nContent-Disposition: form-value; name=\"{1}\"; filename=\"{2}\"\r\nContent-Type: {3}\r\n\r\n";
            string templateEnd = "--{0}--\r\n\r\n";
            string Name = "file";

            HttpWebRequest Request = (HttpWebRequest)WebRequest.Create(upload_url);
            Request.Method = "POST";
            Request.ContentType = String.Format("multipart/form-data; boundary={0}", boundary);
            requstedStream = Request.GetRequestStream();

            byte[] contentFile = Encoding.UTF8.GetBytes(String.Format(templateFile, boundary, Name, FileName, FileType));
            requstedStream.Write(contentFile, 0, contentFile.Length);
            requstedStream.Write(sendingData, 0, sendingData.Length);

            byte[] _lineFeed = Encoding.UTF8.GetBytes("\r\n");
            requstedStream.Write(_lineFeed, 0, _lineFeed.Length);
            byte[] contentEnd = Encoding.UTF8.GetBytes(String.Format(templateEnd, boundary));
            requstedStream.Write(contentEnd, 0, contentEnd.Length);

            HttpWebResponse webResponse = (HttpWebResponse)Request.GetResponse();
            StreamReader read = new StreamReader(webResponse.GetResponseStream());

            return read.ReadToEnd();
        }

        public static string POST(string Url, string Data)
        {
            WebRequest req = WebRequest.Create(Url);
            req.Method = "POST";
            req.Timeout = 1000;
            req.ContentType = "application/x-www-form-urlencoded";
            byte[] sentData = Encoding.UTF8.GetBytes(Data);
            req.ContentLength = sentData.Length;
            Stream sendStream = req.GetRequestStream();
            sendStream.Write(sentData, 0, sentData.Length);
            sendStream.Close();
            WebResponse res = req.GetResponse();
            Stream ReceiveStream = res.GetResponseStream();
            StreamReader sr = new StreamReader(ReceiveStream, Encoding.UTF8);
            //Кодировка указывается в зависимости от кодировки ответа сервера
            Char[] read = new Char[256];
            int count = sr.Read(read, 0, 256);
            string Out = String.Empty;
            while (count > 0)
            {
                String str = new String(read, 0, count);
                Out += str;
                count = sr.Read(read, 0, 256);
                }
            return Out;
        }


    }
}
