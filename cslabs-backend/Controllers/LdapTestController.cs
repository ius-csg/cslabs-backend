using CSLabsBackend.Models.UserModels;
using CSLabsBackend.Services;
using Microsoft.AspNetCore.Mvc;

namespace CSLabsBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LdapTestController : BaseController
    {
        private LdapService _ldapService;
        
        //Need to create a test user
        public LdapTestController(BaseControllerDependencies deps, LdapService ldapService) : base(deps)
        {
            _ldapService = ldapService;
        }
        
        [HttpGet]
        public IActionResult Index()
        {
            _ldapService.CreateEntry(new User
            {
                SchoolEmail = "testuser@iu.edu"
            }, "TestPassword");
         
            return Ok();
        }

        
    }
}