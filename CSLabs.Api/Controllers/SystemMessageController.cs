using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CSLabs.Api.Models;
using CSLabs.Api.Models.ModuleModels;
using CSLabs.Api.RequestModels;
using CSLabs.Api.Util;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CSLabs.Api.Models;

namespace CSLabs.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SystemMessagesController : ControllerBase
    {
        private readonly DefaultContext _context;

        public SystemMessagesController(DefaultContext context)
        {
            _context = context;
        }

        // GET: api/SystemMessages
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SystemMessage>>> GetSystemMessages()
        {
            return await _context.SystemMessages.ToListAsync();
        }

        // GET: api/SystemMessages/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SystemMessage>> GetSystemMessage(int id)
        {
            var systemMessage = await _context.SystemMessages.FindAsync(id);

            if (systemMessage == null)
            {
                return NotFound();
            }

            return systemMessage;
        }

        // PUT: api/SystemMessages/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSystemMessage(int id, SystemMessage systemMessage)
        {
            if (id != systemMessage.Id)
            {
                return BadRequest();
            }

            _context.Entry(systemMessage).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
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

        // POST: api/SystemMessages
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<SystemMessage>> PostSystemMessage(SystemMessage systemMessage)
        {
            _context.SystemMessages.Add(systemMessage);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSystemMessage", new { id = systemMessage.Id }, systemMessage);
        }

        // DELETE: api/SystemMessages/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<SystemMessage>> DeleteSystemMessage(int id)
        {
            var systemMessage = await _context.SystemMessages.FindAsync(id);
            if (systemMessage == null)
            {
                return NotFound();
            }

            _context.SystemMessages.Remove(systemMessage);
            await _context.SaveChangesAsync();

            return systemMessage;
        }
        private bool SystemMessageExists(int id)
        {
            return _context.SystemMessages.Any(e => e.Id == id);
        }
    }
}
