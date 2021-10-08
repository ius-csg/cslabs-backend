using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CSLabs.Api.Models;
using Microsoft.AspNetCore.Authorization;

namespace CSLabs.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SystemMessageController : BaseController
    {

        public SystemMessageController(BaseControllerDependencies deps) : base(deps)
        {
        }

        // GET: api/SystemMessage
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SystemMessage>>> GetSystemMessages()
        {
            return await this.DatabaseContext.SystemMessages.ToListAsync();
        }

        /*// GET: api/SystemMessage/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SystemMessage>> GetSystemMessage(int id)
        {
            var systemMessage = await this.DatabaseContext.SystemMessages.FindAsync(id);

            if (systemMessage == null)
            {
                return NotFound();
            }

            return systemMessage;
            
        }

        // PUT: api/SystemMessage/5
        [HttpPut("{id}")]
        [Authorize] 
         public async Task<IActionResult> PutSystemMessage(int id, SystemMessage systemMessage)
        {
            if (!GetUser().IsAdmin())
            {
                return Forbid("Access denied");
            }

            if (id != systemMessage.Id)
            {
                return BadRequest();
            }

            this.DatabaseContext.Entry(systemMessage).State = EntityState.Modified;

            try
            {
                await this.DatabaseContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SystemMessageExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/SystemMessage
        [HttpPost]
        public async Task<ActionResult<SystemMessage>> PostSystemMessage(SystemMessage systemMessage)
        {
            if (!GetUser().IsAdmin())
            {
                return Forbid("Access denied");
            }

            this.DatabaseContext.SystemMessages.Add(systemMessage);
            await this.DatabaseContext.SaveChangesAsync();

            return CreatedAtAction("GetSystemMessage", new { id = systemMessage.Id }, systemMessage);
        }

        // DELETE: api/SystemMessage/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<SystemMessage>> DeleteSystemMessage(int id)
        {
            if (!GetUser().IsAdmin())
            {
                return Forbid("Access denied");
            }

            var systemMessage = await this.DatabaseContext.SystemMessages.FindAsync(id);
            if (systemMessage == null)
            {
                return NotFound();
            }

            this.DatabaseContext.SystemMessages.Remove(systemMessage);
            await this.DatabaseContext.SaveChangesAsync();

            return systemMessage;
        }

        private bool SystemMessageExists(int id)
        {
            return this.DatabaseContext.SystemMessages.Any(e => e.Id == id);
        }*/
    }
}
