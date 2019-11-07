using CSLabsBackend.Config;
using CSLabsBackend.Models;

namespace CSLabsBackend.Services
{
    public class ControllerService
    {
        public DefaultContext DefaultContext { get; set; }
        public AppSettings AppSettings { get; set; }
    }
}