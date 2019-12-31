using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NetBaires.Api.Services.Meetup;
using NetBaires.Api.Services.Meetup.Models;

namespace NetBaires.Api.Tests.Integration.Services
{
    public class MeetupServicesDummy : IMeetupServices
    {

        public Task<List<MeetupEventDetail>> GetAllEvents()
        {
            return Task.FromResult(new List<MeetupEventDetail>
            {
                new MeetupEventDetail
                {
                    Id = 1234,
                    Name = "Evento test Meetup",
                    Description = "Descripción",
                    Link = new Uri("http://event.com.ar"),
                    LocalDate = DateTimeOffset.Now,
                    FeaturedPhoto = new FeaturedPhoto
                    {
                        HighresLink = new Uri("http://HighresLink.com.ar")
                    }
                },
                new MeetupEventDetail
                {
                    Id = 1234567,
                    Name = "Evento test Meetup",
                    Description = "Descripción",
                    Link = new Uri("http://event.com.ar"),
                    LocalDate = DateTimeOffset.Now,
                    FeaturedPhoto = new FeaturedPhoto
                    {
                        HighresLink = new Uri("http://HighresLink.com.ar")
                    }
                }
            });
        }

        public Task<List<AttendanceResponse>> GetAttendees(int eventId)
        {
            return Task.FromResult(new List<AttendanceResponse>
            {
                new AttendanceResponse
                {
                    Member = new Member
                    {
                        Name = "Asistio",
                        Id = 123456,
                        Photo = new Photo
                        {
                            HighresLink = new Uri("http://www.contoso.com.ar/")
                        },
                        Bio = "Biografia"
                    },
                    Status = "attended"
                }, new AttendanceResponse
                {
                    Member = new Member
                    {
                        Name = "NoShow",
                        Id = 1234568,
                        Photo = new Photo
                        {
                            HighresLink = new Uri("http://www.contoso.com.ar/")
                        },
                        Bio = "Biografia"
                    },
                    Status = "noshow"
                },
                new AttendanceResponse
                {
                    Member = new Member
                    {
                        Name = "No Asistio",
                        Id = 1234567,
                        Photo = new Photo
                        {
                            HighresLink = new Uri("http://www.contoso.com.ar/")
                        },
                        Bio = "Biografia"
                    },
                    Status =  "absent"
                }
            });
        }
    }
}