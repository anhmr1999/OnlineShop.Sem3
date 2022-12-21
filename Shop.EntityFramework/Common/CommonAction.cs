using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shop.EntityFramework.Common
{
    public static class CommonAction
    {
        public static string ToMd5(this string input)
        {
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                var hash = BitConverter.ToString(hashBytes).Replace("-", "");
                return hash;
            }
        }
    }
}