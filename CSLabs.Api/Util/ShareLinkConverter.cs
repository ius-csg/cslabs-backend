using System;
using System.Text.RegularExpressions;

namespace CSLabs.Api.Util
{
    public class ShareLinkConverter
    {
        public static string ConvertUrl(string url)
        {
            if (url.Contains("1drv.ms"))
                return ConvertOneDrive(url);
            if (url.Contains("drive.google.com/file"))
                return ConvertGoogleDrive(url);
            return url;
        }

        public static string ConvertOneDrive(string url)
        {
            string base64Value = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(url));
            var baseUrl = "https://api.onedrive.com/v1.0/shares/";
            string encoded = base64Value.TrimEnd('=').Replace('/', '_').Replace('+', '-');
            return baseUrl + "u!" + encoded + "/root/content";
        }
        public static string ConvertGoogleDrive(string url)
        {
            var match = Regex.Match(url, @"\/file\/d\/([^\/]+)");
            var id =  match.Groups[1].Value;
            return $"https://drive.google.com/uc?export=download&id={id}";
        }
    }
}