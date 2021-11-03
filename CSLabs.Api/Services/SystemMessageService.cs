using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CSLabs.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace CSLabs.Api.Services
{
    public class SystemMessageService
    {
        private DefaultContext Context { get; }
        private ITimeService TimeService { get; }

        public SystemMessageService(DefaultContext context, ITimeService timeService)
        {
            Context = context;
            TimeService = timeService;
        }
        public async Task<List<SystemMessage>> GetActiveMessages()
        {
            var currentDateTimeUtc = TimeService.GetCurrentTimeUtc();
            return await Context.SystemMessages
                .Where(message => message.EndTime > currentDateTimeUtc && message.StartTime < currentDateTimeUtc)
                .ToListAsync();
        }
    }
}