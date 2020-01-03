using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace NetBaires.Api.Features.GroupsCodes.DeleteGroupCode
{
    public class DeleteGroupCodeCommand : IRequest<IActionResult>
    {
        public int GroupCodeId { get; set; }
    }

}