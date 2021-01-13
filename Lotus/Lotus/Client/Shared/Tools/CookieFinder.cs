using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lotus.Client.Shared.Tools
{
    public static class CookieFinder
    {
        /// <summary>
        /// used to find the value of a cookie given the key
        /// </summary>
        /// <param name="result">the result of all cookies</param>
        /// <param name="CookieKey">the key of cookie</param>
        /// <returns>the value of the cookie</returns>
        public static string GetCookie(string result, string CookieKey)
        {
            string Value = "";
            if (result.Contains(CookieKey + "="))
            {
                int CookieStart = result.IndexOf(CookieKey + "=");
                int ValueStart = CookieStart + CookieKey.Length + 1;
                for (int index = ValueStart; index < result.Length; index++)
                {
                    if (result[index] != ';')
                    {
                        Value += result[index];
                    }
                    else
                    {
                        return Value;
                    }

                }

                return Value;
            }
            return "";
        }
    }
}