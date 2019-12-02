using AutoMapper;
using CSLabsBackend.Models;
using CSLabsBackend.Services;
using Microsoft.AspNetCore.Mvc;

namespace CSLabsBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LdapTestController : BaseController
    {
        //Need to create a test user
        public LdapTestController(BaseControllerDependencies deps) : base(deps)
        {
            
        }
        
        [HttpGet]
        public IActionResult Index()
        {
//            CSLabsBackend.Services.Ldap.CreateEntry(testUser);
            return Ok();
        }

        
    }
}