using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace NetBaires.Api.Features.Templates.GetTemplates
{
    public class GetTemplatesQuery : IRequest<IActionResult>
    {
        public int? Id { get; set; }

        public GetTemplatesQuery(int? id)
        {
            Id = id;
        }

        public GetTemplatesQuery()
        {
        }
    }
}