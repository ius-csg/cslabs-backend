using System;
using CommandLine;
using CSLabsBackend.Config;
using CSLabsBackend.Proxmox;

namespace CSLabsConsole.Commands
{
    [Verb("decrypt", HelpText = "Decrypts a string")]
    public class DecryptOptions
    {
        [Value(0)]
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
            Console.WriteLine(Cryptography.DecryptString(options.StringToDecrypt, _encryptionKey));
        }
    }
}