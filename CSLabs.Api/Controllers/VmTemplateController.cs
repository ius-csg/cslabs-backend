using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CSLabs.Api.RequestModels;
using CSLabs.Api.Services;

namespace CSLabs.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class VmTemplateController : BaseController
    {
        private ProxmoxVmTemplateService _vmTemplateService;
        private UrlBasedUploadManager _uploadManager;
        public VmTemplateController(
            BaseControllerDependencies dependencies, 
            ProxmoxVmTemplateService vmTemplateService, 
            UrlBasedUploadManager uploadManager) : base(dependencies)
        {
            _vmTemplateService = vmTemplateService;
            _uploadManager = uploadManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetTemplates()
        {
            var query = DatabaseContext.VmTemplates.AsQueryable();
            if (!GetUser().IsAdmin())
                query = query.Where(t => t.Owner == GetUser() || t.Owner == null);
            return Ok(await query.ToListAsync());
        }

        const long TEN_GB = 1024L * 1024L * 1024L * 10L;
            
        [HttpPost]
        [RequestFormLimits(MultipartBodyLengthLimit = TEN_GB)]
        [RequestSizeLimit(TEN_GB)]
        public async Task<IActionResult> UploadTemplate([FromForm] FileUploadRequest request)
        {
            await using var stream = request.File.OpenReadStream();
            
            await _vmTemplateService.UploadTemplate(DatabaseContext, request.Name, GetUser(), stream, request.File.Length);
            return Ok();
        }
        [HttpPost("from-url")]
        public async Task<IActionResult> UploadTemplateFromUrl([FromBody] FromUrlRequest request)
        {
            string requestId = Guid.NewGuid().ToString();
            _uploadManager.SetProgress(requestId, 0);
            _uploadManager.QueueUpload(request, requestId, GetUser());
            return Ok(requestId);
        }

        [HttpGet("upload-status/{requestId}")]
        public async Task<IActionResult> GetUploadStatus(string requestId)
        {
            return Ok(_uploadManager.GetProgress(requestId));
        }

        [HttpPost("test-upload")]
        [AllowAnonymous]
        public async Task<IActionResult> TestUpload()
        {
            var hypervisor = await DatabaseContext.Hypervisors.FirstOrDefaultAsync();
            await _vmTemplateService.TestConnect(hypervisor);
            return Ok();
        }
    }
}