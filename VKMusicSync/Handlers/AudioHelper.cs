﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VKSyncMusic.Model;
using VKSyncMusic.ModelView;
namespace VKSyncMusic.Handlers
{
    public class ItemHelper
    {
        public static void FillLastInfo(List<Sound> sounds, DotLastFm.LastFmApi lastFmApi)
        {
            foreach(var item in sounds)
            {
                try
                {
                    var artist = lastFmApi.Artist.GetInfo(item.artist);
                    item.authorPhotoPath = artist.Images[2].Value; // little spike 
                }
                catch(DotLastFm.Api.Rest.LastFmApiException ex)
                {

                }
            }
        }

        /*public static void FillLastInfo(SoundModelView mvSounds, DotLastFm.LastFmApi lastFmApi)
        {
                try
                {
                    var artist = lastFmApi.Artist.GetInfo(mvSounds.Artist);
                    mvSounds.PhotoPath = artist.Images[3].Value;// little spike 
                }
                catch (DotLastFm.Api.Rest.LastFmApiException ex)
                {

                }
        }*/


        public static void FillDataInfo(List<VKSyncMusic.Model.Sound> items)
        {
            double coef = 1024 * 1024;
            foreach (var item in items)
                item.Size = ComputeSize(item.Path) / coef;
            var res = items;
        }


        public static Int64 ComputeSize(string link)
        {
            System.Net.WebClient client = new System.Net.WebClient();
            client.OpenRead(link);
            return Convert.ToInt64(client.ResponseHeaders["Content-Length"]);
        }
    }
}
