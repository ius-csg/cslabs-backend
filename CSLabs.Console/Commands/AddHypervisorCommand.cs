using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CommandLine;
using CSLabs.Api.Config;
using CSLabs.Api.Models;
using CSLabs.Api.Models.HypervisorModels;
using CSLabs.Api.Proxmox;
using Newtonsoft.Json;

namespace CSLabs.Console.Commands
{
    [Verb("add-hypervisor", HelpText = "Adds a hypervisor to the database")]
    public class AddHypervisorOptions
    {
        [Option(Required = true, HelpText = "The host or ip address of the hypervisor")]
        public string Host { get; set; }
        [Option(Required = true, HelpText = "The username used to login to the hypervisor")]
        public string UserName { get; set; }
        [Option(Required = true, HelpText = "The password used to login to the hypervisor")]
        public string Password { get; set; }
        [Option(Required = true, HelpText = "The url used by the frontend to proxy noVNC connections")]
        public string NoVncUrl { get; set; }
    }
    
    public class AddHypervisorCommand : AsyncCommand<AddHypervisorOptions>
    {
        private DefaultContext _context;
        private string _encryptionKey;
        
        public AddHypervisorCommand(DefaultContext context, AppSettings appSettings)
        {
            _context = context;
            _encryptionKey = appSettings.ProxmoxEncryptionKey;
        }
        
        public override async Task Run(AddHypervisorOptions options)
        {
            var hypervisor = new Hypervisor
            {
                Host = options.Host,
                UserName = options.UserName,
                NoVncUrl = options.NoVncUrl,
                Password = Cryptography.EncryptString(options.Password, _encryptionKey)
            };
            _context.Hypervisors.Add(hypervisor);
            await _context.SaveChangesAsync();
            System.Console.WriteLine("Added Hypervisor:");
            System.Console.WriteLine(JsonConvert.SerializeObject(hypervisor));
        }
    }
}