using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CSLabs.Api.Models;
using CSLabs.Api.Models.Enums;
using CSLabs.Api.Services;
using NUnit.Framework;

namespace CSLabs.Tests
{
    
    public class SystemMessageControllerTest : DatabaseTest
    {
        private SystemMessageService MessageService { get; set; }
        private MockTimeService MockTimeService { get; set; }
        private List<SystemMessage> Messages { get; set; }
        private DateTime Now { get; set; }
        public override async Task Setup()
        {
            await base.Setup();
            MockTimeService = new MockTimeService();
            Now = DateTime.UtcNow;
            MockTimeService.SetTime(Now);
            MessageService = new SystemMessageService(_context, MockTimeService);
            Messages = new List<SystemMessage>
            {
                MakeMessage(Now.AddMinutes(-5), Now.AddMinutes(5)),
                MakeMessage(Now.AddMinutes(-5), Now.AddMinutes(-1)),
                MakeMessage(Now.AddMinutes(1), Now.AddMinutes(2))
            };
            await _context.AddRangeAsync(Messages);
            await _context.SaveChangesAsync();
            _context.DetachAllEntities();
        }
       
        [Test]
        public async Task TestGetMessages_ShouldFilterBeforeAndAfter()
        {
            var activeMessages = await MessageService.GetActiveMessages();
            Assert.AreEqual(1, activeMessages.Count, "One message is returned");
            Assert.AreEqual(Messages.First().Id, activeMessages.First().Id, "The First message should be active");
        }
        
        [Test]
        public async Task TestGetMessages_ShouldGetMultiple()
        {
            MockTimeService.SetTime( Now.AddMinutes(-2));
            var activeMessages = await MessageService.GetActiveMessages();
            Assert.AreEqual(2, activeMessages.Count, "The first two messages should show");
        }
        
        [Test]
        public async Task TestGetMessages_ShouldGetNone()
        {
            MockTimeService.SetTime(Now.AddMinutes(6));
            var activeMessages = await MessageService.GetActiveMessages();
            Assert.AreEqual(0, activeMessages.Count, "No messages should return");
        }
        
        
        
        private SystemMessage MakeMessage(DateTime startTime, DateTime endTime)
        {
            return new SystemMessage
            {
                Type = ESystemMessageType.Info,
                Description = "Test Message",
                CreatedAt = DateTime.UtcNow,
                StartTime = startTime,
                EndTime =  endTime,
                UpdatedAt = DateTime.UtcNow
            };
        }
    }
}