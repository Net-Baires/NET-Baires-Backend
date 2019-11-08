using System.Threading.Tasks;
using Xunit;
using System.Net.Http;
using FluentAssertions;
using System.Net;
using System.Collections.Generic;
using NetBaires.Data;
using NetBaires.Api.Services.EventBrite;
using System;
using NetBaires.Api.Services.EventBrite.Models;

namespace NetBaires.Api.Tests.Integration
{
    
    public class EventBriteSyncServicesShould : IntegrationTestsBase
    {
        private Data.Event _event;
        public EventBriteSyncServicesShould(CustomWebApplicationFactory<Startup> factory) : base(factory)
        {
            AuthenticateAdminAsync().GetAwaiter().GetResult();
            FillData();
        }

        private void FillData()
        {
            _event = new Data.Event
            {
                EventId = "1234",
                Platform = Data.EventPlatform.EventBrite,
                Attendees = new List<Attendance>{
                    new Attendance{
                        Member = new Member{
                            Email="primero@primero.com"
                        }
                    }
                }

            };
            Context.Events.Add(_event);
            Context.SaveChanges();
        }
        [Fact]
        public async Task Sync_EventBrite_Events()
        {
            await SyncServices.SyncEvent(_event.Id);

        }

    }
}