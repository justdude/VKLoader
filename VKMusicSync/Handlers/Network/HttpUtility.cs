using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace UrlDecode
{
    public static class HttpUtility
    {
        public static string UrlDecode(string s)
        {
            if (s == null) return null;
            if (s.Length < 1) return s;

            char[] chars = s.ToCharArray();
            byte[] bytes = new byte[chars.Length * 2];
            int count = chars.Length;
            int dstIndex = 0;
            int srcIndex = 0;

            while (true)
            {
                if (srcIndex >= count)
                {
                    if (dstIndex < srcIndex)
                    {
                        byte[] sizedBytes = new byte[dstIndex];
                        Array.Copy(bytes, 0, sizedBytes, 0, dstIndex);
                        bytes = sizedBytes;
                    }
                    return new string(Encoding.UTF8.GetChars(bytes));
                }

                if (chars[srcIndex] == '+')
                {
                    bytes[dstIndex++] = (byte)' ';
                    srcIndex += 1;
                }
                else if (chars[srcIndex] == '%' && srcIndex < count - 2)
                    if (chars[srcIndex + 1] == 'u' && srcIndex < count - 5)
                    {
                        int ch1 = HexToInt(chars[srcIndex + 2]);
                        int ch2 = HexToInt(chars[srcIndex + 3]);
                        int ch3 = HexToInt(chars[srcIndex + 4]);
                        int ch4 = HexToInt(chars[srcIndex + 5]);

                        if (ch1 >= 0 && ch2 >= 0 && ch3 >= 0 && ch4 >= 0)
                        {
                            bytes[dstIndex++] = (byte)((ch1 << 4) | ch2);
                            bytes[dstIndex++] = (byte)((ch3 << 4) | ch4);
                            srcIndex += 6;
                            continue;
                        }
                    }
                    else
                    {
                        int ch1 = HexToInt(chars[srcIndex + 1]);
                        int ch2 = HexToInt(chars[srcIndex + 2]);

                        if (ch1 >= 0 && ch2 >= 0)
                        {
                            bytes[dstIndex++] = (byte)((ch1 << 4) | ch2);
                            srcIndex += 3;
                            continue;
                        }
                    }
                else
                {
                    byte[] charBytes = Encoding.UTF8.GetBytes(chars[srcIndex++].ToString());
                    charBytes.CopyTo(bytes, dstIndex);
                    dstIndex += charBytes.Length;
                }
            }
        }

        private static int HexToInt(char ch)
        {
            return
                (ch >= '0' && ch <= '9') ? ch - '0' :
                (ch >= 'a' && ch <= 'f') ? ch - 'a' + 10 :
                (ch >= 'A' && ch <= 'F') ? ch - 'A' + 10 :
                -1;
        }
    }
}