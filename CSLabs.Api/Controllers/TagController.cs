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
using CSLabs.Api.Models.UserModels;
using CSLabs.Api.RequestModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CSLabs.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TagController : BaseController
    {
        public TagController(BaseControllerDependencies deps) : base(deps)
        {
        }
        
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(DatabaseContext.Tags.ToList());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            if (DatabaseContext.Tags.Any(t => t.Id == id)) return BadRequest();
            var tag = await this.DatabaseContext.Tags.FirstAsync(t => t.Id == id);
            return Ok(tag);
        }
    }
}