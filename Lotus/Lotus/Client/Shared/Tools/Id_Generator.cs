using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lotus.Client.Shared.Tools
{
    public class Id_Generator
    {
       public static string Generate(int length)
       {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var stringChars = new char[length];
            var random = new Random();

            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }

            return new String(stringChars);
        }

        public static string GeneratePassword()
        {
            var Specials = "!";
            var Capitals = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            var Numbers = "123456789";
            var Lowers = "abcdefghijklmnopqrstuvwxyz";
            var stringChars = new char[8];
            var random = new Random();

            for (int i = 0; i < stringChars.Length; i++)
            {
                if(i==0 || i == stringChars.Length -1)
                {
                    stringChars[i] = Numbers[random.Next(Numbers.Length)];
                }
                else if(i == 1)
                {
                    stringChars[i] = Capitals[random.Next(Capitals.Length)];
                }
                else if(i == 2 || i == 5)
                {
                    stringChars[i] = Specials[random.Next(Specials.Length)];
                }
                else
                {
                    stringChars[i] = Lowers[random.Next(Lowers.Length)];
                }
                
            }

            return new String(stringChars);
            //return "6L!zz!e7";
        }
    }
}
