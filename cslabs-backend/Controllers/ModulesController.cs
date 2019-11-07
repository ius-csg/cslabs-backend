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
using CSLabsBackend.Models;
using CSLabsBackend.Models.ModuleModels;
using Microsoft.AspNetCore.Mvc;

namespace CSLabsBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ModulesController : BaseController
    {
        public ModulesController(BaseControllerDependencies deps) : base(deps)
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
        
        // GET api/values/5
        [HttpGet("code/{code}")]
        public Module Get(string code)
        {
            return this.DatabaseContext.Modules.First(m => m.SpecialCode == code);
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
