using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;

namespace RundeckClient.Tests
{
    public class ClientTestCase
    {
        private Client client;
        private string testJobId;
        
        [SetUp]
        public void Setup()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false,  true);

            IConfiguration configuration = builder.Build();
            var apiSection = configuration.GetSection("Api");
            this.testJobId = apiSection["TestJobId"];
            client = new Client(apiSection["Scheme"], apiSection["Host"], apiSection["ApiKey"]);
        }

        [Test]
        public async Task TestGetProjects()
        {
            var projects = await client.GetProjects();
            Assert.GreaterOrEqual(projects.Count, 1);
        }
        
        [Test]
        public async Task TestGetJobs()
        {
            var projects = await client.GetProjects();
            Assert.GreaterOrEqual(projects.Count, 1);
            var jobs = await client.GetJobs(projects.First().Url);
            Assert.GreaterOrEqual(jobs.Count, 1);
        }
        
        [Test]
        public async Task TestRunJob()
        {
            var execution = await client.RunJob(testJobId);
            while (execution.Status == "running")
            {
                execution = await client.GetExecution(execution.Id);
            }
            
            Assert.AreEqual("succeeded", execution.Status);
            var output = await client.GetExecutionOutput(execution.Id);
            Assert.GreaterOrEqual(output.Length, 1);
        }
    }
}