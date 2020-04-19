using System.IO;
using CSLabs.Api.Services;
using NUnit.Framework;

namespace CSLabs.Tests
{
    public class ParseOvfTest
    {
        [Test]
        public void TestParseOvf()
        {
            var result = ProxmoxVmTemplateService.ParseOvf(File.ReadAllText("TestFiles/test_ovf.ovf"));
        }
    }
}