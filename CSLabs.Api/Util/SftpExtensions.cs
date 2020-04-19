using System.Collections;
using System.Collections.Generic;
using System.Xml;
using Renci.SshNet.Sftp;

namespace CSLabs.Api.Util
{
    public static class SftpExtensions
    {
        public static string GetExtension(this SftpFile file)
        {
            return file.Name.Substring(file.Name.LastIndexOf(".") + 1);
        }

        public static IEnumerable<XmlNode> ToIEnumerable(this XmlNodeList nodeList)
        {
            var list = new List<XmlNode>();
            for (int i = 0; i < nodeList.Count; i++)
            {
                list.Add(nodeList.Item(i));
            }

            return list;
        }
    }
}