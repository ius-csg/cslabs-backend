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
using Microsoft.AspNetCore.Mvc;

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
        public Module Get(int id)
        {
            return this.DatabaseContext.Modules.Find(id);
        }
        
        [HttpGet("code/{code}")]
        public IActionResult Get(string code)
        {
            var module =  this.DatabaseContext.Modules.First(m => m.SpecialCode == code);
            var userModule = DatabaseContext.UserModules.FirstOrDefault(m => m.UserId == GetUser().Id && m.ModuleId == module.Id);
            if (userModule != null) {
                module.UserModuleId = userModule.Id;
            }
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
