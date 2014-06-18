﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VKMusicSync.Model
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

        public Profile()
        { 
        }

        public Profile(int id, string first_name, string last_name)
        {
            this.uid = id;
            this.first_name = first_name;
            this.last_name = last_name;
        }

        public override string ToString()
        {
            return this.first_name + " " + this.last_name;
        }

    }
}