using System;
using System.Threading.Tasks;
using CommandLine;
using CSLabs.Api.Config;
using CSLabs.Api.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace CSLabs.ConsoleUtil.Commands
{
    [Verb("list-hypervisors", HelpText = "Lists hypervisors in the database")]
    public class ListHypervisorsOptions
    { }
    
    public class ListHypervisorsCommand : AsyncCommand<ListHypervisorsOptions>
    {
        private DefaultContext _context;
        private string _encryptionKey;
        
        public ListHypervisorsCommand(DefaultContext context, AppSettings appSettings)
        {
            _context = context;
            _encryptionKey = appSettings.ProxmoxEncryptionKey;
        }
        
        public override async Task Run(ListHypervisorsOptions options)
        {
            var hypervisors = await _context.Hypervisors.ToListAsync();
            Console.WriteLine("Hypervisor:");
            Console.WriteLine(JsonConvert.SerializeObject(hypervisors));
        }
    }
}