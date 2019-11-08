using NetBaires.Api.Services.EventBrite;
using NetBaires.Api.Services.EventBrite.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

public class EventBriteServicesDummy : IEventBriteServices
{
    public Task<List<Attendee>> GetAttendees(string eventId)
    {
        return Task.FromResult(new List<Attendee> {
                new Attendee {
                    profile = new Profile{
                    Email="asisto@asistio.com",
                    FirstName="Asistio",
                    LastName="Asistio"
               },
                CheckIn = true
            },
                 new Attendee {
                    profile = new Profile{
                    Email="noAsisto@noAsistio.com",
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
                   Url="url://meetup"
               }
            });
    }
}