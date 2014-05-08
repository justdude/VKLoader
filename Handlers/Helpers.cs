using System;

namespace VKMusicSync{

    public class StringHelper
    {
	    public StringHelper()
	    {
	    }

        public static String ClearSpaces(String str)
        {
            String st = "";
            foreach (var c in str)
                if (c != ' ') st += c;
            return st;

        }

    }
}