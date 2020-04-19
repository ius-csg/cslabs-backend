using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CSLabs.Api.Models.HypervisorModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Renci.SshNet;
using ConnectionInfo = Renci.SshNet.ConnectionInfo;
using System.IO.Compression;
using System.Xml;
using System.Xml.Serialization;
using CSLabs.Api.RequestModels;
using CSLabs.Api.Services;
using CSLabs.Api.Util;
using Renci.SshNet.Sftp;

namespace CSLabs.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class VmTemplateController : BaseController
    {
        private ProxmoxVmTemplateService _vmTemplateService;
        
        public VmTemplateController(BaseControllerDependencies dependencies, ProxmoxVmTemplateService vmTemplateService) : base(dependencies)
        {
            _vmTemplateService = vmTemplateService;
        }

        [HttpGet]
        public async Task<IActionResult> GetTemplates()
        {
            var query = DatabaseContext.VmTemplates
                .Where(t => !t.IsCoreRouter);
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
            Console.WriteLine("Got upload file");
           IFormFile ovaFile = request.File;
           var hypervisor = await DatabaseContext.Hypervisors.FirstOrDefaultAsync();
            var vmTemplate = new VmTemplate
            {
                Name = ovaFile.FileName,
                Owner = GetUser(),
                IsCoreRouter = false,
            };
            await DatabaseContext.Database.BeginTransactionAsync();
            DatabaseContext.Add(vmTemplate);
            await DatabaseContext.SaveChangesAsync();
            var templateId = await _vmTemplateService.UploadVmTemplate(ovaFile, hypervisor, vmTemplate);
            var primaryHypervisorNode = await ProxmoxManager.GetPrimaryHypervisorNode(hypervisor);
            vmTemplate.HypervisorVmTemplates = new List<HypervisorVmTemplate>
            {
                new HypervisorVmTemplate
                {
                    HypervisorNode = primaryHypervisorNode,
                    TemplateVmId = templateId
                }
            };
            await DatabaseContext.SaveChangesAsync();
            DatabaseContext.Database.CommitTransaction();
            return Ok(vmTemplate);
        }
        
    }
}