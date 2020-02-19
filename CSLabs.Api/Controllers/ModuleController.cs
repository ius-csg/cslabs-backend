using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using AutoMapper;
using CSLabs.Api.Models.ModuleModels;
using CSLabs.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CSLabs.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ModuleController : BaseController
    {
        public ModuleController(BaseControllerDependencies deps) : base(deps)
        {
        }
        // GET api/values
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(DatabaseContext.Modules.Where(m => m.Published).ToList());
        }

        // GET api/values/5
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> Get(int id)
        {
            var module = await this.DatabaseContext.Modules
                .Where(m => m.Published)
                .FirstAsync(m => m.Id == id);
            if (!User.Identity.IsAuthenticated) return Ok(module);
            await module.SetUserModuleIdIfExists(DatabaseContext, GetUser());
            return Ok(module);
        }
        
        [HttpGet("code/{code}")]
        public async Task<IActionResult> Get(string code)
        {
            var module = await this.DatabaseContext.Modules.FirstAsync(m => m.SpecialCode == code);
            await module.SetUserModuleIdIfExists(DatabaseContext, GetUser());
            return Ok(module);
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
