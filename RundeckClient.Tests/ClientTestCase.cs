using NUnit.Framework;

namespace Tests
{
    public class ClientTestCase
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async void Test1()
        {
            var client = new RundeckClient.Client("https://", "***REMOVED***", "<API KEY HERE>");
            var response = await client.GetProjects();
            var test = "";
        }
    }
}