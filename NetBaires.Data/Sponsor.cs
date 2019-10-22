using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace NetBaires.Data
{
    public class Sponsor
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [JsonIgnore]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string SiteUrl { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string LogoUrl { get; set; }

        public List<SponsorEvent> Events { get; set; }
        
    }
    public class SponsorEvent
    {
        public int EventId { get; set; }
        public Event Event { get; set; }
        public int SponsorId { get; set; }
        public Sponsor Sponsor { get; set; }
        public string Detail { get; set; }
    }
}
