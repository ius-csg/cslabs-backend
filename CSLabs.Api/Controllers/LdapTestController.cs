using CSLabs.Api.Models.UserModels;
using CSLabs.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace CSLabs.Api.Controllers
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
            // _ldapService.CreateEntry(new User
            // {
            //     SchoolEmail = "testuser@iu.edu"
            // }, "TestPassword");
            _ldapService.Search("cbenston");
            return Ok();
        }

        
    }
}