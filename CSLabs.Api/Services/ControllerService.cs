using CSLabs.Api.Config;
using CSLabs.Api.Models;

namespace CSLabs.Api.Services
{
    public class ControllerService
    {
        public DefaultContext DefaultContext { get; set; }
        public AppSettings AppSettings { get; set; }
    }
}