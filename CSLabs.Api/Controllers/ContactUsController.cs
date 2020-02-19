using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using CSLabs.Api.Email;
using CSLabs.Api.Email.ViewModels;
using CSLabs.Api.RequestModels;
using CSLabs.Api.Util;
using FluentEmail.Core;
using FluentEmail.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CSLabs.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class ContactUsController : BaseController
    {
        public ContactUsController(BaseControllerDependencies dependencies) : base(dependencies)
        {
            
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromForm] ContactUsRequest contactRequest)
        {
            var contactEmails = (await DatabaseContext.ContactEmails.ToListAsync())
                .Select(email => new Address(email.Email))
                .ToList();
            var emailComposer = CreateEmail();
            foreach(var file in  contactRequest.Screenshots)
            {
                var attachment = new Attachment
                {
                    Data = new MemoryStream(),
                    Filename = file.FileName,
                    ContentType = file.ContentType
                };
                await file.CopyToAsync(attachment.Data);
                attachment.Data.Position = 0;
                emailComposer.Attach(attachment);
            }
            await emailComposer.SendNewContactRequestEmail(contactEmails, contactRequest);
            return Ok();
        }
    }
}