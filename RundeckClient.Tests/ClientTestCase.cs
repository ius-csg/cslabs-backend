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
            var client = new RundeckClient.Client("https://", "192.168.1.160:4440", "<API KEY HERE>");
            var response = await client.GetProjects();
            var test = "";
        }
    }
}