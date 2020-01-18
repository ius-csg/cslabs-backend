using System;
using CommandLine;
using CSLabsBackend.Config;
using CSLabsBackend.Proxmox;

namespace CSLabsConsole.Commands
{
    [Verb("encrypt", HelpText = "Encrypts a string")]
    public class EncryptOptions
    {
        [Value(0)]
        public string StringToEncrypt { get; set; }
    }
    public class EncryptCommand : Command<EncryptOptions>
    {
        private string _encryptionKey;
        
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