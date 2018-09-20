using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace PUCIT.AIMRL.Common
{
    public static class WinUtility
    {
        public static String WinBasepath = System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath);

        public static String GetPhysicalPathByRelativePath(String pRelativePath)
        {
            return Path.Combine(WinBasepath, pRelativePath);
        }
    }

    public static class WebUtility
    {
        public static String GetPhysicalPathByVirtualPath(String pVirtualPath)
        {
            return System.Web.HttpContext.Current.Server.MapPath(VirtualPathUtility.ToAbsolute(pVirtualPath));
        }
    }

    public static class HelperMethods
    {
        public static String ConvertDTToStr(DateTime dt)
        {
            return dt.ToString("dd-MM-yy h:mm:ss tt");
        }

        public static String ConvertOnlyDateToStr(DateTime dt)
        {
            return dt.Date.ToString("dd-MM-yy");
        }

        public static String ChangeDTFormat (DateTime dt)
        {
            return dt.ToString("dd-MM-yy hh:mm tt");
        }
    }

    public static class MyDateTimeExtension
    {
        

        public static String YYYYMMDD(this DateTime dt)
        {
            return dt.ToString("yyyy-MM-dd HH:mm:ss.fff");
        }

        public static DateTime ToTimeZoneTime(this DateTime time, string timeZoneId = "Pakistan Standard Time")
        {
            TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
            return time.ToTimeZoneTime(tzi);
        }
        public static DateTime ToTimeZoneTime(this DateTime time, TimeZoneInfo tzi)
        {
            return TimeZoneInfo.ConvertTimeFromUtc(time, tzi);
        }
    }

    public static class PasswordSaltedHashingUtility
    {
        private static byte[] salt = { 23, 128, 56, 98, 45, 76, 34, 98, 114, 203, 118, 23, 10, 71, 178, 215 };

        public static String HashPassword(String Password)
        {
            var pbkdf2 = new Rfc2898DeriveBytes(Password, salt, 10000);
            byte[] hash = pbkdf2.GetBytes(20);
            byte[] hashBytes = new byte[36];
            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 20);
            return Convert.ToBase64String(hashBytes);
        }

    }
}
