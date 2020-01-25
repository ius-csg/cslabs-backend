using System;
using System.IO;
using FluentEmail.Core;

namespace CSLabs.Api.Util
{
    public static class EmailExtensions
    {
        public static IFluentEmail UsingTemplateFile<T>(this IFluentEmail email, string filename, T model, bool isHtml = true)
        {
            return email.UsingTemplateFromFile($"{Environment.CurrentDirectory}/Views/{filename}", model, isHtml);
        }
    }
}