using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using CSLabsBackend.Models;

namespace CSLabsBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserModulesController : BaseController
    {
        public UserModulesController(DefaultContext defaultContext, IMapper mapper) : base(defaultContext, mapper)
        {
        }
        // GET api/values
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(DatabaseContext.UserModules.ToList());
        }
        
        //Get api
        [HttpGet("{id}")]
        public UserModule Get(int id)
        {
            return this.DatabaseContext.UserModules.Find(id);
        }
    }
}