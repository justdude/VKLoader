using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using Id3;
using Id3.Info;
using Id3.Id3v1;
using Id3.Id3v2;
using Id3.Frames;

using VKMusicSync.Model;
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace VKMusicSync.Handlers
{
    public class TagReader
    {

        public static void ComputeFromFileName(string path, Sound sound)
        {
            if (!File.Exists(path))
                return;

           
            string[] separators = { "-", ":", "_" };

            path = path.Replace(sound.FileExtention, "");

            var res = Path.GetFileName(path).Split(separators, StringSplitOptions.RemoveEmptyEntries);

            if (res.Any())
            {
                if (res.Length > 1)
                    sound.title = string.IsNullOrWhiteSpace(res[1]) ? sound.title : res[1].Trim();

                sound.artist = string.IsNullOrWhiteSpace(res[0]) ? sound.title : res[0].Trim();
            }

        }

        // Unicode string.
        public static string EncodeString(byte[] input)
        {
            var enc = Encoding.GetEncoding("iso-8859-1");
            return Encoding.UTF8.GetString(Encoding.Convert(enc, Encoding.UTF8, input));
        }

        public static void Read(string path, Sound sound)
        {
            if (!File.Exists(path))
                return;

            File.SetAttributes(path, FileAttributes.Normal);

            try
            {
                Stream stream = new FileStream(path, FileMode.Open, FileAccess.Read);
                Mp3Stream mp3 = new Mp3Stream(stream);

                if (!mp3.HasTags)
                {
                    stream.Close();
                    ComputeFromFileName(path, sound);
                    return;
                }

                //mp3.GetAudioStream().
                var tags = mp3.GetAllTags();

                foreach (var tag in tags)
                    if (tag != null)
                    {
                        if (string.IsNullOrWhiteSpace(tag.Artists.Value) && string.IsNullOrWhiteSpace(tag.Title.Value))
                        {
                            ComputeFromFileName(path, sound);
                        }
                        else
                        {
                          byte[] input = null;

                          if (!string.IsNullOrWhiteSpace(tag.Title.Value))
                          {
                              input = tag.Title.Encode();
                              sound.title = ConvertEncoding(tag.Artists.Value, input, Id3TextEncoding.Iso8859_1);
                          }

                          if (!string.IsNullOrWhiteSpace(tag.Artists))
                          {
                              input = tag.Artists.Encode();
                              sound.artist = ConvertEncoding(tag.Artists.Value, input, Id3TextEncoding.Iso8859_1);
                          }
                        }

                        GetPicture(tag);
                        //int.TryParse(tag.BeatsPerMinute.Value, out modelView.duration);
                        break;
                    }
                stream.Close();

            }//try
            catch (Exception)
            {
                ComputeFromFileName(path, sound);
            }

        }

        private static string ConvertEncoding(string convertedStr,  byte[] input, Id3TextEncoding type)
        {
            switch (type)
            {
                case Id3TextEncoding.Unicode:
                    {
                        return convertedStr;
                    }
                case Id3TextEncoding.Iso8859_1:
                    {
                        return Encoding.GetEncoding("windows-1251").GetString(input).Replace("\0", "");
                    }
            }
            return convertedStr;
        }


        public static void Read(string path, ModelView.SoundModelView modelView)
        {
            if (!File.Exists(path))
                return;

            File.SetAttributes(path, FileAttributes.Normal);

            try
            {
                Stream stream = new FileStream(path, FileMode.Open, FileAccess.Read);
                Mp3Stream mp3 = new Mp3Stream(stream);

                if (!mp3.HasTags)
                {
                    stream.Close();
                    ComputeFromFileName(path, modelView.Sound);
                    return;
                }

                var tags = mp3.GetAllTags();

                foreach (var tag in tags)
                    if (tag != null)
                    {
                        modelView.Sound.artist = tag.Artists.Value;
                        modelView.Sound.title = tag.Title.Value;
                        modelView.Photo = GetPicture(tag);
                        //int.TryParse(tag.BeatsPerMinute.Value, out modelView.duration);
                        break;
                    }
                stream.Close();

            }//try
            catch (Exception)
            {
                ComputeFromFileName(path, modelView.Sound);
            }

        }

        public static BitmapImage BitmapImageFromByteArray(Byte[] bytes)
        {
            MemoryStream stream = new MemoryStream(bytes);
            stream.Seek(0, SeekOrigin.Begin);
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            image.StreamSource = stream;
            image.EndInit();
            return image;
        }


        private static BitmapImage GetPicture(Id3Tag tag)
        {
            var picture = tag.Pictures.FirstOrDefault(p => 
                       p.PictureType == PictureType.BandOrOrchestra
                    || p.PictureType == PictureType.BandOrArtistLogotype
                    || p.PictureType == PictureType.BackCover
                    || p.PictureType == PictureType.FrontCover);

            if (picture != null)
            {
                var buffer = picture.PictureData;

               return BitmapImageFromByteArray(buffer);
            }
            return null;
        }

    }
}
