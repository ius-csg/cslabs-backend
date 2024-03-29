using System;
using System.Threading.Tasks;
using CommandLine;
using CSLabs.Api.Config;
using CSLabs.Api.Models;
using CSLabs.Api.Proxmox;
using Newtonsoft.Json;

namespace CSLabs.ConsoleUtil.Commands
{
    [Verb("change-hypervisor-password", HelpText = "Changes the password for a hypervisor")]
    public class ChangeHypervisorPasswordOptions
    {
        [Option(Required = true, HelpText = "The id of the hypervisor to change the password for")]
        public int Id { get; set; }
        [Option(Required = true, HelpText = "The password to change the password to")]
        public string Password { get; set; }
    }
    
    public class ChangeHypervisorPasswordCommand : AsyncCommand<ChangeHypervisorPasswordOptions>
    {
        private readonly DefaultContext _context;
        private readonly string _encryptionKey;
        
        public ChangeHypervisorPasswordCommand(DefaultContext context, AppSettings appSettings)
        {
            _context = context;
            _encryptionKey = appSettings.ProxmoxEncryptionKey;
        }
        
        public override async Task Run(ChangeHypervisorPasswordOptions options)
        {
            var hypervisor = await _context.Hypervisors.FindAsync(options.Id);
            if (hypervisor == null)
            {
                Console.WriteLine("Could not find a hypervisor with that Id!");
                return;
            }

            hypervisor.Password = Cryptography.EncryptString(options.Password, _encryptionKey);
            await _context.SaveChangesAsync();
            Console.WriteLine("Changed Password:");
            Console.WriteLine(JsonConvert.SerializeObject(hypervisor));
        }
    }
}