using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetBaires.Api.Features.Materials.AddMaterial;
using NetBaires.Api.Features.Materials.GetMaterials;
using NetBaires.Api.Features.Materials.RemoveMaterial;
using NetBaires.Data.Entities;
using Swashbuckle.AspNetCore.Annotations;

namespace NetBaires.Api.Features.Materials
{
    [Authorize]
    [ApiController]
    public class MaterialsController : ControllerBase
    {
        private readonly IMediator _iMediator;

        public MaterialsController(IMediator iMediator)
        {
            _iMediator = iMediator;
        }

        [HttpGet("Events/{eventId}/Materials")]
        [SwaggerOperation(Summary = "Retorna todo el material de un evento")]
        [AuthorizeRoles(UserRole.Admin, UserRole.Organizer, UserRole.Member)]
        [ApiExplorerSettingsExtend(UserRole.Member)]
        public async Task<IActionResult> PutCheckAttendanceGeneral([FromRoute] int eventId) =>
            await _iMediator.Send(new GetMaterialsQuery(eventId));


        [HttpPost("Events/{eventId}/Materials")]
        [SwaggerOperation(Summary = "Agrega material al evento")]
        [AuthorizeRoles(UserRole.Admin, UserRole.Organizer)]
        [ApiExplorerSettingsExtend(UserRole.Organizer)]
        public async Task<IActionResult> PutCheckAttendanceGeneral([FromRoute]int eventId, [FromBody] AddMaterialCommand command)
        {
            command.EventId = eventId;
            return await _iMediator.Send(command);
        }

        [HttpDelete("Events/{eventId}/Materials/{MaterialId}")]
        [SwaggerOperation(Summary = "Agrega material al evento")]
        [AuthorizeRoles(UserRole.Admin, UserRole.Organizer)]
        [ApiExplorerSettingsExtend(UserRole.Organizer)]
        public async Task<IActionResult> PutCheckAttendanceGeneral(int eventId, int materialId)
        {
            return await _iMediator.Send(new RemoveMaterialCommand(eventId, materialId));
        }
    }
}