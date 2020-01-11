using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace NetBaires.Api.Features.GroupsCodes.GetGroupCode
{
    public class GetGroupCodeQuery : IRequest<IActionResult>
    {
        public int GroupCodeId { get; set; }
    }

}