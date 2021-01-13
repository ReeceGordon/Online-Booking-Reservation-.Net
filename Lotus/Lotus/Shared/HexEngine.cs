using System;
using System.Collections.Generic;
using System.Text;

namespace Lotus.Shared
{
    /// <summary>
    /// converts the string to hex
    /// </summary>
    /// <param name="str">the string to be hexed</param>
    /// <returns>hexed string</returns>
    public class HexEngine
    {
        public static string Image64 { get; set; }
        public static string ToHexString(string str)
        {
            var sb = new StringBuilder();

            var bytes = Encoding.Unicode.GetBytes(str);
            foreach (var t in bytes)
            {
                sb.Append(t.ToString("X2"));
            }

            return sb.ToString();
        }
        /// <summary>
        /// converts the hex to string
        /// </summary>
        /// <param name="str">the hex to be converted to string</param>
        /// <returns>normal string</returns>
        public static string FromHexString(string hexString)
        {
            var bytes = new byte[hexString.Length / 2];
            for (var i = 0; i < bytes.Length; i++)
            {
                bytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            }

            return Encoding.Unicode.GetString(bytes);
        }
    }
}