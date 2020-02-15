using System.Collections.Generic;
using System.IO;
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
        public async Task<IActionResult> Post(ContactUsRequest contactRequest)
        {
            var contactEmails = (await DatabaseContext.ContactEmails.ToListAsync())
                .Select(email => new Address(email.Email))
                .ToList();

            await CreateEmail()
                .SendNewContactRequestEmail(contactEmails, contactRequest);
            return Ok();
        }
    }
}