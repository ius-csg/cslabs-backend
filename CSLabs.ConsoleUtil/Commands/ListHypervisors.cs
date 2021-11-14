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
        private readonly DefaultContext _context;
        
        // Do we need this field? It's totally unread, only ever set in the constructor but never used
        private readonly string _encryptionKey;
        
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