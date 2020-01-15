using System;
using System.IO;
using CSLabsBackend.Config;
using CSLabsBackend.Proxmox;
using Microsoft.Extensions.Configuration;

namespace CSLabsConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false,  true);
            IConfiguration configuration = builder.Build();
            var appSettings = new AppSettings();
            configuration.Bind(appSettings);
            
            if (args.Length == 2 && args[0] == "--encrypt")
            {
                string str = args[1];
                Console.WriteLine(Cryptography.EncryptString(str, appSettings.ProxmoxEncryptionKey));
            }
            else if (args.Length == 2 && args[0] == "--decrypt")
            {
                string str = args[1];
                Console.WriteLine(Cryptography.DecryptString(str, appSettings.ProxmoxEncryptionKey));
            }
            else
            {
                Console.WriteLine(
                    "Usage: \n" +
                    "--encrypt <string to encrypt>\n" +
                    "--decrypt <string to decrypt>"
                );
            }
        }
    }
}