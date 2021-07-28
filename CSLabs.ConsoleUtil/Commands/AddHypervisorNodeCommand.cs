using System;
using System.Threading.Tasks;
using CommandLine;
using CSLabs.Api.Models;
using CSLabs.Api.Models.HypervisorModels;
using Newtonsoft.Json;

namespace CSLabs.ConsoleUtil.Commands
{
    [Verb("add-hypervisor-node", HelpText = "Adds a hypervisor node to the database")]
    public class AddHypervisorNodeOptions
    {
        [Option(Required = true, HelpText = "The proxmox node name")]
        public string Name { get; set; }
        [Option(Required = true, HelpText = "The Id of the hypervisor to link to")]
        public int HypervisorId { get; set; }
    }
    
    public class AddHypervisorNodeCommand : AsyncCommand<AddHypervisorNodeOptions>
    {
        private DefaultContext _context;

        public AddHypervisorNodeCommand(DefaultContext context)
        {
            _context = context;
        }
        
        public override async Task Run(AddHypervisorNodeOptions options)
        {
            var hypervisorNode = new HypervisorNode
            {
                Name = options.Name,
                HypervisorId = options.HypervisorId
            };
            _context.HypervisorNodes.Add(hypervisorNode);
            await _context.SaveChangesAsync();
            Console.WriteLine("Added Hypervisor Node:");
            Console.WriteLine(JsonConvert.SerializeObject(hypervisorNode));
        }
    }
}