using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CSLabsBackend.Proxmox;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using Rundeck;

namespace Rundeck.Tests
{
    public class ProxmoxApiTestCase
    {
        private ProxmoxApi client;

        [SetUp]
        public void Setup()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false,  true);

            IConfiguration configuration = builder.Build();
            var apiSection = configuration.GetSection("Api");
            client = new ProxmoxApi(apiSection["Host"], apiSection["Username"], apiSection["Password"]);
        }

        [Test]
        public async Task TestGetTicket()
        {
            var ticketResponse = await client.GetTicket("a1", 104);
            Assert.NotNull(ticketResponse.Ticket);
            Assert.Greater(ticketResponse.Ticket.Length, 1);
        }
        
        [Test]
        public async Task TestGetVMStatus()
        {
            var ticketResponse = await client.GetVmStatus("a1", 104);
            Assert.NotNull(ticketResponse);
        }
        
        [Test]
        public async Task TestCloneTemplate()
        {
            var newVmId = await client.CloneTemplate("a1", 103);
            Assert.NotNull(newVmId);
        }
        
    }
}