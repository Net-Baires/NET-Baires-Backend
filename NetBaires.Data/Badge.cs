using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace NetBaires.Data
{
    public class Badge
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [JsonIgnore]
        public int Id { get; set; }
        public string BadgeId { get; set; }
        public string BadgeUrl { get; set; }
        public string BadgeImageUrl { get; set; }
        public string IssuerUrl { get; set; }
        public string Image { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public IList<BadgeMember> Users { get; set; }
        public DateTime Created { get; set; }
    }
}