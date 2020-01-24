using System;
using CommandLine;
using CSLabs.Api.Config;
using CSLabs.Api.Proxmox;

namespace CSLabs.Console.Commands
{
    [Verb("decrypt", HelpText = "Decrypts a string")]
    public class DecryptOptions
    {
        [Value(0, Required = true, HelpText = "Specify the string to decrypt")]
        public string StringToDecrypt { get; set; }
    }
    public class DecryptCommand : Command<DecryptOptions>
    {
        private string _encryptionKey;
        
        public DecryptCommand(AppSettings appSettings)
        {
            _encryptionKey = appSettings.ProxmoxEncryptionKey;
        }
        public override void Run(DecryptOptions options)
        {
            System.Console.WriteLine(Cryptography.DecryptString(options.StringToDecrypt, _encryptionKey));
        }
    }
}