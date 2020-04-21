using System.Linq;
using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using NetBaires.Data;
using NetBaires.Data.Entities;
using NetBaires.Host;
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