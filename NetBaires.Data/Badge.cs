using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace NetBaires.Data
{
    public class Badge
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string ImageName { get; set; }
        public string ImageUrl { get; set; }
        public string SimpleImageName { get; set; }
        public string SimpleImageUrl { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public IList<BadgeMember> Members { get; set; } = new List<BadgeMember>();
        public DateTime Created { get; set; } = DateTime.Now;
    }
}