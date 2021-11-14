using System;
using CommandLine;
using CSLabs.Api.Config;
using CSLabs.Api.Proxmox;

namespace CSLabs.ConsoleUtil.Commands
{
    [Verb("encrypt", HelpText = "Encrypts a string")]
    public class EncryptOptions
    {
        [Value(0, Required = true, HelpText = "Specify the string to encrypt")]
        public string StringToEncrypt { get; set; }
    }
    public class EncryptCommand : Command<EncryptOptions>
    {
        private readonly string _encryptionKey;
        
        public EncryptCommand(AppSettings appSettings)
        {
            _encryptionKey = appSettings.ProxmoxEncryptionKey;
        }
        public override void Run(EncryptOptions options)
        {
            Console.WriteLine(Cryptography.EncryptString(options.StringToEncrypt, _encryptionKey));
        }
    }
}