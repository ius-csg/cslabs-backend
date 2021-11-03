using System;

namespace CSLabs.Api.Config
{
    public class ConfigurationException : Exception
    {
        public ConfigurationException(string message) : base(message)
        {
        }
    }
}