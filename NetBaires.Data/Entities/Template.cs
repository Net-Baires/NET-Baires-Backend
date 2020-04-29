using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;

namespace NetBaires.Data.Entities
{
    public class Template : Entity
    {
        public TemplateTypeEnum Type { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string TemplateContent { get; set; }
        public List<Event> EventsThanksSpeakers { get; set; }
        public List<Event> EventsThanksAttended { get; set; }
        public List<Event> EventsThanksThanksSponsors { get; set; }

    }
}