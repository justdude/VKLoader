using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace VKSyncMusic.Handlers.vkontakte.Commands
{
    public class CommandHelper
    {
        public static bool IsExist(string value)
        {
            if (!string.IsNullOrEmpty(value))
                return true;
            return false;
        }


        public static bool IsExist(int value)
        {
            if (value >= 0)
                return true;
            return false;
        }

        public static bool AddIfExict(NameValueCollection coll, string name, string param, ref string refParams)
        {
            if (IsExist(param))
            {
                refParams += ","+ name;
                coll.Add(name, param);
                return true;
            }
            return false;
        }


    }
}
