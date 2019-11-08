//using System.Collections.Generic;
//using System.Linq;
//using System.Net;
//using System.Net.Http;
//using System.Threading.Tasks;
//using FluentAssertions;
//using NetBaires.Api.Models;
//using NetBaires.Data;
//using Xunit;

//namespace NetBaires.Api.Tests.Integration.Events
//{
//    public class GetToSyncShould : IntegrationTestsBase
//    {
//        public GetToSyncShould(CustomWebApplicationFactory<Startup> factory) : base(factory)
//        {
//            AuthenticateAdminAsync().GetAwaiter().GetResult(); ;
//        }

//        [Fact]
//        public async Task Return_Event()
//        {

//            var newEvent = new Event
//            {
//                Done = false,
//                Attendees = new List<Attendance> {
//                            new Attendance{
//                                Attended=false,
//                                DidNotAttend=true
//                            },
//                            new Attendance{
//                                Attended=false,
//                                DidNotAttend=true
//                            },
//                            new Attendance{
//                                Attended=true,
//                                DidNotAttend=false
//                            }
//                           }
//            };
//            Context.Events.Add(newEvent);
//            Context.SaveChanges();
//            var response = await HttpClient.GetAsync("/events/toSync");

//            response.StatusCode.Should().Be(HttpStatusCode.OK);
//            var events = await response.Content.ReadAsAsync<List<GetToSyncResponse>>();
//            events.Count.Should().Be(1);
//            events.First().Attended.Should().Be(1);
//            events.First().DidNotAttend.Should().Be(2);
//            events.First().Registered.Should().Be(3);

//        }

//    }
//}