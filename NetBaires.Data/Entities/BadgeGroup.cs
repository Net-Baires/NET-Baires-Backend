using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace NetBaires.Data.Entities
{
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