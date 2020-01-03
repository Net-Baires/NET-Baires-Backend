using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace NetBaires.Api.Features.GroupsCodes.AddMemberToGroupCode
{
    public class AddMemberToGroupCodeCommand : IRequest<IActionResult>
    {
        public int GroupCodeId { get; set; }
        public string Code { get; set; }
    }
}