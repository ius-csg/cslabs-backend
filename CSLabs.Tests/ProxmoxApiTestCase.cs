using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CSLabsBackend.Models;
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
            var hypervisorNode = new HypervisorNode
            {
                Name = "a1",
                Hypervisor = new Hypervisor
                {
                    Host = apiSection["Host"],
                    UserName = apiSection["Username"],
                    Password = apiSection["Password"]
                }
            };
            client = new ProxmoxApi(hypervisorNode);
        }

        [Test]
        public async Task TestGetTicket()
        {
            var ticketResponse = await client.GetTicket(104);
            Assert.NotNull(ticketResponse.Ticket);
            Assert.Greater(ticketResponse.Ticket.Length, 1);
        }
        
        [Test]
        public async Task TestGetVMStatus()
        {
            var ticketResponse = await client.GetVmStatus(104);
            Assert.NotNull(ticketResponse);
        }
        
        [Test]
        public async Task TestCloneTemplate()
        {
            var newVmId = await client.CloneTemplate(103);
            Assert.NotNull(newVmId);
        }
        
        [Test]
        public async Task GetNodeStatus()
        {
            var result = await client.GetNodeStatus();
        }

    }
}