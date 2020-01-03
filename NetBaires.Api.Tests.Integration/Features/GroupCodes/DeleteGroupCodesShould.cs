using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NetBaires.Api.Features.Events.UpdateEvent;
using NetBaires.Api.Features.GroupsCodes.UpdateGroupCode;
using NetBaires.Data;
using NetBaires.Host;
using Newtonsoft.Json;
using Xunit;

namespace NetBaires.Api.Tests.Integration.Features.GroupCodes
{
    public class DeleteGroupCodesShould : IntegrationTestsBase
    {
        private GroupCode _newGroupCode;

        public DeleteGroupCodesShould(CustomWebApplicationFactory<Startup> factory) : base(factory)
        {
            AuthenticateAdminAsync().GetAwaiter().GetResult();
        }

        [Fact]
        public async Task Delete_GroupCode()
        {
            FillData();
            var response = await HttpClient.DeleteAsync($"/groupcodes/{_newGroupCode.Id}");
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
            RefreshContext();
            Context.GroupCodes.Count().Should().Be(0);
        }

       

        private void FillData()
        {
            _newGroupCode = new GroupCode("Detalle");
            Context.GroupCodes.Add(_newGroupCode);
            Context.SaveChanges();
        }
    }
}