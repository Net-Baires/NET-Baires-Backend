using System.Collections.Generic;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace NetBaires.Api.Features.GroupsCodes.UpdateGroupCode
{
    public class UpdateGroupCodeCommand : IRequest<IActionResult>
    {
        [JsonIgnore]
        public int GroupCodeId { get; set; }
        [FromBody]
        public string Detail { get; set; }
        [FromBody]
        public bool Open { get;  set; }
    }
}