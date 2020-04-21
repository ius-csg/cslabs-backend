using CSLabs.Api.Models.ModuleModels;
using Microsoft.AspNetCore.Http;

namespace CSLabs.Api.RequestModels
{
    public class LabRequest : Lab
    {
        public IFormFile Readme { get; set; }
        public IFormFile Topology { get; set; }
    }
}