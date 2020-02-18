using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;

namespace CSLabs.Api.Util
{
    public static class IFormFileExtensions
    {
        public const int ImageMinimumBytes = 512;

        public static List<string> AcceptedImageExtensions => new List<string>
        {
            ".jpg",
            ".jpeg",
            ".gif",
            ".png"
        };

        public static List<string> AcceptedMimeTypes =>
            new List<string>
            {
                "image/jpg",
                "image/jpeg",
                "image/pjpeg",
                "image/gif",
                "image/x-png",
                "image/png"
            };


        // Code pulled from https://stackoverflow.com/questions/11063900/determine-if-uploaded-file-is-image-any-format-on-mvc
        public static bool IsImage(this IFormFile postedFile)
        {
            //-------------------------------------------
            //  Check the image mime types
            //-------------------------------------------
            var contentType = postedFile.ContentType.ToLower();
            
            if (!AcceptedMimeTypes.Contains(contentType))
                return false;
            
            //-------------------------------------------
            //  Check the image extension
            //-------------------------------------------
            var postedFileExtension = Path.GetExtension(postedFile.FileName).ToLower();
           
            if (!AcceptedImageExtensions.Contains(postedFileExtension))
                return false;
            

            //-------------------------------------------
            //  Attempt to read the file and check the first bytes
            //-------------------------------------------
            try
            {
             
                //------------------------------------------
                //   Check whether the image size exceeding the limit or not
                //------------------------------------------ 
                if (postedFile.Length < ImageMinimumBytes)
                {
                    return false;
                }

                byte[] buffer = new byte[ImageMinimumBytes];
                var stream = postedFile.OpenReadStream();
                stream.Read(buffer, 0, ImageMinimumBytes);
                stream.Close();
                string content = System.Text.Encoding.UTF8.GetString(buffer);
                if (Regex.IsMatch(content,
                    @"<script|<html|<head|<title|<body|<pre|<table|<a\s+href|<img|<plaintext|<cross\-domain\-policy",
                    RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Multiline))
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }
    }
}