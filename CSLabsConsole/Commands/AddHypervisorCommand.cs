using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CommandLine;
using CSLabsBackend.Config;
using CSLabsBackend.Models;
using CSLabsBackend.Proxmox;

namespace CSLabsConsole.Commands
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
            _context.Hypervisors.Add(new Hypervisor
            {
                Host = options.Host,
                UserName = options.UserName,
                NoVncUrl = options.NoVncUrl,
                Password = Cryptography.EncryptString(options.Password, _encryptionKey)
            });
            await _context.SaveChangesAsync();
        }
    }
}