using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace NetBaires.Data
{
    public class Badge
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string ImageName { get; set; }
        public string SimpleImageName { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public IList<BadgeMember> Members { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
    }

    public class BadgeGroup
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public IList<Badge> Badges { get; set; } = new List<Badge>();
        public DateTime Created { get; set; } = DateTime.Now;
    }
}