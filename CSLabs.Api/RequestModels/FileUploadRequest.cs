using Microsoft.AspNetCore.Http;

namespace CSLabs.Api.RequestModels
{
    public class FileUploadRequest
    {
        public IFormFile File { get; set; }
    }
}