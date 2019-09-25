using CSLabsBackend.Models;
using Microsoft.AspNetCore.Mvc;

namespace CSLabsBackend.Controllers
{
    public class BaseController: ControllerBase
    {
        protected readonly DefaultContext DatabaseContext;
        
        public BaseController(DefaultContext defaultContext)
        {
            this.DatabaseContext = defaultContext;
        }
    }
}