using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NetBaires.Api.Services.EventBrite;
using NetBaires.Api.Services.EventBrite.Models;

namespace NetBaires.Api.Tests.Integration.Services
{
    public class EventBriteServicesDummy : IEventBriteServices
    {
        public Task<List<Attendee>> GetAttendees(string eventId)
        {
            return Task.FromResult(new List<Attendee> {
                new Attendee {
                    profile = new Profile{
                        Email="Asisto@Asistio.com",
                        FirstName="Asistio",
                        LastName="Asistio"
                    },
                    CheckIn = true
                },
                new Attendee {
                    profile = new Profile{
                        Email="NoAsisto@NoAsistio.com",
                        FirstName="No Asistio",
                        LastName="No Asistio"
                    },
                    CheckIn = false
                }
            });
        }

        public Task<List<Event>> GetEvents()
        {
            return Task.FromResult(new List<Event> {
                new Event{
                    Name= new Description
                    {
                        Text="Evento Meetup",
                        Html="Evento Meetup",
                    },
                    Description = new Description{
                        Text="Descripcion del evento",
                        Html="Descripcion del evento"
                    },
                    Id="123456",
                    Url="url://meetup",
                    Logo = new Logo
                    {
                        Original = new Original
                        {
                            Url = new Uri("http://logo.com")
                        }
                    },
                    Start = new End
                    {
                        Utc = DateTimeOffset.Now
                    }

                }
            });
        }
    }
}